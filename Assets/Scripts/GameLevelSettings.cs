using UnityEngine;

public class GameLevelSettings : MonoBehaviour 
{
    public Transform P1SpawnPos
    {
        get => m_P1SpawnPos;
        private set
        {
            m_P1SpawnPos = value;
        }
    }
    public Transform P2SpawnPos
    {
        get => m_P2SpawnPos;
        private set
        {
            m_P2SpawnPos = value;
        }
    }
    public Transform BallSpawnPos
    {
        get => m_BallSpawnPos;
        private set
        {
            m_BallSpawnPos = value;
        }
    }
    public Transform P1NetSpawnPos
    {
        get => m_P1NetSpawnPos;
        private set
        {
            m_P1NetSpawnPos = value;
        }
    }
    public Transform P2NetSpawnPos
    {
        get => m_P2NetSpawnPos;
        private set
        {
            m_P2SpawnPos=value;
        }
    }


    [Tooltip("Player 1 Start Position")]
    [SerializeField]
    private Transform m_P1SpawnPos;
    [Tooltip("Player 2 Start Position")]
    [SerializeField]
    private Transform m_P2SpawnPos;
    [Tooltip("Game Ball Start Position")]
    [SerializeField]
    private Transform m_BallSpawnPos;
    [Tooltip("Player 1 Net Position")]
    [SerializeField]
    private Transform m_P1NetSpawnPos;
    [Tooltip("Player 2 Net Position")]
    [SerializeField]
    private Transform m_P2NetSpawnPos;

}
