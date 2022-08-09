using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public delegate void SwitchSceneHandler();
    public event SwitchSceneHandler SwitchToMenu;

    [SerializeField]
    private GameObject m_RebindPanel;
    [SerializeField]
    private Button m_ExitGame;
    [SerializeField]
    private Button m_BackToMenu;

    private void Start()
    {
        DisablePauseMenu();
        m_ExitGame.onClick.AddListener(ExitGame);
        m_BackToMenu.onClick.AddListener(SwitchSceneToMenu);
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
        Debug.Log("kok");
    }

    private void DisablePauseMenu()
    {
        gameObject.SetActive(false);
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
