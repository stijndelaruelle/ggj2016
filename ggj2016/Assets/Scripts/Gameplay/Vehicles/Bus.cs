using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bus : Vehicle
{
    [SerializeField]
    private Clock m_Clock;

    [SerializeField]
    private int m_ArriveTime;

    [SerializeField]
    private int m_LeaveTime;

    protected override void Start()
    {
        base.Start();

        m_Clock.ClockUpdatedEvent += OnClockUpdated;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (m_Clock != null)
        {
            m_Clock.ClockUpdatedEvent -= OnClockUpdated;
        }
    }

    private void OnClockUpdated(int secondsLeft)
    {
        //Debug.Log("Time left: " + secondsLeft);
        if (m_IsDriving)
            return;

        if (secondsLeft == m_ArriveTime || secondsLeft <= m_LeaveTime)
        {
            m_IsDriving = true;
        }
    }
}
