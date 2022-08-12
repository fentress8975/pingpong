using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject m_PlayerPrefab;
    [SerializeField]
    private Material m_RedSide;
    [SerializeField]
    private Material m_BlueSide;
    [SerializeField]
    private GameObject m_BallPrefab;
    [SerializeField]
    private GameObject m_NetPrefab;

    public PlayerNet Player1Net
    {
        get;
        private set;
    }
    public PlayerNet Player2Net
    {
        get;
        private set;
    }

    private List<Rigidbody> m_Players;
    private GameLevelSettings m_LevelSettings;

    public List<Rigidbody> BuildLevel()
    {
        m_LevelSettings = FindObjectOfType<GameLevelSettings>();

        SpawnNet();
        SpawnPlayers();
        SpawnBall();

        return m_Players;
    }

    private void SpawnNet()
    {
        Player1Net = Instantiate(m_NetPrefab).GetComponent<PlayerNet>();
        Player2Net = Instantiate(m_NetPrefab).GetComponent<PlayerNet>();
        Player1Net.transform.position = m_LevelSettings.P1NetSpawnPos.position;
        Player2Net.transform.position = m_LevelSettings.P2SpawnPos.position;
    }

    private void SpawnBall()
    {
        Instantiate(m_BallPrefab,m_LevelSettings.BallSpawnPos);
    }

    private void SpawnPlayers()
    {
        GameObject player1 = Instantiate(m_PlayerPrefab);
        GameObject player2 = Instantiate(m_PlayerPrefab);

        player1.GetComponent<Material>().CopyPropertiesFromMaterial(m_RedSide);
        player2.GetComponent<Material>().CopyPropertiesFromMaterial(m_BlueSide);

        player1.transform.position = m_LevelSettings.P1SpawnPos.position;
        player2.transform.position = m_LevelSettings.P2SpawnPos.position;

        m_Players.Add(player1.GetComponent<Rigidbody>());
        m_Players.Add(player2.GetComponent<Rigidbody>());
    }
}
