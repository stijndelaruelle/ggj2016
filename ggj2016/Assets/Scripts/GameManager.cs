using UnityEngine;
using System.Collections;
using Sjabloon;
using System.Collections.Generic;
using UnityEngine.Events;

public delegate void VoidDelegate();
public delegate void IntDelegate(int i);

[System.Serializable]
public class VoidEvent : UnityEvent { }

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private List<Player> m_Players;
    private int m_PlayersLeftScreen = 0;

    private int m_Day;
    public int Day
    {
        get { return m_Day; }
    }

    //Unity events
    private VoidEvent m_StartGameUnityEvent;

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

    private VoidDelegate m_CreateDayEvent;
    public VoidDelegate CreateDayEvent
    {
        get { return m_CreateDayEvent; }
        set { m_CreateDayEvent = value; }
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
    private void Start()
    {
        foreach(Player player in m_Players)
        {
            player.LeftScreenEvent += OnPlayerLeftScreen;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        foreach (Player player in m_Players)
        {
            if (player != null)
                player.LeftScreenEvent -= OnPlayerLeftScreen;
        }
    }

    public void StartGame()
    {
        Debug.Log("START GAME!");

        m_Day = 0;

        if (m_StartGameEvent != null)
            m_StartGameEvent();

        CreateDay();
    }

    private void EndGame()
    {
        Debug.Log("END GAME!");

        if (m_EndGameEvent != null)
            m_EndGameEvent();
    }

    public void CreateDay()
    {
        ++m_Day;

        if (m_CreateDayEvent != null)
            m_CreateDayEvent();
    }

    public void StartDay()
    {
        Debug.Log("START DAY!");
        m_PlayersLeftScreen = 0;

        if (m_StartDayEvent != null)
            m_StartDayEvent();
    }

    private void EndDay()
    {
        Debug.Log("END DAY!");

        if (m_EndDayEvent != null)
            m_EndDayEvent();
    }


    private void OnPlayerLeftScreen()
    {
        Debug.Log("PLAYER LEFT THE SCREEN!");

        ++m_PlayersLeftScreen;

        //When all the players left the screen, end the day
        if (m_PlayersLeftScreen >= m_Players.Count)
        {
            EndDay();
        }
    }

    public bool AllChildrenInVehicle()
    {
        foreach(Player player in m_Players)
        {
            if (player.PlayerType == PlayerType.Child)
            {
                if (player.IsInVehicle == false)
                    return false;
            }
        }

        return true;
    }
}
