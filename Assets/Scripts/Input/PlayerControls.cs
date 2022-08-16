using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public UnityEvent OnOpenMenu;

    private Rigidbody m_Player;

    private float m_fSpeed = 6f;
    private BarMovementDirection m_Direction;

    private bool m_IsMovingUp = false;
    private bool m_IsMovingDown = false;

    private void Start()
    {
        m_Player = GetComponent<Rigidbody>();
    }

    public void MoveUp(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                m_Direction = BarMovementDirection.UP;
                m_IsMovingUp = true;
                break;
            case InputActionPhase.Canceled:
                m_IsMovingUp = false;
                break;
        }
    }

    public void MoveDown(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                m_Direction = BarMovementDirection.DOWN;
                m_IsMovingDown = true;
                break;
            case InputActionPhase.Canceled:
                m_IsMovingDown = false;
                break;
        }
    }

    public void OpenMenu(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                OnOpenMenu?.Invoke();
                break;
        }
    }

    private void Move()
    {
        CheckDirection();

        switch (m_Direction)
        {
            case BarMovementDirection.UP:
                m_Player.position += m_fSpeed * Time.deltaTime * Vector3.forward;
                break;
            case BarMovementDirection.DOWN:
                m_Player.position += m_fSpeed * Time.deltaTime * Vector3.back;
                break;
            case BarMovementDirection.STOP:
                break;
        }
    }

    private void CheckDirection()
    {
        if ((m_IsMovingDown == false && m_IsMovingUp == false) || (m_IsMovingDown && m_IsMovingUp))
        {
            m_Direction = BarMovementDirection.STOP;
        }
        else if (m_IsMovingUp == false && m_IsMovingDown)
        {
            m_Direction = BarMovementDirection.DOWN;
        }
        else if (m_IsMovingUp && m_IsMovingDown == false)
        {
            m_Direction = BarMovementDirection.UP;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }
}

public enum BarMovementDirection
{
    UP,
    DOWN,
    STOP
}
