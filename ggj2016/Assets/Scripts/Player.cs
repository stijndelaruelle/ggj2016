using UnityEngine;
using System.Collections;
using Sjabloon;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int m_PlayerID;

    [SerializeField]
    private CharacterController2D m_CharacterController;

    [SerializeField]
    private float m_Speed;

    private InputManager m_InputManager;

    private void Start()
    {
        InitializeControls();
    }

    private void OnDestroy()
    {
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
        m_InputManager.BindButton("Action_" + m_PlayerID, KeyCode.Space, InputManager.ButtonState.OnPress);
        m_InputManager.BindButton("Action_" + m_PlayerID, m_PlayerID, ControllerButtonCode.A, InputManager.ButtonState.OnPress);
    }

    private void Update()
    {
        UpdateMovement();
    }

    public void UpdateMovement()
    {
        //Horizontal
        float horizInput = m_InputManager.GetAxis("HorizontalAxis_" + m_PlayerID);
        float vertInput = m_InputManager.GetAxis("VerticalAxis_" + m_PlayerID);

        m_CharacterController.Move(horizInput * m_Speed, vertInput * m_Speed);

        //Action
        bool didAction = m_InputManager.GetButton("Action_" + m_PlayerID);

        if (didAction)
        {
            Debug.Log("Player " + m_PlayerID + " did an action!");
        }
    }
}
