using UnityEngine;
using System.Collections;
using Sjabloon;
using System.Collections.Generic;

public enum PlayerType
{
    Parent = 0,
    Child = 1
}

public enum GenderType
{
    Male = 0,
    Female = 1
}

[System.Serializable]
public class Task
{
    [SerializeField]
    private TaskDefinition m_TaskDefinition;
    public TaskDefinition TaskDefinition
    {
        get { return m_TaskDefinition; }
    }

    private bool m_IsDone;
    public bool IsDone
    {
        get { return m_IsDone; }
        set { m_IsDone = value; }
    }
}

public class Player : MonoBehaviour
{
    [SerializeField]
    private int m_PlayerID;

    [SerializeField]
    private string m_PlayerName;
    public string Name
    {
        get { return m_PlayerName; }
    }

    [SerializeField]
    private PlayerType m_PlayerType;
    public PlayerType PlayerType
    {
        get { return m_PlayerType; }
    }

    [SerializeField]
    private GenderType m_Gender;
    public GenderType Gender
    {
        get { return m_Gender; }
    }

    [SerializeField]
    private CharacterController2D m_CharacterController;

    [SerializeField]
    private SpriteRenderer m_SpriteRenderer;

    [SerializeField]
    private float m_Speed;

    [SerializeField]
    private List<Task> m_Tasks;
    public List<Task> Tasks
    {
        get { return m_Tasks; }
        set {
			m_Tasks = value;
		}
    }

    [SerializeField]
    private Clock m_Clock;

    [SerializeField]
    private Icon m_Icon;
    public Icon Icon
    {
        get { return m_Icon; }
        set { m_Icon = value; }
    }

	[SerializeField]
	private CharacterAnimation m_CharacterAnimation;
	public CharacterAnimation CharacterAnimation
	{
		get { return m_CharacterAnimation; }
		set { m_CharacterAnimation = value; }
	}

	private InputManager m_InputManager;
    private InteractableObject m_CurrentInteractableObject;
    private Vector3 m_OriginalPosition;

    private bool m_IsInVehicle;
    public bool IsInVehicle
    {
        get { return m_IsInVehicle; }
    }

    private bool m_IsOnScreen;
    private int m_TimeScreenLeft = 0; //When did we leave the screen?
    public int TimeScreenLeft
    {
        get { return m_TimeScreenLeft; }
    }

	private Vector2 m_Velocity = Vector2.zero; 
	public Vector2 Velocity
	{
		get { return m_Velocity; }
	}

	[SerializeField]
	private TaskListUI m_assignedTaskList;
	public TaskListUI AssignedTaskList
	{
		get
		{
			return m_assignedTaskList;
		}

		set
		{
			m_assignedTaskList = value;
		}
	}

	//Events
	private VoidDelegate m_TaskListUpdatedEvent;
    public VoidDelegate TaskListUpdatedEvent
    {
        get { return m_TaskListUpdatedEvent; }
        set { m_TaskListUpdatedEvent = value;}
    }

    private VoidDelegate m_LeftScreenEvent;
    public VoidDelegate LeftScreenEvent
    {
        get { return m_LeftScreenEvent; }
        set { m_LeftScreenEvent = value; }

    }

	//Functions
	private void Start()
    {
        GameManager.Instance.StartDayEvent += OnStartDay;

        m_CharacterController.OnTriggerEnterEvent += OnCustomTriggerEnter;
        m_CharacterController.OnTriggerExitEvent += OnCustomTriggerExit;

        m_OriginalPosition = transform.position.Copy();

        InitializeControls();

        m_Icon.Initialize();
	    m_CharacterAnimation.Initialize();
		m_assignedTaskList.UpdateTaskList(m_Tasks);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartDayEvent -= OnStartDay;
        }

        if (m_CharacterController == null)
        {
            m_CharacterController.OnTriggerEnterEvent -= OnCustomTriggerEnter;
            m_CharacterController.OnTriggerExitEvent -= OnCustomTriggerExit;
        }
    }

    private void InitializeControls()
    {
        m_InputManager = InputManager.Instance;

        //Movement
        m_InputManager.BindAxis("HorizontalAxis_" + m_PlayerID, KeyCode.RightArrow, KeyCode.LeftArrow);
        m_InputManager.BindAxis("HorizontalAxis_" + m_PlayerID, m_PlayerID, ControllerButtonCode.Right, ControllerButtonCode.Left);
        m_InputManager.BindAxis("HorizontalAxis_" + m_PlayerID, m_PlayerID, ControllerAxisCode.LeftStickX);

        m_InputManager.BindAxis("VerticalAxis_" + m_PlayerID, KeyCode.UpArrow, KeyCode.DownArrow);
        m_InputManager.BindAxis("VerticalAxis_" + m_PlayerID, m_PlayerID, ControllerButtonCode.Up, ControllerButtonCode.Down);
        m_InputManager.BindAxis("VerticalAxis_" + m_PlayerID, m_PlayerID, ControllerAxisCode.LeftStickY);

        //Action
        m_InputManager.BindButton("Action_" + m_PlayerID, KeyCode.Z, InputManager.ButtonState.OnPress);
        m_InputManager.BindButton("Action_" + m_PlayerID, m_PlayerID, ControllerButtonCode.A, InputManager.ButtonState.OnPress);
    }

    private void Update()
    {
        if (!m_IsOnScreen)
            return;

        UpdateMovement();
        UpdateAction();

        //If we're off screen, send an event!
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPos.x < 0.0f || viewPos.x > 1.0f || viewPos.y < 0.0f || viewPos.y > 1.0f)
        {
            m_IsOnScreen = false;
            m_TimeScreenLeft = m_Clock.TimeLeftInSeconds();

            if (m_LeftScreenEvent != null)
                m_LeftScreenEvent();
        }
    }

    private void UpdateMovement()
    {
        if (m_IsInVehicle)
            return;

        //Horizontal
        float horizInput = m_InputManager.GetAxis("HorizontalAxis_" + m_PlayerID);
        float vertInput = m_InputManager.GetAxis("VerticalAxis_" + m_PlayerID);

        m_Velocity = m_CharacterController.Move(horizInput * m_Speed, vertInput * m_Speed);
    }

    private void UpdateAction()
    {
        bool didAction = m_InputManager.GetButton("Action_" + m_PlayerID);

        if (didAction && m_CurrentInteractableObject != null)
        {
            if (m_CurrentInteractableObject.CanInteract(this))
                m_CurrentInteractableObject.Interact(this);
        }
    }

    public void UpdateTask(TaskDefinition taskDefinition)
    {
        foreach(Task task in m_Tasks)
        {
            if (task.TaskDefinition == taskDefinition &&
                task.IsDone == false)
            {
                task.IsDone = true;

                Debug.Log("Task: " + taskDefinition.Title + " completed!");

                if (m_TaskListUpdatedEvent != null)
                    m_TaskListUpdatedEvent();

				m_assignedTaskList.UpdateTaskList(m_Tasks);

				return;
            }
        }

    }

    public void UpdateVehicle(Vehicle vehicle)
    {
        if (vehicle != null)
        {
            m_IsInVehicle = true;
            m_SpriteRenderer.enabled = false;
            transform.SetParent(vehicle.transform);
        }
        else
        {
            m_IsInVehicle = false;
            m_SpriteRenderer.enabled = true;
            transform.SetParent(null);
        }
    }


    private void OnCustomTriggerEnter(Collider2D other)
    {
        m_CurrentInteractableObject = other.gameObject.GetComponent<InteractableObject>();
        Debug.Log(m_CurrentInteractableObject);

		m_Icon.Reset();
        m_Icon.ShowSprite(m_Icon._properties._standardSprite);
    }

    private void OnCustomTriggerExit(Collider2D other)
    {
        if (m_CurrentInteractableObject != null &&
            m_CurrentInteractableObject.IsInteracting(this))
        {
            m_CurrentInteractableObject.Interact(null);
        }

        m_CurrentInteractableObject = null;
        Debug.Log(m_CurrentInteractableObject);

        m_Icon.Reset();
    }


    private void OnStartDay()
    {
        //Reset everything
        UpdateVehicle(null);

        transform.position = m_OriginalPosition;
        
        m_IsOnScreen = true;
        m_TimeScreenLeft = 0;

        m_IsInVehicle = false;
        m_CurrentInteractableObject = null;

        if (m_TaskListUpdatedEvent != null)
            m_TaskListUpdatedEvent();
    }
}
