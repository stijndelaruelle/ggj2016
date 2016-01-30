using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_PanelMainMenu;

    [SerializeField]
    private GameObject m_PanelStartDay;

    [SerializeField]
    private GameObject m_PanelEndDay;

    [SerializeField]
    private GameObject m_PanelInGame;

    private void Start()
    {
        GameManager gameManager = GameManager.Instance;

        gameManager.CreateDayEvent += OnCreateDay;
        gameManager.StartDayEvent += OnStartDay;
        gameManager.EndDayEvent += OnEndDay;

        m_PanelEndDay.GetComponent<UIPanel>().Initialize();

        ShowMainMenu();
    }

    private void OnDestroy()
    {
        GameManager gameManager = GameManager.Instance;

        if (gameManager == null)
            return;

        gameManager.CreateDayEvent -= OnCreateDay;
        gameManager.StartDayEvent -= OnStartDay;
        gameManager.EndDayEvent -= OnEndDay;
    }

    private void ShowMainMenu()
    {
        m_PanelMainMenu.SetActive(true);
        m_PanelStartDay.SetActive(false);
        m_PanelEndDay.SetActive(false);
        m_PanelInGame.SetActive(false);
    }

    private void ShowStartDay()
    {
        m_PanelMainMenu.SetActive(false);
        m_PanelStartDay.SetActive(true);
        m_PanelEndDay.SetActive(false);
        m_PanelInGame.SetActive(false);
    }

    private void ShowEndDay()
    {
        m_PanelMainMenu.SetActive(false);
        m_PanelStartDay.SetActive(false);
        m_PanelEndDay.SetActive(true);
        m_PanelInGame.SetActive(false);
    }

    private void ShowInGame()
    {
        m_PanelMainMenu.SetActive(false);
        m_PanelStartDay.SetActive(false);
        m_PanelEndDay.SetActive(false);
        m_PanelInGame.SetActive(true);
    }


    private void OnCreateDay()
    {
        ShowStartDay();
    }

    private void OnStartDay()
    {
        ShowInGame();
    }

    private void OnEndDay()
    {
        ShowEndDay();
    }
}
