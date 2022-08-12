using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameTimeControl
{
    public static void UnPause()
    {
        UnpauseGame();
    }

    public static void Pause()
    {
        PauseGame();
    }
    
    private static void PauseGame()
    {
        Time.timeScale = 0;
    }

    private static void UnpauseGame()
    {
        Time.timeScale = 1;
    }
}
