using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EpisodeTitle : MonoBehaviour, UIPanel
{
    [SerializeField]
    private Text m_Text;


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
        m_Text.text = "Episode " + GameManager.Instance.Day;
    }
}
