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
	private string m_Royalty;
	private string m_Gender;

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
			m_Royalty = "King";
			m_Gender = "man";
		}
        else
        {
            m_HeShe = "she";
            m_HisHer = "Her";
			m_Royalty = "Queen";
			m_Gender = "woman";
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
        m_PlayerScore.Calculate();

        string sentence = CalculateOnTimeStory() + "<br><br>";

        //Boss
        sentence += CalculateProductivityStory() + "<br>";
        sentence += CalculateWardrobeStory() + "<br><br>";

        //Colleagues
        sentence += CalculateSmellStory() + "<br>";
        sentence += CalculateEntertainmentStory() + "<br><br>";

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

    private string CalculateProductivityStory()
    {
        string sentence = "";

        ScoreGroup scoreGroup = m_PlayerScore.GetScoreGroup(TaskCategoryType.Productivity);

        Rating score = scoreGroup.Score;
        Rating lastScore = scoreGroup.LastScore;

        switch (score)
        {
            case Rating.VeryGood: sentence += "A real eager beaver!";				break;
            case Rating.Good: sentence += "Almost broke a sweat today.";			break;
            case Rating.Normal: sentence += "Made no impression at all.";			break;
            case Rating.Bad: sentence += "Studying to be a La-Z-Boy.";				break;
            case Rating.VeryBad: sentence += "Master slacker!";						break;

            default:
                break;
        }

        //if ((scoreGroup.TotalScore >= 2) &&
        //    (scoreGroup.TotalScore > scoreGroup.LastTotalScore))
        //{
        //    sentence += " This is very much appreciated!";
        //}

        return sentence;
    }

    private string CalculateWardrobeStory()
    {
        string sentence = "";
        ScoreGroup scoreGroup = m_PlayerScore.GetScoreGroup(TaskCategoryType.Wardrobe);

        Rating score = scoreGroup.Score;
        Rating lastScore = scoreGroup.LastScore;

        switch (score)
        {
            case Rating.VeryGood: sentence += "Dressed like the " + m_Royalty + ", fabulous!"; break;
            case Rating.Good: sentence += "Clothes make the " + m_Gender + " they say."; break;
            case Rating.Normal: sentence += "Shirt & pants, check."; break;
            case Rating.Bad: sentence += "Even a starved moth wouldn't touch this wardrobe."; break;
            case Rating.VeryBad: sentence += "The coalmines called, they want their clothes back!"; break;

            default:
                break;
        }

        return sentence;
    }

    private string CalculateSmellStory()
    {
        string sentence = "";
        ScoreGroup scoreGroup = m_PlayerScore.GetScoreGroup(TaskCategoryType.Smell);

        Rating score = scoreGroup.Score;
        Rating lastScore = scoreGroup.LastScore;

        switch (score)
        {
            case Rating.VeryGood: sentence += "They sell your sweat as the most exquisite perfume!"; break;
            case Rating.Good: sentence += "A flowery garden scent."; break;
            case Rating.Normal: sentence += "A most ordinary scent."; break;
            case Rating.Bad: sentence += "Your stench preceeds you."; break;
            case Rating.VeryBad: sentence += "What's that smell? Dungbell!"; break;

            default:
                break;
        }

        return sentence;
    }

    private string CalculateEntertainmentStory()
    {
        string sentence = "";
        ScoreGroup scoreGroup = m_PlayerScore.GetScoreGroup(TaskCategoryType.Entertainment);

        Rating score = scoreGroup.Score;
        Rating lastScore = scoreGroup.LastScore;

        switch (score)
        {
            case Rating.VeryGood: sentence += "Excited! Excited! Excited!"; break;
            case Rating.Good: sentence += "No day like a fun day."; break;
            case Rating.Normal: sentence += "Not bored, not amused, meh."; break;
            case Rating.Bad: sentence += "Such a dull " + m_Gender + "."; break;
            case Rating.VeryBad: sentence += "All work and no play makes me go *@#*€%#!"; break;

            default:
                break;
        }

        return sentence;
    }
}
