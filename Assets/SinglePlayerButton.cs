using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SinglePlayerButton : MonoBehaviour
{
    public UnityEvent<AIDificulty> OnClickDifficulty;

    [SerializeField]
    private Button m_EasyButton;
    [SerializeField]
    private Button m_MediumButton;
    [SerializeField]
    private Button m_HardButton;

    void Start()
    {
        SetUpListeners();

    }

    private void SetUpListeners()
    {
        m_EasyButton.onClick.AddListener(StartEasy);
        m_MediumButton.onClick.AddListener(StartMedium);
        m_HardButton.onClick.AddListener(StartHard);

    }

    private void StartEasy()
    {
        OnClickDifficulty?.Invoke(AIDificulty.EASY);
    }
    private void StartMedium()
    {
        OnClickDifficulty?.Invoke(AIDificulty.MEDIUM);
    }
    private void StartHard()
    {
        OnClickDifficulty?.Invoke(AIDificulty.HARD);
    }
}

public enum AIDificulty
{
    NONE,
    EASY,
    MEDIUM,
    HARD
}
