using UnityEngine;

public class PlayersHotSeatControls : MonoBehaviour
{
    private Rigidbody m_Player1;
    private Rigidbody m_Player2;

    private float m_fSpeed = 6f;

    private bool m_bP1Moving = false;
    private bool m_bP2Moving = false;
    private BarMovementDirection m_P1Direction;
    private BarMovementDirection m_P2Direction;

    public void Init(Rigidbody player1, Rigidbody player2, InputsControl input)
    {
        m_Player1 = player1;
        m_Player2 = player2;

        //SetupControls(input);
    }

    private void SetupControls(InputsControl input)
    {
        input.Player1Movement += MovePlayer1;
        input.Player2Movement += MovePlayer2;
    }

    private void MovePlayer1(BarMovementDirection dir)
    {

        switch (dir)
        {
            case BarMovementDirection.UP:
                m_bP1Moving = true;
                break;
            case BarMovementDirection.DOWN:
                m_bP1Moving = true;
                break;
            case BarMovementDirection.STOP:
                m_bP1Moving = false;
                break;
            default:
                m_bP1Moving = false;
                break;
        }
        m_P1Direction = dir;
    }

    private void MovePlayer2(BarMovementDirection dir)
    {
        switch (dir)
        {
            case BarMovementDirection.UP:
                m_bP2Moving = true;
                break;
            case BarMovementDirection.DOWN:
                m_bP2Moving = true;
                break;
            case BarMovementDirection.STOP:
                m_bP2Moving = false;
                break;
            default:
                m_bP2Moving = false;
                break;
        }
        m_P2Direction = dir;
    }

    private void MovePlayers()
    {
        if (m_bP1Moving)
        {
            switch (m_P1Direction)
            {
                case BarMovementDirection.UP:
                    m_Player1.position += m_fSpeed * Time.deltaTime * Vector3.forward;
                    break;
                case BarMovementDirection.DOWN:
                    m_Player1.position += m_fSpeed * Time.deltaTime * Vector3.back;
                    break;
                case BarMovementDirection.STOP:
                    break;
            }
        }

        if (m_bP2Moving)
        {
            switch (m_P2Direction)
            {
                case BarMovementDirection.UP:
                    m_Player2.position += m_fSpeed * Time.deltaTime * Vector3.forward;
                    break;
                case BarMovementDirection.DOWN:
                    m_Player2.position += m_fSpeed * Time.deltaTime * Vector3.back;
                    break;
                case BarMovementDirection.STOP:
                    break;
            }
        }
    }

    private void Update()
    {
        MovePlayers();
    }
}

public enum BarMovementDirection
{
    UP,
    DOWN,
    STOP
}