using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EpisodeTitle : MonoBehaviour
{
    [SerializeField]
    private Text m_Text;


    private void Start()
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
