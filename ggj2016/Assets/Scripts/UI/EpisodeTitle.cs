using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class EpisodeTitle : MonoBehaviour, UIPanel
{
    [SerializeField]
    private Text m_Text;

    [SerializeField]
    private List<string> m_EpisodeTitles;

    public void Initialize()
    {
        GameManager.Instance.CreateDayEvent += OnCreateDay;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.CreateDayEvent -= OnCreateDay;
    }

    private void OnCreateDay()
    {
        int rand = Random.Range(0, m_EpisodeTitles.Count - 1);
        m_Text.text = "Episode " + GameManager.Instance.Day + ": " + m_EpisodeTitles[rand];
    }
}
