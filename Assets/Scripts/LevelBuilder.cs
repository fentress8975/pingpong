using System;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField]
    private Material m_RedSide;
    [SerializeField]
    private Material m_BlueSide;
    [SerializeField]
    private GameObject m_BallPrefab;
    [SerializeField]
    private GameObject m_NetPrefab;

    private GameLevelSettings m_LevelSettings;

    private void Awake()
    {
        LoadResources();
    }

    private void LoadResources()
    {
        m_RedSide = Resources.Load<Material>("Materials/Player1");
        m_BlueSide = Resources.Load<Material>("Materials/Player2");
        m_BallPrefab = Resources.Load<GameObject>("Prefabs/Ball");
        m_NetPrefab = Resources.Load<GameObject>("Prefabs/Net");
    }

    public void BuildHotSeatlevel(PlayerControls player1, PlayerControls player2, out Ball ball, out PlayerNet playerNet1, out PlayerNet playerNet2)
    {
        m_LevelSettings = FindObjectOfType<GameLevelSettings>();
        GameObject Player1 = player1.gameObject;
        GameObject Player2 = player2.gameObject;
        SetUpPlayers(Player1, Player2);
        SpawnBall(out ball);
        SpawnNet(out playerNet1, out playerNet2);
    }

    public void BuildSinglePlayerlevel(PlayerControls player1, AIController player2, out Ball ball, out PlayerNet playerNet1, out PlayerNet playerNet2)
    {
        m_LevelSettings = FindObjectOfType<GameLevelSettings>();
        GameObject Player1 = player1.gameObject;
        GameObject Player2 = player2.gameObject;
        SetUpPlayers(Player1, Player2);
        SpawnBall(out ball);
        SpawnNet(out playerNet1, out playerNet2);
    }
    

    private void SpawnNet(out PlayerNet p1Net, out PlayerNet p2Net)
    {
        GameObject player1net = Instantiate(m_NetPrefab);
        GameObject player2net = Instantiate(m_NetPrefab);

        player1net.transform.position = m_LevelSettings.P1NetSpawnPos.position;
        player2net.transform.position = m_LevelSettings.P2NetSpawnPos.position;
        player1net.transform.rotation = m_LevelSettings.P1NetSpawnPos.rotation;
        player2net.transform.rotation = m_LevelSettings.P2NetSpawnPos.rotation;

        PlayerNet Player1Net = player1net.GetComponent<PlayerNet>();
        PlayerNet Player2Net = player2net.GetComponent<PlayerNet>();

        Player1Net.ChangeTeam(BorderTeam.player1);
        Player2Net.ChangeTeam(BorderTeam.player2);

        p1Net = Player1Net;
        p2Net = Player2Net;

    }

    private void SpawnBall(out Ball ball)
    {
        GameObject BallGO = Instantiate(m_BallPrefab);
        BallGO.transform.position = m_LevelSettings.BallSpawnPos.position;
        ball = BallGO.GetComponent<Ball>();
        ball.SetStartPos(m_LevelSettings.BallSpawnPos.position);
    }

    private static void SetPlayerName(GameObject player1, GameObject player2)
    {
        player1.name = "Player1";
        player2.name = "Player2";
    }

    private void SetPlayerPosition(GameObject player1, GameObject player2)
    {
        player1.transform.position = m_LevelSettings.P1SpawnPos.position;
        player1.transform.rotation = m_LevelSettings.P1SpawnPos.rotation;

        player2.transform.position = m_LevelSettings.P2SpawnPos.position;
        player2.transform.rotation = m_LevelSettings.P2SpawnPos.rotation;
    }

    private void SetPlayerMaterials(GameObject player1, GameObject player2)
    {
        var meshP1 = player1.GetComponent<MeshRenderer>();
        var meshP2 = player2.GetComponent<MeshRenderer>();
        meshP1.material = m_RedSide;
        meshP2.material = m_BlueSide;
    }

    private void SetUpPlayers(GameObject player1, GameObject player2)
    {
        SetPlayerName(player1, player2);
        SetPlayerMaterials(player1, player2);
        SetPlayerPosition(player1, player2);
    }
}
