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
    private Vector3 m_OriginalPosition;

    protected override void Start()
    {
        base.Start();

        m_Clock.ClockUpdatedEvent += OnClockUpdated;

        m_OriginalPosition = transform.position.Copy();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (m_Clock != null)
        {
            m_Clock.ClockUpdatedEvent -= OnClockUpdated;
        }
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
        //Debug.Log("Time left: " + secondsLeft);
        if (m_IsDriving)
            return;

        if (secondsLeft == m_ArriveTime || secondsLeft == m_LeaveTime)
        {
            m_IsDriving = true;
        }
    }

    protected override void OnStartDay()
    {
        base.OnStartDay();

        //Reset everything
        transform.position = m_OriginalPosition;
        m_ReachedBusStop = false;
    }
}
