using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartOfDayPanel : MonoBehaviour, UIPanel
{
    [SerializeField]
    private List<GameObject> m_DayPanels;

    [SerializeField]
    private List<EpisodeTitle> m_EpisodeTitles;

    public void Initialize()
    {
        GameManager.Instance.CreateDayEvent += OnCreateDay;

        foreach (EpisodeTitle episodeTitle in m_EpisodeTitles)
        {
            episodeTitle.Initialize();
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.EndDayEvent -= OnCreateDay;
    }

    private void OnCreateDay()
    {
        int day = GameManager.Instance.Day;

        int id = day - 1;
        if (id >= m_DayPanels.Count)
            id = m_DayPanels.Count - 1;

        for (int i = 0; i < m_DayPanels.Count; ++i)
        {
            m_DayPanels[i].SetActive((i == id));
        }
    }


}
