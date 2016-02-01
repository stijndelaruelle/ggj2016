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
    public int PlayerID
    {
        get { return m_PlayerID; }
    }

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
    private float m_MaxSpeed;

    private float m_OriginalSpeed;

    [SerializeField]
    private List<Task> m_Tasks;
    public List<Task> Tasks
    {
        get { return m_Tasks; }
        set
        {
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
    private List<InteractableObject> m_CurrentInteractableObjects;

    private Vector3 m_OriginalPosition;

    private Vehicle m_CurrentVehicle;
    public Vehicle CurrentVehicle
    {
        get { return m_CurrentVehicle; }
    }

    private bool m_IsOnScreen;
    public bool IsOnScreen
    {
        get { return m_IsOnScreen; }
    }

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

    private AudioController m_PlayerAudio;
    public AudioController PlayerAudio
    {
        get { return m_PlayerAudio; }
    }

    //Events
    private VoidDelegate m_TaskListUpdatedEvent;
    public VoidDelegate TaskListUpdatedEvent
    {
        get { return m_TaskListUpdatedEvent; }
        set { m_TaskListUpdatedEvent = value; }
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
        m_OriginalSpeed = m_Speed;

        InitializeControls();

        m_Icon.Initialize();
        m_CharacterAnimation.Initialize();
        m_assignedTaskList.UpdateTaskList(m_Tasks);
        m_PlayerAudio = GetComponentInChildren<AudioController>();

        m_CurrentInteractableObjects = new List<InteractableObject>();
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

        //PC couldn't handle 4 controllers (super lame, no time)
        if (m_PlayerID == 2)
        {
            m_InputManager.BindAxis("HorizontalAxis_" + m_PlayerID, KeyCode.D, KeyCode.A);
            m_InputManager.BindAxis("VerticalAxis_" + m_PlayerID, KeyCode.W, KeyCode.S);
            m_InputManager.BindButton("Action_" + m_PlayerID, KeyCode.LeftControl, InputManager.ButtonState.OnPress);
        }

        if (m_PlayerID == 3)
        {
            m_InputManager.BindAxis("HorizontalAxis_" + m_PlayerID, KeyCode.RightArrow, KeyCode.LeftArrow);
            m_InputManager.BindAxis("VerticalAxis_" + m_PlayerID, KeyCode.UpArrow, KeyCode.DownArrow);
            m_InputManager.BindButton("Action_" + m_PlayerID, KeyCode.RightControl, InputManager.ButtonState.OnPress);
        }

        m_InputManager.BindAxis("HorizontalAxis_" + m_PlayerID, m_PlayerID, ControllerButtonCode.Right, ControllerButtonCode.Left);
        m_InputManager.BindAxis("HorizontalAxis_" + m_PlayerID, m_PlayerID, ControllerAxisCode.LeftStickX);

        m_InputManager.BindAxis("VerticalAxis_" + m_PlayerID, m_PlayerID, ControllerButtonCode.Up, ControllerButtonCode.Down);
        m_InputManager.BindAxis("VerticalAxis_" + m_PlayerID, m_PlayerID, ControllerAxisCode.LeftStickY);

        //Action
        m_InputManager.BindButton("Action_" + m_PlayerID, m_PlayerID, ControllerButtonCode.A, InputManager.ButtonState.OnPress);
    }

    private void Update()
    {
        if (!m_IsOnScreen)
            return;

        UpdateMovement();
        UpdateAction();
        UpdateIcon();

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
        if (IsInVehicle())
            return;

        //Horizontal
        float horizInput = m_InputManager.GetAxis("HorizontalAxis_" + m_PlayerID);
        float vertInput = m_InputManager.GetAxis("VerticalAxis_" + m_PlayerID);

        m_Velocity = m_CharacterController.Move(horizInput * m_Speed, vertInput * m_Speed);
    }

    private void UpdateAction()
    {
        bool didAction = m_InputManager.GetButton("Action_" + m_PlayerID);

        if (didAction)
        {
            foreach (InteractableObject obj in m_CurrentInteractableObjects)
            {
                if (obj.CanInteract(this))
                    obj.Interact(this);
            }
        }
    }

    public void UpdateTask(TaskDefinition taskDefinition)
    {
        foreach (Task task in m_Tasks)
        {
            if (task.TaskDefinition == taskDefinition &&
                task.IsDone == false)
            {
                task.IsDone = true;

                // Stop playing audio
                m_PlayerAudio.Stop();

                Debug.Log("Task: " + taskDefinition.Title + " completed!");

                if (m_TaskListUpdatedEvent != null)
                    m_TaskListUpdatedEvent();

                return;
            }
        }

    }

    public void UpdateVehicle(Vehicle vehicle)
    {
        if (vehicle != null)
        {
            m_CurrentVehicle = vehicle;
            m_SpriteRenderer.enabled = false;
            m_CharacterController.BoxCollider.enabled = false;
            m_Icon.gameObject.SetActive(false);
            transform.SetParent(vehicle.transform);
        }
        else
        {
            m_CurrentVehicle = null;
            m_SpriteRenderer.enabled = true;
            m_CharacterController.BoxCollider.enabled = true;
            m_Icon.gameObject.SetActive(true);
            transform.SetParent(null);
        }
    }

    public bool IsInVehicle()
    {
        return (m_CurrentVehicle != null);
    }

    public void ModifySpeed(float modifier)
    {
        m_Speed += modifier;

        if (m_Speed > m_MaxSpeed)
            m_Speed = m_MaxSpeed;
    }

    private void OnCustomTriggerEnter(Collider2D other)
    {
        InteractableObject temp = other.gameObject.GetComponent<InteractableObject>();
        if (temp == null)
            return;

        if (temp.IsUnlocked())
        {
            m_CurrentInteractableObjects.Add(temp);
        }
    }

    private void OnCustomTriggerExit(Collider2D other)
    {
        InteractableObject temp = other.gameObject.GetComponent<InteractableObject>();
        if (temp == null)
            return;

        if (!temp.IsUnlocked())
            return;

        foreach (InteractableObject obj in m_CurrentInteractableObjects)
        {
            if (obj.IsInteracting(this))
            {
                obj.CancelInteraction(this);
                m_Icon.Fail(true);
            }
        }

        m_CurrentInteractableObjects.Remove(temp);
    }

    private void UpdateIcon()
    {
        if (m_CurrentInteractableObjects.Count == 0)
        {
            m_Icon.Hide();
            return;
        }

        foreach (InteractableObject obj in m_CurrentInteractableObjects)
        {
            if (obj.CanInteract(this))
            {
                m_Icon.Reset();
                m_Icon.ShowSprite(m_Icon._properties._standardSprite);
                return;
            }
        }

        if (m_Icon.IsUsingDefaultSprite())
            m_Icon.Hide();
    }

    private void OnStartDay()
    {
        //Reset everything
        UpdateVehicle(null);
        m_Icon.Hide();

        transform.position = m_OriginalPosition;
        m_Speed = m_OriginalSpeed;

        m_IsOnScreen = true;
        m_TimeScreenLeft = 0;
        m_CurrentInteractableObjects.Clear();

        foreach (Task task in m_Tasks)
        {
            task.IsDone = false;
        }

        if (m_TaskListUpdatedEvent != null)
            m_TaskListUpdatedEvent();
    }
}
