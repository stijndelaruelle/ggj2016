using UnityEngine;
using System.Collections;
using Sjabloon;

public class ExecuteFunctionOnButtonPress : MonoBehaviour
{
    [SerializeField]
    private KeyCode m_KeyCode;

    [SerializeField]
    private ControllerButtonCode m_ControllerButtonCode;

    [SerializeField]
    private VoidEvent m_Function;

    private InputManager m_InputManager;

    private void Start()
    {
        m_InputManager = InputManager.Instance;

        //I need to make a generic BindButton function for controllers
        for (int i = 0; i < 4; ++i)
        {
            m_InputManager.BindButton("ExecuteFunction_" + i, m_KeyCode, InputManager.ButtonState.OnPress);
            m_InputManager.BindButton("ExecuteFunction_" + i, i, m_ControllerButtonCode, InputManager.ButtonState.OnPress);
        }
    }

    private void Update()
    {
        for (int i = 0; i < 4; ++i)
        {
            bool pressed = m_InputManager.GetButton("ExecuteFunction_" + i);

            if (pressed)
            {
                m_Function.Invoke();
                return;
            }
        }
    }
}
