using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsSaver : MonoBehaviour
{
    [SerializeField]
    private Slider m_PointsToWinSlider;
    [SerializeField]
    private Slider m_BallBoosterSlider;

    private void SaveGameSettings()
    {
        PlayerPrefs.SetFloat("BallBooster", m_BallBoosterSlider.value);
        PlayerPrefs.SetFloat("PointsToWin", m_PointsToWinSlider.value);
    }

    private void LoadGameSettings()
    {
        m_BallBoosterSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("BallBooster"));
        m_PointsToWinSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("PointsToWin"));
    }

    private void OnEnable()
    {
        LoadGameSettings();
    }

    private void OnDisable()
    {
        SaveGameSettings();
    }
}
