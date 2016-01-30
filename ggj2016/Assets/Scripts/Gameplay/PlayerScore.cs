using UnityEngine;
using System.Collections;

public class PlayerScore : MonoBehaviour
{
    public enum TimeScore
    {
        None = 0,
        VeryEarly = 1,
        Early = 2,
        OnTime = 3,
        Late = 4,
        VeryLate = 5,
        NeverArrived = 6
    }

    [SerializeField]
    private Player m_Player;
    public Player Player
    {
        get { return m_Player; }
    }

    private TimeScore m_LastTimeScore;
    public TimeScore LastTimeScore
    {
        get { return m_LastTimeScore; }
    }

    private TimeScore m_CurrentTimeScore;
    public TimeScore CurrentTimeScore
    {
        get { return m_CurrentTimeScore; }
    }

    private int m_LastTotalTimeScore; //negative is too late
    public int LastTotalTimeScore
    {
        get { return m_LastTotalTimeScore; }
    }

    private int m_CurrentTotalTimeScore; //negative is too late
    public int CurrentTotalTimeScore
    {
        get { return m_CurrentTotalTimeScore; }
    }

    private void Start()
    {
        GameManager.Instance.EndDayEvent += OnDayEnd;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.EndDayEvent -= OnDayEnd;
    }

    private void OnDayEnd()
    {
        CalculateOnTimeScore();

        if (m_CurrentTotalTimeScore < -3)
        {
            //Ontslagen of van school getrapt
        }
    }

    private void CalculateOnTimeScore()
    {
        int timeLeft = m_Player.TimeScreenLeft;

        m_LastTotalTimeScore = m_CurrentTotalTimeScore;
        m_LastTimeScore = m_CurrentTimeScore;

        //if we're a parent, and we took the schoolbus, we never arrive!
        if (m_Player.PlayerType == PlayerType.Parent)
        {

        }

        //We're very early
        if (timeLeft > 10)
        {
            m_CurrentTimeScore = TimeScore.VeryEarly;
            m_CurrentTotalTimeScore += 2;
        }
           
        //We're early
        else if (timeLeft > 5)
        {
            m_CurrentTimeScore = TimeScore.Early;
            m_CurrentTotalTimeScore += 1;
        }
           
        //We're on time (this is expected
        else if (timeLeft > 0)
        {
            m_CurrentTimeScore = TimeScore.OnTime;
        }

        //We're late
        else if (timeLeft > -5)
        {
            m_CurrentTimeScore = TimeScore.Late;
            m_CurrentTotalTimeScore -= 1;
        }

        //We're VERY late!
        else if (timeLeft > -10)
        {
            m_CurrentTimeScore = TimeScore.VeryLate;
            m_CurrentTotalTimeScore -= 2;
        }
    }
}
