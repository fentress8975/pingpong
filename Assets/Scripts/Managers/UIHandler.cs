using GameSystems.Scene;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIHandler : SingletonMonoPersistent<UIHandler>
{
    public UnityEvent<bool> OnControlsRebinding;
    private Action OnSwitchSceneToMenu;

    [SerializeField]
    private GameObject m_PrefabMenuPanel;
    [SerializeField]
    private GameObject m_PrefabGameUI;

    private GameObject m_GameUI;
    private GameObject m_MenuPanel;
    private PauseMenu m_GamePausePanel;
    private MainMenu m_MainMenu;
    private PlayersScoreHandler m_ScorePanel;
    private PlayersScoreHandler m_ScoreHandler;

    private List<GameObject> m_ListActiveUi = new();

    private StateUI m_StateUI = StateUI.MainMenu;

    private void Start()
    {
        OnSwitchSceneToMenu = BuildUI;
    }

    public void Init(GameState stateUI)
    {
        switch (stateUI)
        {
            case GameState.MainMenu:
                BuildMainMenu();
                break;
            case GameState.Active:
                BuildGameMenu();
                break;
        }
        
    }

    private void StartGame(MultiplayerMode mpMode, string levelName)
    {
        Debug.Log("Ui want Start Game");
        GameManager.Instance.ChangeGameLevel(mpMode, levelName);
    }

    private void BuildUI()
    {
        ClearUI();
        switch (m_StateUI)
        {
            case StateUI.MainMenu:
                BuildMainMenu();
                break;
            case StateUI.Game:
                BuildGameMenu();
                break;
        }
    }

    public void UpdateUIState(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                CheckState(state);
                break;
            case GameState.Active:
                CheckState(state);
                SetActivePauseMenu(false);
                break;
            case GameState.Pause:
                CheckState(state);
                SetActivePauseMenu(true);
                break;
        }
    }

    private void CheckState(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                if (m_StateUI == StateUI.Game)
                {
                    m_StateUI = StateUI.MainMenu;
                    BuildUI();
                }
                break;
            case GameState.Active:
                if (m_StateUI == StateUI.MainMenu)
                {
                    m_StateUI = StateUI.Game;
                    BuildUI();
                }
                break;
            case GameState.Pause:
                if (m_StateUI == StateUI.MainMenu)
                {
                    m_StateUI = StateUI.Game;
                    BuildUI();
                }
                break;
        }
    }

    private void ClearUI()
    {
        foreach (var item in m_ListActiveUi)
        {
            Destroy(item);
        }
        m_ListActiveUi.Clear();
    }

    private void BuildMainMenu()
    {
        Debug.Log("UI Build MainMenu");
        m_MainMenu = FindObjectOfType<MainMenu>();

        if (m_MainMenu == null)
        {
            m_MenuPanel = Instantiate(m_PrefabMenuPanel);
            m_MainMenu = m_MenuPanel.GetComponent<MainMenu>();
            m_ListActiveUi.Add(m_MenuPanel);
        }

        m_MainMenu.StartGameEvent.AddListener(StartGame);
    }

    private void BuildGameMenu()
    {
        m_GameUI = Instantiate(m_PrefabGameUI);
        m_GamePausePanel = m_GameUI.GetComponentInChildren<PauseMenu>();
        m_GamePausePanel.SwitchToMenu.AddListener(OpenMainMenu);

        m_ScorePanel = m_GameUI.GetComponentInChildren<PlayersScoreHandler>();
        m_ScorePanel.gameObject.SetActive(true);
        m_ScoreHandler = m_ScorePanel.GetComponent<PlayersScoreHandler>();

        m_ListActiveUi.Add(m_GameUI);
    }

    private void OpenMainMenu()
    {
        Debug.Log("Trying Switch to main menu");
        m_StateUI = StateUI.MainMenu;
        StartCoroutine(SceneChanger.Instance.ChangeScene(SceneChanger.SceneMainMenu,OnSwitchSceneToMenu));
    }

    public void SetActivePauseMenu(bool active)
    {
        switch (m_StateUI)
        {
            case StateUI.Game:
                if (active)
                {
                    m_GamePausePanel.OpenPauseMenu();
                }
                else
                {
                    m_GamePausePanel.ClosePauseMenu();
                }
                
                break;
            case StateUI.MainMenu:
                throw new NotImplementedException($"Not Allowed Open Game Pause Menu in {m_StateUI} state!");
                break;
        }
    }

    public void UpdateScore(int player1Score, int player2Score)
    {
        if (m_StateUI == StateUI.MainMenu)
        {
            throw new NotImplementedException($"Not Allowed to change Players Score in {m_StateUI} state!");
        }
        m_ScoreHandler.UpdatePlayer1Score(player1Score);
        m_ScoreHandler.UpdatePlayer2Score(player2Score);
    }

    public enum StateUI
    {
        MainMenu,
        Game
    }
}
