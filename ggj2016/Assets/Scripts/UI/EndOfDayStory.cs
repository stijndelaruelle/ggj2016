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
    private string m_GenderWord; //He or she
    private string m_Employer;
    private string m_Collegue;
    private string m_JobName;

    public void Initialize()
    {
        GameManager.Instance.EndDayEvent += OnDayEnd;

        Player player = m_PlayerScore.Player;
        m_Name = player.Name;

        if (player.Gender == GenderType.Male) m_GenderWord = "he";
        else                                  m_GenderWord = "she";

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
        PlayerScore.TimeScore timeScore = m_PlayerScore.CurrentTimeScore;
        PlayerScore.TimeScore lastTimeScore = m_PlayerScore.LastTimeScore;

        string sentence = m_Name;

        if (timeScore == PlayerScore.TimeScore.NeverArrived)
        {
            sentence += " never arrived at " + m_JobName + ", because " + m_GenderWord + " took the schoolbus!";
        }
        else
        {
            sentence += " was";

            if (timeScore == lastTimeScore)
                sentence += " again";

            switch (timeScore)
            {
                case PlayerScore.TimeScore.VeryEarly:
                    sentence += " very early";
                    break;

                case PlayerScore.TimeScore.Early:
                    sentence += " early";
                    break;

                case PlayerScore.TimeScore.OnTime:
                    sentence += " on time";
                    break;

                case PlayerScore.TimeScore.Late:
                    sentence += " late";
                    break;

                case PlayerScore.TimeScore.VeryLate:
                    sentence += " very late";
                    break;

                default:
                    break;
            }

            sentence += " at " + m_JobName + ".";
        }

        if ((m_PlayerScore.CurrentTotalTimeScore >= 2) &&
            (m_PlayerScore.CurrentTotalTimeScore > m_PlayerScore.LastTotalTimeScore))
        {
            sentence += " This is very much appreciated!";
        }

        if (m_PlayerScore.CurrentTotalTimeScore <= -2)
        {
            sentence += " If this happens 1 more time " + m_GenderWord + " will be ";

            if (m_PlayerScore.Player.PlayerType == PlayerType.Parent)
                sentence += "fired!";
            else
                sentence += "kicked from school.";
        }

        return sentence;
    }

    private string CalculateEmployerStory()
    {
        return m_Employer + " story...";
    }

    private string CalculateCollegueStory()
    {
        return m_Collegue + " story...";
    }
}
