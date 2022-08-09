using GameSystems.Scene;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private const string GameScene = "Game";

    [SerializeField]
    private Button m_StartGameButton;
    [SerializeField]
    private Button m_OpenOptionsButton;
    [SerializeField]
    private Button m_CloseOptionsButton;
    [SerializeField]
    private Button m_CloseAppButton;
    [SerializeField]
    private GameObject m_OptionsMenu;

    private ISceneChanger m_SceneChanger;

    private void SubcribeButtons()
    {
        m_StartGameButton.onClick.AddListener(StartGame);
        m_OpenOptionsButton.onClick.AddListener(OpenOptions);
        m_CloseOptionsButton.onClick.AddListener(OpenOptions);
        m_CloseAppButton.onClick.AddListener(CloseApp);
    }

    private void Start()
    {
        m_OptionsMenu.SetActive(false);

        m_SceneChanger = (ISceneChanger)SceneChanger.Instance;

        SubcribeButtons();
    }

    private void StartGame()
    {
        m_SceneChanger.ChangeScene(GameScene);
    }

    private void OpenOptions()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            m_OptionsMenu.SetActive(true);
        }
        else
        {
            m_OptionsMenu.SetActive(false);
            gameObject.SetActive(true);
        }
    }

    private void CloseApp()
    {
#if UNITY_EDITOR

        EditorApplication.ExitPlaymode();

#endif

#if UNITY_STANDALONE

        Application.Quit();

#endif
    }
}

public enum MultiplayerMode
{
    HOTSEAT,
    Online
}
