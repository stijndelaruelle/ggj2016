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
	private string m_HeSheCap;
	private string m_HisHer;
	private string m_HimHer;
	private string m_Royalty;
	private string m_Gender;
	private string m_GenderDesignation;

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
			m_HeSheCap = "He";
			m_HisHer = "his";
			m_HimHer = "him";
			m_Royalty = "King";
			m_Gender = "man";
			m_GenderDesignation = "Boy";
		}
        else
        {
            m_HeShe = "she";
			m_HeSheCap = "She";
			m_HisHer = "her";
			m_HimHer = "her";
			m_Royalty = "Queen";
			m_Gender = "woman";
			m_GenderDesignation = "Girl";
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
            sentence += m_Name + " " + m_HisHer + " " + m_Collegue + " know they can count on " + m_HisHer +" schedule!";
        }

        if (scoreGroup.TotalScore <= -2)
        {
            sentence += m_Name + " still has one chance left or " + m_HeShe + " will be ";

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
            case Rating.VeryGood: sentence += m_Name + " is a real eager beaver!"; break;
            case Rating.Good: sentence += m_Name + " almost broke a sweat today."; break;
            case Rating.Normal: sentence += m_Name + " made no impression at all.";	break;
            case Rating.Bad: sentence += m_Name + " is studying to be a La-Z-" + m_GenderDesignation + ".";	break;
            case Rating.VeryBad: sentence += m_Name + " is a master slacker!"; break;

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
            case Rating.VeryGood: sentence += m_Employer + " thinks " + m_Name + " is dressed like the " + m_Royalty + ", fabulous!"; break;
            case Rating.Good: sentence += "Clothes make the " + m_Gender + " they say."; break;
            case Rating.Normal: sentence += "Shirt & pants, check."; break;
            case Rating.Bad: sentence += m_Employer + " thinks that even a starved moth wouldn't touch " + m_Name + " " + m_HisHer + " wardrobe."; break;
            case Rating.VeryBad: sentence += m_Employer +  " says that the coalmines called, they want their clothes back!"; break;

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
            case Rating.VeryGood:
				sentence += m_Name + " smells so great that " + m_HisHer + " " + m_Collegue + "want to sell " + m_HisHer + " sweat as a perfume!"; break;
            case Rating.Good:
				sentence += "'" + m_HeSheCap + " smells like a flowery garden. - One of " + m_Name + "'s " + m_Collegue + "."; break;
            case Rating.Normal: sentence += m_Name + " has a most ordinary scent."; break;
            case Rating.Bad:
				sentence += m_Name + " " + m_HisHer + " stench preceeds" + m_HimHer + "."; break;
            case Rating.VeryBad: sentence += "'What's that smell? Dungbell!' " + m_Name + " has a new nickname."; break;

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
            case Rating.Normal: sentence += m_Name + " is feeling neither bored, nor amused, meh."; break;
            case Rating.Bad: sentence += "Such a dull " + m_Gender + "."; break;
            case Rating.VeryBad: sentence += "All work and no play makes " + m_Name + " go *@#*€%#!"; break;

            default:
                break;
        }

        return sentence;
    }
}
