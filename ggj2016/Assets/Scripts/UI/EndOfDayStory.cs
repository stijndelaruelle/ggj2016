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
    private string m_HisHerCap;
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
            m_HisHerCap = "His";
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
            m_HisHerCap = "Her";
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

        //Compliment
        if (m_PlayerScore.AlmostWon())
        {
            sentence += "Keep it up and " + m_HeShe + " might get ";

            if (m_PlayerScore.Player.PlayerType == PlayerType.Parent)
            {
                sentence += "promoted!";
            }
            else
            {
                sentence += "invited into join the school's elite chess club!";
            }

        }


        sentence = sentence.Replace("<br>", "\n");
        m_Text.text = sentence;
    }

    private string CalculateOnTimeStory()
    {
        ScoreGroup scoreGroup = m_PlayerScore.GetScoreGroup(TaskCategoryType.Time);

        Rating timeScore = scoreGroup.Score;
        Rating lastTimeScore = scoreGroup.LastScore;

        string sentence = "Today " + m_Name;

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

        if (scoreGroup.TotalScore <= -2)
        {
            sentence += " " + m_HeSheCap + " has one chance left to be on time or " + m_HeShe + " will be ";

            if (m_PlayerScore.Player.PlayerType == PlayerType.Parent)
                sentence += "fired!";
            else
                sentence += "kicked from school!";
        }

        return sentence;
    }

    private string CalculateProductivityStory()
    {
        string sentence = "";

        ScoreGroup scoreGroup = m_PlayerScore.GetScoreGroup(TaskCategoryType.Productivity);

        Rating score = scoreGroup.Score;
        Rating lastScore = scoreGroup.LastScore;

        sentence += m_HisHerCap + " " + m_Employer + " thought " + m_HeShe;

        switch (score)
        {
            case Rating.VeryGood: sentence += " was a real eager beaver!"; break;
            case Rating.Good: sentence += " did a fair amount of work."; break;
            case Rating.Normal: sentence += " made no impression at all.";	break;
            case Rating.Bad: sentence += " was studying to be a La-Z-" + m_GenderDesignation + ".";	break;
            case Rating.VeryBad: sentence += " was a master slacker!"; break;

            default:
                break;
        }

        if (scoreGroup.TotalScore <= -2)
        {
            sentence += " " + m_HeSheCap + " has one chance left to be more productive or " + m_HeShe + " will be ";

            if (m_PlayerScore.Player.PlayerType == PlayerType.Parent)
                sentence += "fired!";
            else
                sentence += "kicked out of school!";
        }

        return sentence;
    }

    private string CalculateWardrobeStory()
    {
        string sentence = "";
        ScoreGroup scoreGroup = m_PlayerScore.GetScoreGroup(TaskCategoryType.Wardrobe);

        Rating score = scoreGroup.Score;
        Rating lastScore = scoreGroup.LastScore;

        sentence += "On top of that he ";

        switch (score)
        {
            case Rating.VeryGood: sentence += "thought that " + m_Name + " was dressed like the " + m_Royalty + ", fabulous!"; break;
            case Rating.Good: sentence += "thought that " + m_Name + " was dressed like a real " + m_Gender + "."; break;
            case Rating.Normal: sentence += "told " + m_Name + " should swap shirts every once in a while."; break;
            case Rating.Bad: sentence += "told " + m_Name + " that even a starved moth wouldn't touch " + m_HisHer + " wardrobe."; break;
            case Rating.VeryBad: sentence += "told " + m_Name + " that the coalmines called and they want their clothes back!"; break;

            default:
                break;
        }

        if (scoreGroup.TotalScore <= -2)
        {
            sentence += " " + m_HeSheCap + " has one chance left to <b>dress propertly</b> or " + m_HeShe + " will be ";

            if (m_PlayerScore.Player.PlayerType == PlayerType.Parent)
                sentence += "fired!";
            else
                sentence += "kicked out of school!";
        }

        return sentence;
    }

    private string CalculateSmellStory()
    {
        string sentence = "";
        ScoreGroup scoreGroup = m_PlayerScore.GetScoreGroup(TaskCategoryType.Smell);

        Rating score = scoreGroup.Score;
        Rating lastScore = scoreGroup.LastScore;

        sentence += m_HisHerCap + " " + m_Collegue + " on the other hand ";

        switch (score)
        {
            case Rating.VeryGood:
				sentence += "said that " + m_HeShe + " smelled so great that they want to sell " + m_HisHer + " sweat as a perfume!"; break;
            case Rating.Good:
				sentence += "said " + m_HeShe + " smelled like a flowery garden."; break;
            case Rating.Normal:
                sentence += "told " + m_HimHer + " " + m_HeShe + " has a most ordinary scent."; break;
            case Rating.Bad:
                sentence += "gave " + m_HisHer + " the catchphrase: 'What's that smell? Dungbell!'"; break;
            case Rating.VeryBad:
                sentence += "knew when " + m_HeShe + " was around when when they heard the sound of a billion flies."; break;
            default:
                break;
        }

        if (scoreGroup.TotalScore <= -2)
        {
            sentence += " " + m_HeSheCap + " has one chance left to stop smelling or " + m_HeShe + " will be ";

            if (m_PlayerScore.Player.PlayerType == PlayerType.Parent)
                sentence += "fired!";
            else
                sentence += "kicked out of school!";
        }

        return sentence;
    }

    private string CalculateEntertainmentStory()
    {
        string sentence = "";
        ScoreGroup scoreGroup = m_PlayerScore.GetScoreGroup(TaskCategoryType.Entertainment);

        Rating score = scoreGroup.Score;
        Rating lastScore = scoreGroup.LastScore;

        sentence += "They also ";

        switch (score)
        {
            case Rating.VeryGood:
                {
                    sentence += "were very excited to hear about the lastest ";

                    if (m_PlayerScore.Player.PlayerType == PlayerType.Parent)
                    {
                        if (m_PlayerScore.Player.Gender == GenderType.Male)
                        {
                            sentence += "soccer news!";
                        }
                        else
                        {
                            sentence += "drama in hollywood!";
                        }
                    }
                    else
                    {
                        sentence += "episode of SUPER DINOBOTS!";
                    }
                    break;
                }
                
            case Rating.Good:
                sentence += "liked the joke " + m_HeShe + " told during lunch"; break;
            case Rating.Normal:
                sentence += "thought " + m_HeShe + " was very quiet today."; break;
            case Rating.Bad:
                sentence += "found " + m_HimHer + " quite boring today."; break;
            case Rating.VeryBad:
                sentence += "thought " + m_HeShe + " has been living under a rock for years!"; break;

            default:
                break;
        }

        if (scoreGroup.TotalScore <= -2)
        {
            sentence += " " + m_HeSheCap + " has one chance left to know about the latest entertainment or " + m_HeShe + " will be ";

            if (m_PlayerScore.Player.PlayerType == PlayerType.Parent)
                sentence += "fired!";
            else
                sentence += "kicked out of school!";
        }

        return sentence;
    }
}
