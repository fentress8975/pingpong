using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStartGame : MonoBehaviour
{
    [SerializeField]
    private Button m_StartButton;

    private bool m_isOptionsEnabled = false;

    private void Start()
    {
        m_StartButton.onClick.AddListener(ShowOptions);
    }

    private void ShowOptions()
    {
        if (m_isOptionsEnabled)
        {
            foreach (var go in GetComponentsInChildren<Button>(true))
            {
                if (go == m_StartButton) continue;
                go.gameObject.SetActive(false);
            }
            m_isOptionsEnabled = false; 
        }
        else
        {
            foreach (var go in GetComponentsInChildren<Button>(true))
            {
                if (go == m_StartButton) continue;
                go.gameObject.SetActive(true);
            }
            m_isOptionsEnabled = true;
        }

    }
}
