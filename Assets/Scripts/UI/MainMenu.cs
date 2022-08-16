using GameSystems.Scene;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public UnityEvent<MultiplayerMode, string> StartGameEvent;

    [SerializeField]
    private Button m_StartHotSeat;
    [SerializeField]
    private Button m_StartMP;
    [SerializeField]
    private TMP_Dropdown m_LevelChoose;

    [SerializeField]
    private Button m_OpenOptionsButton;
    [SerializeField]
    private Button m_CloseOptionsButton;

    [SerializeField]
    private Button m_CloseAppButton;
    [SerializeField]
    private GameObject m_OptionsMenu;
    [SerializeField]
    private GameObject m_FaceMenu;

    private ISceneChanger m_SceneChanger;

    private void SubcribeButtons()
    {
        m_StartHotSeat.onClick.AddListener(StartHotSeatGame);
        m_StartMP.onClick.AddListener(StartOnlineGame);
        m_OpenOptionsButton.onClick.AddListener(OpenOptions);
        m_CloseOptionsButton.onClick.AddListener(OpenOptions);
        m_CloseAppButton.onClick.AddListener(CloseApp);
    }

    private void Start()
    {
        m_OptionsMenu.SetActive(false);

        m_SceneChanger = (ISceneChanger)SceneChanger.Instance;

        SubcribeButtons();

        m_LevelChoose.ClearOptions();
        m_LevelChoose.AddOptions(SceneChanger.Instance.GetSceneList());
    }

    private void StartHotSeatGame()
    {
        Debug.Log("Starting HotSeat Game");
        StartGameEvent?.Invoke(MultiplayerMode.HOTSEAT, m_LevelChoose.captionText.text);
    }

    private void StartOnlineGame()
    {
        StartGameEvent?.Invoke(MultiplayerMode.Online, m_LevelChoose.itemText.text);
    }

    private void OpenOptions()
    {
        if (m_OptionsMenu.gameObject.activeInHierarchy)
        {
            m_FaceMenu.SetActive(true);
            m_OptionsMenu.SetActive(false);
        }
        else
        {
            m_FaceMenu.SetActive(false);
            m_OptionsMenu.SetActive(true);
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
