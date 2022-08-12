using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNet : MonoBehaviour
{
    public delegate void Goalhandlerer(BorderTeam hit);
    public event Goalhandlerer GoalEvent;

    [SerializeField]
    private BorderTeam m_Team;


    private void OnTriggerEnter(Collider other)
    {
        if (m_Team != BorderTeam.none)
        {
            if (other.TryGetComponent<Ball>(out Ball ball))
            {
                GoalEvent.Invoke(m_Team);
            }
        }
    }

    
}

public enum BorderTeam
{
    player1,
    player2,
    none
}
