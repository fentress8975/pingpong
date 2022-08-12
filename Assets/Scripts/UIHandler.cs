using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : SingletonMonoPersistent<UIHandler>
{
    [SerializeField]
    private GameObject m_PrefabMenuPanel;
    [SerializeField]
    private GameObject m_PrefabGamePausePanel;
    [SerializeField]
    private GameObject m_PrefabScorePanel;

    private GameObject m_MenuPanel;
    private GameObject m_GamePausePanel;
    private GameObject m_ScorePanel;
    private PlayersScoreHandler m_ScoreHandler;

    private List<GameObject> m_ListActiveUi = new();

    private StateUI m_StateUI = StateUI.MainMenu;


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
        GameManager.Instance.InitGame(mpMode, levelName);
    }

    private void BuildUI()
    {
        ClearUI();
        switch (m_StateUI)
        {
            case StateUI.MainMenu:
                BuildGameMenu();
                break;
            case StateUI.Game:
                BuildMainMenu();
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
        m_MenuPanel = FindObjectOfType<MainMenu>().gameObject;

        if (m_MenuPanel == null)
        {
            m_MenuPanel = Instantiate(m_MenuPanel);
            m_ListActiveUi.Add(m_MenuPanel);
        }

        m_MenuPanel.GetComponent<MainMenu>().StartGameEvent.AddListener(StartGame);
    }

    private void BuildGameMenu()
    {
        m_GamePausePanel = FindObjectOfType<PauseMenu>().gameObject;
        m_ScorePanel = FindObjectOfType<PlayersScoreHandler>().gameObject;

        if(m_GamePausePanel == null)
        {
            m_GamePausePanel = Instantiate(m_MenuPanel);
            m_GamePausePanel.SetActive(false);
        }
        if (m_ScorePanel == null)
        {
            m_ScorePanel = Instantiate(m_PrefabScorePanel);
            m_ScorePanel.SetActive(true);
            m_ScoreHandler = m_ScorePanel.GetComponent<PlayersScoreHandler>();
        }
        m_ListActiveUi.Add(m_GamePausePanel);
        m_ListActiveUi.Add(m_ScorePanel);
    }

    public void SetActivePauseMenu(bool active)
    {
        switch (m_StateUI)
        {
            case StateUI.MainMenu:
                m_GamePausePanel.SetActive(active);
                break;
            case StateUI.Game:
                throw new NotImplementedException($"Not Allowed Open Pause Menu in {m_StateUI} state!");
                break;
        }
    }

    public void UpdateScore(int player1Score, int player2Score)
    {
        if(m_StateUI == StateUI.MainMenu)
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
