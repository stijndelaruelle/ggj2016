using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
        else if(value > -4) rating = Rating.VeryBad;

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
}

public class PlayerScore : MonoBehaviour
{
    [SerializeField]
    private Player m_Player;
    public Player Player
    {
        get { return m_Player; }
    }

    //Timescore
    List<ScoreGroup> m_ScoreGroups;

    private void Start()
    {
        GameManager.Instance.EndDayEvent += OnDayEnd;

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

        GameManager.Instance.EndDayEvent -= OnDayEnd;
    }

    private void OnDayEnd()
    {
        CalculateOnTimeScore();
        CalculateTaskScore();

        if (m_ScoreGroups[(int)TaskCategoryType.Time].TotalScore < -3)
        {
            //Ontslagen of van school getrapt
        }

        if ((m_ScoreGroups[(int)TaskCategoryType.Productivity].TotalScore) < -3)
        {
            //Boss score
        }
    }

    private void CalculateOnTimeScore()
    {
        int timeLeft = m_Player.TimeScreenLeft;
        ScoreGroup scoreGroup = m_ScoreGroups[(int)TaskCategoryType.Time];

        //if we're a parent, and we took the schoolbus, we never arrive!
        if (m_Player.PlayerType == PlayerType.Parent)
        {

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
        if (timeLeft > -10)
        {
            scoreGroup.SetRating(Rating.VeryBad);
            scoreGroup.AddToTotal(-2);
            return;
        }
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
            int isDone = -1;
            if (task.IsDone) isDone = 1;

            scoreList[(int)task.TaskDefinition.TaskCategory] += task.TaskDefinition.Weight * isDone;
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
}
