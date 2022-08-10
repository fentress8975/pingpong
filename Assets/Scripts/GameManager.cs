using GameSystems.Scene;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private const string SceneMainMenu = "MainMenu";

    [SerializeField]
    private Ball m_Ball;
    [SerializeField]
    private Border m_BorderP1;
    [SerializeField]
    private Border m_BorderP2;
    private int m_scoreP1;
    private int m_scoreP2;

    [SerializeField]
    private Rigidbody m_Player1;
    [SerializeField]
    private Rigidbody m_Player2;

    [SerializeField]
    private PlayerInput m_PlayerInputs1;
    [SerializeField]
    private PlayerInput m_PlayerInputs2;
    private List<PlayerInput> m_ListInputs = new();

    [SerializeField]
    private TMP_Text m_scoreTextP1;
    [SerializeField]
    private TMP_Text m_scoreTextP2;

    [SerializeField]
    private PauseMenu m_PauseMenu;

    private ISceneChanger m_SceneChanger;

    private void Start()
    {
        //m_InputsControl = InputsControl.Instance.GetComponent<InputsControl>();
        m_SceneChanger = (ISceneChanger)SceneChanger.Instance;

        m_BorderP1.e_Score += ScoreGoal;
        m_BorderP2.e_Score += ScoreGoal;
        UpdateScore();
        MultiplayerMode mode = new();
        switch (mode)
        {
            case MultiplayerMode.HOTSEAT:
                SetupHotSeat();
                break;
            case MultiplayerMode.Online:
                SetupMP();
                break;
            default:
                break;
        }

        m_PauseMenu.SwitchToMenu += SwitchToMenu;
        //m_PauseMenu.ControlsRemaping.AddListener(SetActiveControls);
        m_ListInputs.Add(m_PlayerInputs1);
        m_ListInputs.Add(m_PlayerInputs2);
        m_ListInputs.Add(GetComponent<PlayerInput>());
    }

    private void Init(MultiplayerMode mpMode)
    {

        switch (mpMode)
        {
            case MultiplayerMode.HOTSEAT:
                SetupHotSeat();
                break;
            case MultiplayerMode.Online:
                SetupMP();
                break;
            default:
                break;
        }
    }

    public void OnPauseGame(InputAction.CallbackContext ctx)
    {
        switch (ctx.phase)
        {
            case InputActionPhase.Started:
                PauseGame();
                break;
        }
        
    }

    private void PauseGame()
    {
        if (m_PauseMenu.gameObject.activeInHierarchy == true)
        {
            m_PauseMenu.ClosePauseMenu();
            
        }
        else if (m_PauseMenu.gameObject.activeInHierarchy == false)
        {
            m_PauseMenu.OpenPauseMenu();
        }
        

    }

    private void SetupHotSeat()
    {

    }
    private void SetupMP()
    {
        throw new NotImplementedException();
    }

    private void ScoreGoal(Border.BorderTeam team)
    {
        m_Ball.Restart(Vector3.zero);

        switch (team)
        {
            case Border.BorderTeam.player1:
                m_scoreP2 += 1;
                UpdateScore();
                break;
            case Border.BorderTeam.player2:
                m_scoreP1 += 1;
                UpdateScore();
                break;
            case Border.BorderTeam.none:
                break;
        }
    }

    private void UpdateScore()
    {
        m_scoreTextP2.text = $"Player2 score: {m_scoreP2}";
        m_scoreTextP1.text = $"Player1 score: {m_scoreP1}";
    }

    private void OnDestroy()
    {
        m_BorderP1.e_Score -= ScoreGoal;
        m_BorderP2.e_Score -= ScoreGoal;
        m_PauseMenu.SwitchToMenu -= SwitchToMenu;

    }

    private void SwitchToMenu()
    {
        m_PauseMenu.ClosePauseMenu();
        SceneChanger.Instance.ChangeScene(SceneMainMenu);
    }


    private void SetActiveControls(bool active)
    {
        if (active)
        {
            foreach (var playerInput in m_ListInputs)
            {
                playerInput.enabled = true;
                playerInput.ActivateInput();
                playerInput.currentActionMap.Enable();
                playerInput.actions.Enable();

            }
        }
        else
        {
            foreach (var playerInput in m_ListInputs)
            {
                playerInput.actions.Disable();
                playerInput.currentActionMap.Disable();
                playerInput.DeactivateInput();
                playerInput.enabled = false;
                
            }
        }
    }
}