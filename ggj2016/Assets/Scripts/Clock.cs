using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour
{
    [SerializeField]
    private float m_TimePerDay;
    private float m_TimeLeft;

    private bool m_IsTicking = false;

    //Events
    private IntDelegate m_ClockUpdatedEvent;
    public IntDelegate ClockUpdatedEvent
    {
        get { return m_ClockUpdatedEvent; }
        set { m_ClockUpdatedEvent = value; }
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Instance;

        gameManager.StartDayEvent += OnStartDay;
        gameManager.EndDayEvent += OnEndDay;
    }

    private void OnDestroy()
    {
        GameManager gameManager = GameManager.Instance;
        if (gameManager == null)
            return;

        gameManager.StartDayEvent -= OnStartDay;
        gameManager.EndDayEvent -= OnEndDay;
    }

    private void Update()
    {
        if (!m_IsTicking)
            return;

        int prevSeconds = TimeLeftInSeconds();
        m_TimeLeft -= Time.deltaTime;
        int seconds = TimeLeftInSeconds();

        //We only send an event every second
        if (seconds != prevSeconds)
        {
            //Debug.Log("Time left: " + seconds);

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
        m_IsTicking = true;
    }

    private void OnEndDay()
    {
        m_IsTicking = false;
    }
}
