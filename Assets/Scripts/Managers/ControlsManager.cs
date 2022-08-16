using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsManager : SingletonMonoPersistent<ControlsManager>
{
    public InputActionAsset InputActions
    {
        get => m_InputActionAsset;
        private set
        {
            m_InputActionAsset = value;
        }
    }

    public GameObject HotSeatControls
    {
        get => m_HotSeatControls;
        private set
        {
            m_HotSeatControls = value;
        }
    }


    [SerializeField]
    private InputActionAsset m_InputActionAsset;
    [SerializeField]
    private GameObject m_HotSeatControls;
}
