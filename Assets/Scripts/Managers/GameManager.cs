using GameSystems.Scene;
using System;
using UnityEngine;


public class GameManager : SingletonMonoPersistent<GameManager>
{
    private Action m_SetupGame;
    private Action m_StartGame;

    private Ball m_Ball;
    private PlayerNet m_NetP1;
    private PlayerNet m_NetP2;

    private int m_scoreP1;
    private int m_scoreP2;

    private PlayerControls m_Player1;
    private string m_Player1Name;
    private PlayerControls m_Player2;
    private AIController m_Player2AI;
    private string m_Player2Name;
    private GameControllesSetup m_GameControls;

    private GameSettings m_GameSettings;
    private LevelAnimationHandler m_Animator;
    private LevelBuilder m_Builder;
    private int m_PoitnsToWin;
    private int m_Difficulty;

    [SerializeField]
    private UIHandler m_UIHandler;

    private UI.MultiplayerMode m_MultiplayerMode;

    public void Start()
    {
        m_SetupGame += SetupGame;
        m_StartGame += StartGame;
    }

    public void StartGameLevel(UI.MultiplayerMode mpMode, string level, GameSettings gameSettings)
    {
        m_MultiplayerMode = mpMode;
        m_GameSettings = gameSettings;
        LoadGameLevel(level);
    }

    public void RematchGame()
    {
        ResetPlayerPoints();
        RestartBall();
        StartLevelAnimation();
    }

    private void ResetPlayerPoints()
    {
        m_scoreP1 = 0;
        m_scoreP2 = 0;
    }

    private void SetupGame()
    {
        GameSystemManager.Instance.SwitchGameState(GameState.Pause);
        switch (m_MultiplayerMode)
        {
            case UI.MultiplayerMode.HOTSEAT:
                SetupHotSeat();
                break;
            case UI.MultiplayerMode.Online:
                break;
            case UI.MultiplayerMode.Single:
                SetupSinglePlayer();
                break;
            default:
                break;
        }
    }

    private void StartGame()
    {
        ResetPlayerPoints();
        UpdateScore();
        SetActivePlayersControls(true);
        m_Ball.Restart();
    }
    private void LoadGameLevel(string level)
    {
        Debug.Log("Начало загрузки");
        StartCoroutine(SceneChanger.Instance.ChangeScene(level, m_SetupGame));
    }
    private void PauseGame()
    {
        Debug.Log("PausingGame");
        if (GameSystemManager.Instance.gameState == GameState.Active)
        {
            GameSystemManager.Instance.SwitchGameState(GameState.Pause);
        }
        else if (GameSystemManager.Instance.gameState == GameState.Pause)
        {
            GameSystemManager.Instance.SwitchGameState(GameState.Active);
        }
    }
    private void SetupModules()
    {
        PrepareLevelBuilder();
        PrepareLevelAnimator();
        PreparePlayerSpawner();
    }
    private void PrepareLevelBuilder()
    {
        if (TryGetComponent(out m_Builder))
        {
            Destroy(GetComponent(m_Builder.GetType()));
        }
        Debug.Log("Making New LevelBuilder");
        m_Builder = gameObject.AddComponent<LevelBuilder>();
    }
    private void PrepareLevelAnimator()
    {
        if (TryGetComponent(out m_Animator))
        {
            Destroy(GetComponent(m_Animator.GetType()));
            return;
        }
        Debug.Log("Making New LevelAnimator");
        m_Animator = gameObject.AddComponent<LevelAnimationHandler>();
    }
    private void PreparePlayerSpawner()
    {
        if (TryGetComponent(out m_GameControls))
        {
            Destroy(GetComponent(m_GameControls.GetType()));
        }
        Debug.Log("Making New PlayerSpawner");
        m_GameControls = gameObject.AddComponent<GameControllesSetup>();
    }


    private void SetupSinglePlayer()
    {
        SetupModules();

        m_GameControls.SpawnPlayer(out m_Player1, out m_Player2AI);
        m_Builder.BuildSinglePlayerlevel(m_Player1, m_Player2AI, out m_Ball, out m_NetP1, out m_NetP2);

        SetUpSubcripes();
        ReadGameSettings();
        StartLevelAnimation();
        GameSystemManager.Instance.SwitchGameState(GameState.Active);
    }

    private void SetupHotSeat()
    {
        SetupModules();

        m_GameControls.SpawnHotSeatPlayers(out m_Player1, out m_Player2);
        m_Builder.BuildHotSeatlevel(m_Player1, m_Player2, out m_Ball, out m_NetP1, out m_NetP2);
        SetActivePlayersControls(false);

        SetUpSubcripes();
        ReadGameSettings();
        StartLevelAnimation();
        GameSystemManager.Instance.SwitchGameState(GameState.Active);
    }

    private void SetActivePlayersControls(bool active)
    {
        m_Player1.enabled = active;
        if (m_Player2)
        {
            m_Player2.enabled = active;
        }
        if (m_Player2AI)
        {
            m_Player2AI.enabled = active;
        }
    }

    private void StartLevelAnimation()
    {
        switch (m_MultiplayerMode)
        {
            case UI.MultiplayerMode.Single:
                m_Animator.StartArena(m_Player1.transform, m_Player2AI.transform, m_Ball);
                break;
            case UI.MultiplayerMode.HOTSEAT:
                m_Animator.StartArena(m_Player1.transform, m_Player2.transform, m_Ball);
                break;
            case UI.MultiplayerMode.Online:
                m_Animator.StartArena(m_Player1.transform, m_Player2.transform, m_Ball);
                break;
        }
        
    }

    private void SetUpSubcripes()
    {
        m_NetP1.GoalEvent += ScoreGoal;
        m_NetP2.GoalEvent += ScoreGoal;
        m_Player1.OnOpenMenu.AddListener(PauseGame);
        m_Animator.onReady.AddListener(StartGame);
    }

    private void ReadGameSettings()
    {
        Debug.Log("Booster " + m_GameSettings.BallBooster);
        Debug.Log("Difficulty " + m_GameSettings.Difficulty);
        Debug.Log("Points to win " + m_GameSettings.PointsToWin);
        m_Ball.ChangeBoost(m_GameSettings.BallBooster);
        m_Ball.Init(m_GameSettings.Difficulty);
        m_PoitnsToWin = m_GameSettings.PointsToWin;
    }

    private void ScoreGoal(BorderTeam team)
    {
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

        if (isFinalScore())
        {
            m_Ball.StopBall();
            ShowVictoryScreen();
        }
        else
        {
            RestartBall();
        }
    }

    private void ShowVictoryScreen()
    {
        if (m_scoreP1 == m_PoitnsToWin)
        {
            UIHandler.Instance.ShowVictoryPanel(BorderTeam.player1);
        }
        else
        {
            UIHandler.Instance.ShowVictoryPanel(BorderTeam.player2);
        }

    }

    private void RestartBall()
    {
        m_Ball.Restart();
    }

    private bool isFinalScore()
    {
        if (m_scoreP1 == m_PoitnsToWin || m_scoreP2 == m_PoitnsToWin)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateScore()
    {
        m_UIHandler.UpdateScore(m_scoreP1, m_scoreP2);
    }


    private void OnDestroy()
    {

    }

}