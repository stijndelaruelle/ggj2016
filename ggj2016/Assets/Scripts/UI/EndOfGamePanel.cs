using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndOfGamePanel : MonoBehaviour, UIPanel
{
    [SerializeField]
    private Text m_Text;

    public void Initialize()
    {
        GameManager.Instance.EndGameEvent += OnGameEnd;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.EndGameEvent -= OnGameEnd;
    }

    private void OnGameEnd(Player player, TaskCategoryType reason, bool victory)
    {
        string sentence = player.Name;
        string heShe = "";
        string hisHer = "";

        if (player.Gender == GenderType.Male)
        {
            heShe = "he";
            hisHer = "His";
        }
        else
        {
            heShe = "she";
            hisHer = "Her";
        }

        if (victory)
        {
            if (player.PlayerType == PlayerType.Parent)
            {
                sentence += " got a promotion! Your family has earned the respect of the neighbourhood.";
            }
            else
            {
                sentence += " got into the chess club! Your family has earned the respect of the neighbourhood.";
            }
        }
        else
        {
            if (player.PlayerType == PlayerType.Parent)
            {
                sentence += " got fired because ";

                switch (reason)
                {
                    case TaskCategoryType.Time:
                        sentence += heShe + " was always too late!";
                        break;

                    case TaskCategoryType.Productivity:
                        sentence += heShe + " didn't process 4000 documents a day!";
                        break;

                    case TaskCategoryType.Wardrobe:
                        sentence += heShe + " didn't learn how to dress!";
                        break;

                    case TaskCategoryType.Smell:
                        sentence += heShe + " smelled quite unfunny!";
                        break;

                    case TaskCategoryType.Entertainment:
                        sentence += heShe + " wasn't up to date!";
                        break;

                    default:
                        break;
                }
            }
            else
            {
                sentence += " got kicked out of school because ";

                switch (reason)
                {
                    case TaskCategoryType.Time:
                        sentence += heShe + " was always too late!";
                        break;

                    case TaskCategoryType.Productivity:
                        sentence += heShe + " wasn't able to learn the names of every planet in the galaxy!";
                        break;

                    case TaskCategoryType.Wardrobe:
                        sentence += heShe + " didn't learn how to dress!";
                        break;

                    case TaskCategoryType.Smell:
                        sentence += heShe + " smelled quite unfunny!";
                        break;

                    case TaskCategoryType.Entertainment:
                        sentence += heShe + " din't know the names of all 151 pokémon!";
                        break;

                    default:
                        break;
                } 
            }

            sentence += "<br>Your family has once again proven...";
        }

        sentence = sentence.Replace("<br>", "\n");
        m_Text.text = sentence;
    }
}
