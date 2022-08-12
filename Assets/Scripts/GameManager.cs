using GameSystems.Scene;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : SingletonMonoPersistent<GameManager>
{
    public UnityEvent<int, int> ScoreUpdate;

    private Ball m_Ball;
    private PlayerNet m_GoalP1;
    private PlayerNet m_GoalP2;

    private int m_scoreP1;
    private int m_scoreP2;

    private Rigidbody m_Player1;
    private Rigidbody m_Player2;

    private UIHandler m_UIHandler;

    private MultiplayerMode m_MultiplayerMode;

    public void Init(UIHandler ui)
    {
        m_UIHandler = ui;
    }

    public void InitGame(MultiplayerMode mpMode, string level)
    {
        m_MultiplayerMode = mpMode;
        LoadGameLevel(level);
        switch (m_MultiplayerMode)
        {
            case MultiplayerMode.HOTSEAT:
                SetupHotSeat();
                break;
            case MultiplayerMode.Online:
                break;
            default:
                break;
        }
    }

    private void LoadGameLevel(string level)
    {
        SceneChanger.Instance.ChangeScene(level);
    }

    private void PauseGame()
    {
        if (GameSystemManager.Instance.gameState == GameState.Active)
        {
            GameSystemManager.Instance.SwitchGameState(GameState.Pause);
            GameTimeControl.Pause();
        }
        else if (GameSystemManager.Instance.gameState == GameState.Pause)
        {
            GameSystemManager.Instance.SwitchGameState(GameState.Active);
            GameTimeControl.UnPause();
        }
    }

    private void SetupHotSeat()
    {
        
        m_GoalP1.GoalEvent += ScoreGoal;
        m_GoalP2.GoalEvent += ScoreGoal;
        UpdateScore();
        GameSystemManager.Instance.SwitchGameState(GameState.Active);
    }

    private void ScoreGoal(BorderTeam team)
    {
        m_Ball.Restart(Vector3.zero);

        switch (team)
        {
            case BorderTeam.player1:
                m_scoreP2 += 1;
                UpdateScore();
                break;
            case BorderTeam.player2:
                m_scoreP1 += 1;
                UpdateScore();
                break;
            case BorderTeam.none:
                break;
        }
    }

    private void UpdateScore()
    {
        m_UIHandler.UpdateScore(m_scoreP1, m_scoreP2);
    }

    private void OnDestroy()
    {
        m_GoalP1.GoalEvent -= ScoreGoal;
        m_GoalP2.GoalEvent -= ScoreGoal;
       // m_PauseMenu.SwitchToMenu -= SwitchToMenu;

    }

}