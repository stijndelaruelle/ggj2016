using UnityEngine;
using System.Collections;

public class Bus : Vehicle
{
    [SerializeField]
    private Clock m_Clock;

    [SerializeField]
    private int m_ArriveTime;

    [SerializeField]
    private int m_LeaveTime;

    [SerializeField]
    private Transform m_BusStop;
    private bool m_ReachedBusStop = false;

    protected override void Start()
    {
        base.Start();
        m_Clock.ClockUpdatedEvent += OnClockUpdated;
    }

    private void OnDestroy()
    {
        if (m_Clock == null)
            return;

        m_Clock.ClockUpdatedEvent -= OnClockUpdated;
    }

    protected override void Update()
    {
        base.Update();

        if (m_ReachedBusStop)
            return;

        if (transform.position.x > m_BusStop.position.x)
        {
            m_IsDriving = false;
            m_ReachedBusStop = true;
        }   
    }

    private void OnClockUpdated(int secondsLeft)
    {
        Debug.Log("Time left: " + secondsLeft);
        if (m_IsDriving)
            return;

        if (secondsLeft == m_ArriveTime || secondsLeft == m_LeaveTime)
        {
            m_IsDriving = true;
        }
    }
}
