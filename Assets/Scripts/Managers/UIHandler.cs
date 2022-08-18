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

    private GameObject m_GameUICanvas;
    private GameObject m_MenuCanvas;
    private PauseMenu m_GamePausePanel;
    private MainMenu m_MainMenu;
    private PlayersScoreHandler m_ScoreHandler;

    private string m_Player1Name;
    private string m_Player2Name;

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

        m_MenuCanvas = Instantiate(m_PrefabMenuPanel);
        m_MainMenu = m_MenuCanvas.GetComponentInChildren<MainMenu>();
        m_ListActiveUi.Add(m_MenuCanvas);


        m_MainMenu.OnStartGameEvent.AddListener(StartGame);
        m_MainMenu.OnUpdatePlayer1Name.AddListener(SetPlayer1Name);
        m_MainMenu.OnUpdatePlayer2Name.AddListener(SetPlayer2Name);
        m_Player1Name = m_MainMenu.Player1Name;
        m_Player2Name = m_MainMenu.Player2Name;
    }

    private void BuildGameMenu()
    {
        m_GameUICanvas = Instantiate(m_PrefabGameUI);
        m_GamePausePanel = m_GameUICanvas.GetComponentInChildren<PauseMenu>();
        m_GamePausePanel.SwitchToMenu.AddListener(OpenMainMenu);

        m_ScoreHandler = m_GameUICanvas.GetComponentInChildren<PlayersScoreHandler>();
        m_ScoreHandler.gameObject.SetActive(true);
        m_ScoreHandler.Init(m_Player1Name, m_Player2Name);

        m_ListActiveUi.Add(m_GameUICanvas);
    }

    private void OpenMainMenu()
    {
        Debug.Log("Trying Switch to main menu");
        m_StateUI = StateUI.MainMenu;
        StartCoroutine(SceneChanger.Instance.ChangeScene(SceneChanger.SceneMainMenu, OnSwitchSceneToMenu));
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

    private void SetPlayer1Name(string name)
    {
        m_Player1Name = name;
    }

    private void SetPlayer2Name(string name)
    {
        m_Player2Name = name;
    }

    public enum StateUI
    {
        MainMenu,
        Game
    }
}
