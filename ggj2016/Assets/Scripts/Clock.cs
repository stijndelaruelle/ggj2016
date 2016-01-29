using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour
{
    [SerializeField]
    private float m_TimePerDay;
    private float m_TimeLeft;

    //Events
    private IntDelegate m_ClockUpdatedEvent;
    public IntDelegate ClockUpdatedEvent
    {
        get { return m_ClockUpdatedEvent; }
        set { m_ClockUpdatedEvent = value; }
    }

    private void Start()
    {
        GameManager.Instance.StartDayEvent += OnStartDay;
    }

    private void Update()
    {
        int prevSeconds = TimeLeftInSeconds();
        m_TimeLeft -= Time.deltaTime;
        int seconds = TimeLeftInSeconds();

        //We only send an event every second
        if (seconds != prevSeconds)
        {
            Debug.Log("Time left: " + seconds);

            if (m_ClockUpdatedEvent != null)
                m_ClockUpdatedEvent(seconds);
        }
    }

    public int TimeLeftInSeconds()
    {
        return Mathf.FloorToInt(m_TimeLeft);
    }

    private void OnStartDay()
    {
        m_TimeLeft = m_TimePerDay;
    }
}
