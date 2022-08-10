using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public UnityEvent<bool> ControlsRemaping;

    public delegate void SwitchSceneHandler();
    public event SwitchSceneHandler SwitchToMenu;

    [SerializeField]
    private RebindOverlay m_RebindOverlay;
    [SerializeField]
    private Button m_ExitGame;
    [SerializeField]
    private Button m_BackToMenu;

    private void Start()
    {
        DisablePauseMenu();
        m_ExitGame.onClick.AddListener(ExitGame);
        m_BackToMenu.onClick.AddListener(SwitchSceneToMenu);

        m_RebindOverlay.RemapingControls.AddListener(RemapingControls);
    }

    public void OpenPauseMenu()
    {
        PauseGame();
        EnablePauseMenu();
    }

    public void ClosePauseMenu()
    {
        PauseGame();
        DisablePauseMenu();
    }

    private void EnablePauseMenu()
    {
        gameObject.SetActive(true);
        Debug.Log("kokon");
    }

    private void DisablePauseMenu()
    {
        gameObject.SetActive(false);
        Debug.Log("kokoff");
    }

    private void PauseGame()
    {
        if(Time.timeScale == 1)
        {
        Time.timeScale = 0;
        }
        else if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    private void SwitchSceneToMenu()
    {
        SwitchToMenu?.Invoke();
    }

    private void RemapingControls(bool isRemaping)
    {
        ControlsRemaping?.Invoke(!isRemaping);
    }

    private void ExitGame()
    {
#if UNITY_EDITOR

        EditorApplication.ExitPlaymode();

#endif

#if UNITY_STANDALONE

        Application.Quit();

#endif
    }
}
