using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class EndOfGamePanel : MonoBehaviour, UIPanel
{
    [SerializeField]
    private Text m_Text;

    [SerializeField]
    private List<RectTransform> m_Characters;

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
                sentence += " got a promotion!";
            }
            else
            {
                sentence += " got into the elite chess club!";
            }

            sentence += "<br><br>Your family has finally earned the respect of the neighbourhood.";
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
                        sentence += heShe + " didn't process the required 4000 documents a day!";
                        break;

                    case TaskCategoryType.Wardrobe:
                        sentence += heShe + " didn't learn how to dress!";
                        break;

                    case TaskCategoryType.Smell:
                        sentence += heShe + " smelled quite unfunny!";
                        break;

                    case TaskCategoryType.Entertainment:
                        sentence += heShe + " wasn't very entertaining!";
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
                        sentence += heShe + " was very boring!";
                        break;

                    default:
                        break;
                } 
            }

            sentence += "<br><br>Your family has once again shown what they are capable of...";
        }

        sentence = sentence.Replace("<br>", "\n");
        m_Text.text = sentence;

        ScaleCharacter(player.PlayerID);
    }

    private void ScaleCharacter(int id)
    {
        for (int i = 0; i < m_Characters.Count; ++i)
        {
            m_Characters[i].localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }

        m_Characters[id].localScale = new Vector3(2.25f, 2.25f, 2.25f);
    }
}
