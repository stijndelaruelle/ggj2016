using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public delegate void GameEndDelegate(Player player, TaskCategoryType type, bool victory);

public enum Rating
{
    None = 0,
    VeryGood = 1,
    Good = 2,
    Normal = 3,
    Bad = 4,
    VeryBad = 5,
    Terrible = 6
}

public class ScoreGroup
{
    private Rating m_LastScore;
    public Rating LastScore
    {
        get { return m_LastScore; }
    }

    private Rating m_Score;
    public Rating Score
    {
        get { return m_Score; }
    }

    private int m_LastTotalScore;
    public int LastTotalScore
    {
        get { return m_LastTotalScore; }
    }

    private int m_TotalScore;
    public int TotalScore
    {
        get { return m_TotalScore; }
    }

    public void SetRating(int value)
    {
        Rating rating = Rating.None;

        if (value > 3)      rating = Rating.VeryGood;
        else if (value > 1) rating = Rating.Good;
        else if(value >= 0) rating = Rating.Normal;
        else if(value > -2) rating = Rating.Bad;
        else                rating = Rating.VeryBad;

        SetRating(rating);
    }

    public void SetRating(Rating rating)
    {
        m_LastScore = m_Score;
        m_Score = rating;
    }

    public void AddToTotal(int value)
    {
        m_LastTotalScore = m_TotalScore;
        m_TotalScore += value;
    }

    public void Reset()
    {
        m_LastScore = Rating.None;
        m_Score = Rating.None;
        m_LastTotalScore = 0;
        m_TotalScore = 0;
    }
}

public class PlayerScore : MonoBehaviour
{
    [SerializeField]
    private Player m_Player;
    public Player Player
    {
        get { return m_Player; }
    }

    private List<ScoreGroup> m_ScoreGroups;

    //Events
    private GameEndDelegate m_EndGameEvent;
    public GameEndDelegate EndGameEvent
    {
        get { return m_EndGameEvent; }
        set { m_EndGameEvent = value; }
    }

    private void Start()
    {
        GameManager.Instance.StartGameEvent += OnGameStart;

        int num = Enum.GetNames(typeof(TaskCategoryType)).Length;

        m_ScoreGroups = new List<ScoreGroup>();
        for (int i = 0; i < num; ++i)
        {
            m_ScoreGroups.Add(new ScoreGroup());
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.StartGameEvent -= OnGameStart;
    }

    public void Calculate()
    {
        CalculateOnTimeScore();
        CalculateTaskScore();

        //Check for victory & gameover
        bool victory = true;
        TaskCategoryType bestStat = TaskCategoryType.Time;
        int bestStatScore = -9999;

        for (int i = 0; i < m_ScoreGroups.Count; ++i)
        {
            if (m_ScoreGroups[i].TotalScore > bestStatScore)
                bestStat = (TaskCategoryType)i;

            //To win, we need all stats at 2 or above
            if (m_ScoreGroups[i].TotalScore < 2)
            {
                victory = false;
            }

            //To lose, we need 1 stat at -3 or lower
            if (m_ScoreGroups[i].TotalScore <= -3)
            {
                if (m_EndGameEvent != null)
                    m_EndGameEvent(m_Player, (TaskCategoryType)i, false);

                return;
            }
        }

        if (victory)
        {
            if (m_EndGameEvent != null)
                m_EndGameEvent(m_Player, bestStat, true);
        }
    }

    private void CalculateOnTimeScore()
    {
        int timeLeft = m_Player.TimeScreenLeft;
        ScoreGroup scoreGroup = m_ScoreGroups[(int)TaskCategoryType.Time];

        //if we're a parent, and we took the schoolbus, we never arrive!
        if (m_Player.PlayerType == PlayerType.Parent)
        {
            Vehicle vehicle = m_Player.CurrentVehicle;
            if (vehicle != null && vehicle.CurrentVehicleType == Vehicle.VehicleType.Bus)
            {
                scoreGroup.SetRating(Rating.Terrible);
                scoreGroup.AddToTotal(-100);
                return;
            }
        }

        //If we're a parent, and we had to bring a kid to school, we remove some time.
        if (m_Player.PlayerType == PlayerType.Parent &&
            GameManager.Instance.AllChildrenOnBus() == false)
        {
            timeLeft -= 10;
        }

        //We're very early
        if (timeLeft > 10)
        {
            scoreGroup.SetRating(Rating.VeryGood);
            scoreGroup.AddToTotal(2);
            return;
        }
           
        //We're early
        if (timeLeft > 5)
        {
            scoreGroup.SetRating(Rating.Good);
            scoreGroup.AddToTotal(1);
            return;
        }
           
        //We're on time (this is expected
        if (timeLeft >= 0)
        {
            scoreGroup.SetRating(Rating.Normal);
            return;
        }

        //We're late
        if (timeLeft > -5)
        {
            scoreGroup.SetRating(Rating.Bad);
            scoreGroup.AddToTotal(-1);
            return;
        }

        //We're VERY late!
        scoreGroup.SetRating(Rating.VeryBad);
        scoreGroup.AddToTotal(-2);
    }

    private void CalculateTaskScore()
    {
        //Go trough all the tasks that collegues comment on and calculate the score
        List<int> scoreList = new List<int>();
        
        for (int i = 0; i < m_ScoreGroups.Count; ++i)
        {
            scoreList.Add(0);
        }

        foreach (Task task in m_Player.Tasks)
        {
            //Only count if it's unlocked
            if (GameManager.Instance.Day >= task.TaskDefinition.UnlockDay)
            {
                int isDone = -1;
                if (task.IsDone) isDone = 1;

                scoreList[(int)task.TaskDefinition.TaskCategory] += task.TaskDefinition.Weight * isDone;
            }
        }

        for (int i = 1; i < m_ScoreGroups.Count; ++i)
        {
            m_ScoreGroups[(int)i].SetRating(scoreList[i]);
            m_ScoreGroups[(int)i].AddToTotal(scoreList[i]);
        }
    }

    public ScoreGroup GetScoreGroup(TaskCategoryType type)
    {
        return m_ScoreGroups[(int)type];
    }

    private void OnGameStart()
    {
        foreach(ScoreGroup scoreGroup in m_ScoreGroups)
        {
            scoreGroup.Reset();
        }
    }

    public bool AlmostWon()
    {
        //all above 1
        for (int i = 0; i < m_ScoreGroups.Count; ++i)
        {
            if (m_ScoreGroups[i].TotalScore < 1)
                return false;
        }

        return true;
    }
}
