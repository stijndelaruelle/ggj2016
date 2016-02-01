using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VisualClock : MonoBehaviour
{
    [SerializeField]
    private Text m_Text;

    [SerializeField]
	private Clock m_Clock;

    private void Start()
    {
        m_Clock.ClockUpdatedEvent += OnClockUpdate;
    }

    private void OnDestroy()
    {
        if (m_Clock == null)
            return;

        m_Clock.ClockUpdatedEvent -= OnClockUpdate;
    }

    private void OnClockUpdate(int timeLeft)
    {
        if (timeLeft <= 0)
        {
            m_Text.text = "00 : 00";
            return;
        }

        int minutes = Mathf.FloorToInt(timeLeft / 60);
        timeLeft -= minutes * 60;

        int seconds = timeLeft;

        m_Text.text = minutes.ToString("00") + " : " + seconds.ToString("00");
    }
}
