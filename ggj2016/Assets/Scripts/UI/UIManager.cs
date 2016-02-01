using UnityEngine;
using System.Collections;
using Sjabloon;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_PanelMainMenu;

    [SerializeField]
    private GameObject m_PanelEndGame;

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
        gameManager.EndGameEvent += OnGameEnd;

        //Lame
        m_PanelStartDay.GetComponent<UIPanel>().Initialize();
        m_PanelEndDay.GetComponent<UIPanel>().Initialize();
        m_PanelEndGame.GetComponent<UIPanel>().Initialize();

        InitializeControls();
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
        gameManager.EndGameEvent -= OnGameEnd;
    }

    private void InitializeControls()
    {
        //Quit game
        for (int i = 0; i < 4; ++i)
        {
            InputManager.Instance.BindButton("Quit_" + i, KeyCode.Escape, InputManager.ButtonState.OnPress);
            InputManager.Instance.BindButton("Quit_" + i, i, ControllerButtonCode.Start, InputManager.ButtonState.OnPress);
        }
    }

    private void Update()
    {
        for (int i = 0; i < 4; ++i)
        {
            if (InputManager.Instance.GetButton("Quit_" + i))
            {
                Application.Quit();
            }
        }
    }

    public void ShowMainMenu()
    {
        m_PanelMainMenu.SetActive(true);
        m_PanelEndGame.SetActive(false);
        m_PanelStartDay.SetActive(false);
        m_PanelEndDay.SetActive(false);
        m_PanelInGame.SetActive(false);
    }

    private void ShowEndMenu()
    {
        m_PanelMainMenu.SetActive(false);
        m_PanelEndGame.SetActive(true);
        m_PanelStartDay.SetActive(false);
        m_PanelEndDay.SetActive(false);
        m_PanelInGame.SetActive(false);
    }

    private void ShowStartDay()
    {
        m_PanelMainMenu.SetActive(false);
        m_PanelEndGame.SetActive(false);
        m_PanelStartDay.SetActive(true);
        m_PanelEndDay.SetActive(false);
        m_PanelInGame.SetActive(false);
    }

    private void ShowEndDay()
    {
        m_PanelMainMenu.SetActive(false);
        m_PanelEndGame.SetActive(false);
        m_PanelStartDay.SetActive(false);
        m_PanelEndDay.SetActive(true);
        m_PanelInGame.SetActive(false);
    }

    private void ShowInGame()
    {
        m_PanelMainMenu.SetActive(false);
        m_PanelEndGame.SetActive(false);
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

    private void OnGameEnd(Player player, TaskCategoryType reason, bool victory)
    {
        ShowEndMenu();
    }
}
