using GameSystems.Scene;
using System;
using UnityEngine;

public class GameManager : SingletonMonoPersistent<GameManager>
{
    private Action m_StartGame;


    private Ball m_Ball;
    private PlayerNet m_NetP1;
    private PlayerNet m_NetP2;

    private int m_scoreP1;
    private int m_scoreP2;

    private PlayerControls m_Player1;
    private PlayerControls m_Player2;
    private HotSeatControllesSetup m_HotSeatControls;

    [SerializeField]
    private UIHandler m_UIHandler;

    private MultiplayerMode m_MultiplayerMode;

    public void Start()
    {
        m_StartGame += SetupGame;
        m_UIHandler.OnControlsRebinding.AddListener(SetActiveControls);
    }

    public void ChangeGameLevel(MultiplayerMode mpMode, string level)
    {
        m_MultiplayerMode = mpMode;
        LoadGameLevel(level);
    }

    private void SetupGame()
    {
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
        StartGame();
    }

    private void StartGame()
    {
        UpdateScore();
        m_Ball.Restart(Vector3.zero);
    }

    private void LoadGameLevel(string level)
    {
        Debug.Log("Начало загрузки");
        StartCoroutine(SceneChanger.Instance.ChangeScene(level, m_StartGame));
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

    private void SetupHotSeat()
    {
        LevelBuilder m_Builder;
        if (TryGetComponent(out m_Builder))
        {

        }
        else
        {
            m_Builder = gameObject.AddComponent<LevelBuilder>();
        }

        Destroy(GetComponent<HotSeatControllesSetup>());

        m_HotSeatControls = gameObject.AddComponent<HotSeatControllesSetup>();
        m_HotSeatControls.SpawnPlayers(out m_Player1, out m_Player2);
        m_Builder.BuildHotSeatlevel(m_Player1, m_Player2, out m_Ball, out m_NetP1, out m_NetP2);

        Destroy(GetComponent(m_Builder.GetType()));

        m_NetP1.GoalEvent += ScoreGoal;
        m_NetP2.GoalEvent += ScoreGoal;

        m_Player1.OnOpenMenu.AddListener(PauseGame);

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

    private void SetActiveControls(bool active)
    {
        Debug.Log("Controls is now " + active);
    }

    private void OnDestroy()
    {
        Destroy(m_HotSeatControls);
    }

}