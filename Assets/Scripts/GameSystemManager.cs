using GameSystems.Scene;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemManager : SingletonMonoPersistent<GameSystemManager>
{
    private GameManager m_GameManager;
    private UIHandler m_UIHandler;
    private SceneChanger m_SceneChanger;
    private GameState m_GameState;

    public GameState gameState
    {
        get => m_GameState;
        private set
        {
            m_GameState = value;
        }
    }

    void Start()
    {
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
        switch (state)
        {
            case GameState.MainMenu:
                StartMenuScene();
                break;
            case GameState.Active:
                StartGameScene();
                break;
            case GameState.Pause:
                SetGamePause();
                break;
        }
        m_GameState = state;
        UpdateSystems();
    }

    private void SetGamePause()
    {
        throw new NotImplementedException();
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
