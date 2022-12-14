using GameSystems.Scene;
using System;
using UnityEngine;

public class GameSystemManager : SingletonMonoPersistent<GameSystemManager>
{
    [SerializeField]
    private int m_TargetFrameRate = 240;
    private GameManager m_GameManager;
    private UIHandler m_UIHandler;
    private SceneChanger m_SceneChanger;
    [SerializeField]
    private GameState m_GameState;
    public GameState gameState
    {
        get => m_GameState;
        private set
        {
            m_GameState = value;
        }
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = m_TargetFrameRate;

        m_GameManager = GetComponentInChildren<GameManager>();
        m_UIHandler = GetComponentInChildren<UIHandler>();
        m_SceneChanger = SceneChanger.Instance;
        m_GameState = GameState.MainMenu;
        InitSystems();
        UpdateSystems();
    }

    private void InitSystems()
    {
        m_UIHandler.Init(m_GameState);
    }

    private void UpdateSystems()
    {
        m_UIHandler.UpdateUIState(gameState);
    }

    public void SwitchGameState(GameState state)
    {
        m_GameState = state;
        UpdateSystems();

        switch (m_GameState)
        {
            case GameState.MainMenu:
                SetGamePause(false);
                break;
            case GameState.Active:
                SetGamePause(false);
                break;
            case GameState.Pause:
                SetGamePause(true);
                break;
        }
    }

    private void SetGamePause(bool pause)
    {
        if (pause)
        {
            GameTimeControl.Pause();
        }
        else
        {
            GameTimeControl.UnPause();
        }

    }

    private void StartGameScene()
    {
        throw new NotImplementedException();
    }

    private void StartMenuScene()
    {
        throw new NotImplementedException();
    }
}
public enum GameState
{
    MainMenu,
    Active,
    Pause
}
