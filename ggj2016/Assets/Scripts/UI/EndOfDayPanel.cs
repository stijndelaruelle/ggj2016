using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndOfDayPanel : MonoBehaviour, UIPanel
{
    [SerializeField]
    List<EndOfDayStory> m_StoryPanels;
    private int m_CurrentPanel = 0;

    public void Initialize()
    {
        GameManager.Instance.EndDayEvent += OnDayEnd;

        foreach (EndOfDayStory endOfDayStory in m_StoryPanels)
        {
            endOfDayStory.Initialize();
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.EndDayEvent -= OnDayEnd;
    }

    private void OnEnable()
    {
        ShowPanel(0);
    }

    private void OnDayEnd()
    {
        m_CurrentPanel = 0;
    }

    public void ShowNextPanel()
    {
        ++m_CurrentPanel;
        ShowPanel(m_CurrentPanel);
    }

    private void ShowPanel(int id)
    {
        for (int i = 0; i < m_StoryPanels.Count; ++i)
        {
            bool enabled = (id == i);
            m_StoryPanels[i].gameObject.SetActive(enabled);
        }
    }
}
