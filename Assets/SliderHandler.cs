using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SliderHandler : MonoBehaviour
{
    [SerializeField]
    private Slider m_Slider;
    [SerializeField]
    private TextMeshProUGUI m_TextMeshPro;

    private int m_CurrentNumber;

    void Start()
    {
        m_Slider = GetComponent<Slider>();
        m_Slider.onValueChanged.AddListener(ChangeCurrentNumber);
        ChangeCurrentNumber(m_Slider.value);
    }

    private void ChangeCurrentNumber(float arg0)
    {
        m_CurrentNumber = (int)m_Slider.value;
        UpdateDislpay();
    }

    private void UpdateDislpay()
    {
        m_TextMeshPro.SetText(m_CurrentNumber.ToString());
    }
}
