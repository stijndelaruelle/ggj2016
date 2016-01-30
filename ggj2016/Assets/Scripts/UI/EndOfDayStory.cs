using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndOfDayStory : MonoBehaviour
{
    [SerializeField]
    private PlayerScore m_PlayerScore;

    [SerializeField]
    private Text m_Text;
    private string m_Name;
    private string m_HeShe;
    private string m_HisHer;
    private string m_Employer;
    private string m_Collegue;
    private string m_JobName;

    public void Initialize()
    {
        GameManager.Instance.EndDayEvent += OnDayEnd;

        Player player = m_PlayerScore.Player;
        m_Name = player.Name;

        if (player.Gender == GenderType.Male)
        {
            m_HeShe = "he";
            m_HisHer = "His";
        }
        else
        {
            m_HeShe = "she";
            m_HisHer = "Her";
        }

        if (player.PlayerType == PlayerType.Parent)
        {
            m_JobName = "work";
            m_Employer = "boss";
            m_Collegue = "collegues";
        }
        else
        {
            m_JobName = "school";
            m_Employer = "teacher";
            m_Collegue = "classmates";
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
        string sentence = CalculateOnTimeStory() + "<br><br>";
        sentence += CalculateEmployerStory() + "<br><br>";
        sentence += CalculateCollegueStory();

        sentence = sentence.Replace("<br>", "\n");
        m_Text.text = sentence;
    }

    private string CalculateOnTimeStory()
    {
        ScoreGroup scoreGroup = m_PlayerScore.GetScoreGroup(TaskCategoryType.Time);

        Rating timeScore = scoreGroup.Score;
        Rating lastTimeScore = scoreGroup.LastScore;

        string sentence = m_Name;

        if (timeScore == Rating.Terrible)
        {
            sentence += " never arrived at " + m_JobName + ", because " + m_HeShe + " took the schoolbus!";
        }
        else
        {
            sentence += " was";

            if (timeScore == lastTimeScore)
                sentence += " again";

            switch (timeScore)
            {
                case Rating.VeryGood:
                    sentence += " very early at " + m_JobName + ".";
                    break;

                case Rating.Good:
                    sentence += " early at " + m_JobName + ".";
                    break;

                case Rating.Normal:
                    sentence += " at " + m_JobName + " on time";
                    break;

                case Rating.Bad:
                    sentence += " late at " + m_JobName + "."; ;
                    break;

                case Rating.VeryBad:
                    sentence += " very late at " + m_JobName + ".";
                    break;

                default:
                    break;
            }
        }

        if ((scoreGroup.TotalScore >= 2) &&
            (scoreGroup.TotalScore > scoreGroup.LastTotalScore))
        {
            sentence += " This is very much appreciated!";
        }

        if (scoreGroup.TotalScore <= -2)
        {
            sentence += " If this happens 1 more time " + m_HeShe + " will be ";

            if (m_PlayerScore.Player.PlayerType == PlayerType.Parent)
                sentence += "fired!";
            else
                sentence += "kicked from school.";
        }

        return sentence;
    }

    private string CalculateEmployerStory()
    {
        ScoreGroup scoreGroup = m_PlayerScore.GetScoreGroup(TaskCategoryType.Productivity);

        Rating timeScore = scoreGroup.Score;
        Rating lastTimeScore = scoreGroup.LastScore;

        string sentence = "";

        switch (timeScore)
        {
            case Rating.VeryGood: sentence += "Very good productivity."; break;
            case Rating.Good: sentence += "Good productivity.";          break;
            case Rating.Normal: sentence += "Normal productivity.";      break;
            case Rating.Bad: sentence += "Bad productivity.";            break;
            case Rating.VeryBad: sentence += "Aweful productivity.";    break;

            default:
                break;
        }

        if ((scoreGroup.TotalScore >= 2) &&
            (scoreGroup.TotalScore > scoreGroup.LastTotalScore))
        {
            sentence += " This is very much appreciated!";
        }

        return sentence;
    }

    private string CalculateCollegueStory()
    {
        return m_Collegue + " story...";
    }
}
