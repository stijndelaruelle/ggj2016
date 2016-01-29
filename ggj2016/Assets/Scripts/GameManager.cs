using UnityEngine;
using System.Collections;
using Sjabloon;

public delegate void VoidDelegate();
public delegate void IntDelegate(int i);

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Clock m_Clock;

    //Events
    private VoidDelegate m_StartGameEvent;
    public VoidDelegate StartGameEvent
    {
        get { return m_StartGameEvent; }
        set { m_StartGameEvent = value; }
    }

    private VoidDelegate m_EndGameEvent;
    public VoidDelegate EndGameEvent
    {
        get { return m_EndGameEvent; }
        set { m_EndGameEvent = value; }
    }

    private VoidDelegate m_StartDayEvent;
    public VoidDelegate StartDayEvent
    {
        get { return m_StartDayEvent; }
        set { m_StartDayEvent = value; }
    }

    private VoidDelegate m_EndDayEvent;
    public VoidDelegate EndDayEvent
    {
        get { return m_EndDayEvent; }
        set { m_EndDayEvent = value; }
    }

    //Functions
    public void StartGame()
    {
        Debug.Log("START GAME!");

        if (m_StartGameEvent != null)
            m_StartGameEvent();

        StartDay();
    }

    private void EndGame()
    {
        Debug.Log("END GAME!");

        if (m_EndGameEvent != null)
            m_EndGameEvent();
    }

    public void StartDay()
    {
        Debug.Log("START DAY!");

        if (m_StartDayEvent != null)
            m_StartDayEvent();
    }

    private void EndDay()
    {
        Debug.Log("END DAY!");

        if (m_EndDayEvent != null)
            m_EndDayEvent();
    }
}
