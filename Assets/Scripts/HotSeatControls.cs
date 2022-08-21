using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSeatControls : MonoBehaviour
{
    public PlayerControls Player1
    {
        get => m_Player1;
        private set
        {
            m_Player1 = value;
        }
    }

    public PlayerControls Player2
    {
        get => m_Player2;
        private set
        {
            m_Player2 = value;
        }
    }


    [SerializeField]
    private PlayerControls m_Player1;
    [SerializeField]
    private PlayerControls m_Player2;
}
