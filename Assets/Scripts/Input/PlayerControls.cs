using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public UnityEvent OnOpenMenu;

    private Rigidbody m_Player;
    [SerializeField]
    private GameObject m_StunImg;

    private float m_fSpeed = 6f;
    private BarMovementDirection m_Direction;

    private bool m_IsMovingUp = false;
    private bool m_IsMovingDown = false;

    private enum BarState
    {
        Active,
        Stuned
    }

    private BarState m_State = BarState.Active;

    private void Start()
    {
        m_StunImg.SetActive(false);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Border border))
        {
            m_State = BarState.Stuned;
            StartCoroutine(StunBar());
        }
    }

    private IEnumerator StunBar()
    {
        m_StunImg.SetActive(true);
        StartCoroutine(StunAnimation());

        Debug.Log($"{gameObject.name} stunned");
        Vector3 destination = m_Direction == BarMovementDirection.UP ? transform.position + Vector3.back : transform.position + Vector3.forward;
        float current = 0;
        float target = 1;
        while (current != target)
        {
            current = Mathf.MoveTowards(current, target, m_fSpeed * 0.1f * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, destination, current);
            yield return null;
        }
        Debug.Log($"{gameObject.name} Active");
        m_State = BarState.Active;

        StopCoroutine(StunAnimation());
        m_StunImg.SetActive(false);
    }

    private IEnumerator StunAnimation()
    {
        while(true)
        {
        m_StunImg.transform.Rotate(new Vector3(0, 0, 45));
        yield return new WaitForSeconds(0.2f);
        }
    }

    private void FixedUpdate()
    {
        switch (m_State)
        {
            case BarState.Active:
                Move();
                break;
            case BarState.Stuned:

                break;
        }

    }
}

public enum BarMovementDirection
{
    UP,
    DOWN,
    STOP
}
