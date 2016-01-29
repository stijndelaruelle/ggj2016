using UnityEngine;
using System.Collections;
using Sjabloon;
using System.Collections.Generic;

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
    private CharacterController2D m_CharacterController;

    [SerializeField]
    private float m_Speed;

    [SerializeField]
    private List<Task> m_Tasks;

    private InputManager m_InputManager;
    private InteractableObject m_CurrentInteractableObject;

    //Events
    private VoidDelegate m_OnTaskListUpdated;
    public VoidDelegate OnTaskListUpdated
    {
        get { return m_OnTaskListUpdated; }
        set { m_OnTaskListUpdated = value; }
    }

    //Functions
    private void Start()
    {
        InitializeControls();

        m_CharacterController.OnTriggerEnterEvent += OnCustomTriggerEnter;
        m_CharacterController.OnTriggerExitEvent += OnCustomTriggerExit;
    }

    private void OnDestroy()
    {
        if (m_CharacterController == null)
            return;

        m_CharacterController.OnTriggerEnterEvent -= OnCustomTriggerEnter;
        m_CharacterController.OnTriggerExitEvent -= OnCustomTriggerExit;
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
        UpdateMovement();
        UpdateAction();
    }

    private void UpdateMovement()
    {
        //Horizontal
        float horizInput = m_InputManager.GetAxis("HorizontalAxis_" + m_PlayerID);
        float vertInput = m_InputManager.GetAxis("VerticalAxis_" + m_PlayerID);

        m_CharacterController.Move(horizInput * m_Speed, vertInput * m_Speed);
    }

    private void UpdateAction()
    {
        bool didAction = m_InputManager.GetButton("Action_" + m_PlayerID);

        if (didAction && m_CurrentInteractableObject != null)
        {
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

                if (m_OnTaskListUpdated != null)
                    m_OnTaskListUpdated();

                return;
            }
        }

    }

    private void OnCustomTriggerEnter(Collider2D other)
    {
        m_CurrentInteractableObject = other.gameObject.GetComponent<InteractableObject>();
        Debug.Log(m_CurrentInteractableObject);
    }

    private void OnCustomTriggerExit(Collider2D other)
    {
        m_CurrentInteractableObject = null;
        Debug.Log(m_CurrentInteractableObject);
    }
}
