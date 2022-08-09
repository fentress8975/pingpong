using UnityEngine;
using UnityEngine.InputSystem;

public class InputsControl : SingletonMonoPersistent<InputsControl>
{
    public delegate void Player1MovementHandler(BarMovementDirection direction);
    public delegate void Player2MovementHandler(BarMovementDirection direction);

    public event Player1MovementHandler Player1Movement;
    public event Player2MovementHandler Player2Movement;

    private PC_Controls m_PCControls;

    private InputActionAsset inputActions;

    private bool m_P1IsMovingUp = false;
    private bool m_P1IsMovingDown = false;

    private bool m_P2IsMovingUp = false;
    private bool m_P2IsMovingDown = false;


    private void Start()
    {
        m_PCControls = new PC_Controls();
        SetupEvents();
        m_PCControls.Enable();
        RemapButtonClicked(m_PCControls.Player1Controls.MoveDown);
    }

    void RemapButtonClicked(InputAction actionToRebind)
    {
        Debug.Log("Kek");
        Disable();
        var rebindOperation = actionToRebind.PerformInteractiveRebinding()
                    // To avoid accidental input from mouse motion
                    .WithControlsExcluding("Mouse")
                    .OnMatchWaitForAnother(0.1f)
                    .OnComplete(callback =>
                    {
                        Enable();
                        callback.Dispose();
                    })
                    .Start();
    }

    private void SetupEvents()
    {
        m_PCControls.Player1Controls.MoveUp.started += ctx => Player1MoveUp();
        m_PCControls.Player1Controls.MoveDown.started += ctx => Player1MoveDown();
        m_PCControls.Player1Controls.MoveUp.canceled += ctx => Player1Stop(ref m_P1IsMovingUp);
        m_PCControls.Player1Controls.MoveDown.canceled += ctx => Player1Stop(ref m_P1IsMovingDown);

        m_PCControls.Player2Controls.MoveUp.started += ctx => Player2MoveUp();
        m_PCControls.Player2Controls.MoveDown.started += ctx => Player2MoveDown();
        m_PCControls.Player2Controls.MoveUp.canceled += ctx => Player2Stop(ref m_P2IsMovingUp);
        m_PCControls.Player2Controls.MoveDown.canceled += ctx => Player2Stop(ref m_P2IsMovingDown);
    }


    private void Player1MoveUp()
    {
        m_P1IsMovingUp = true;
        Player1Movement?.Invoke(BarMovementDirection.UP);
    }

    private void Player1MoveDown()
    {
        m_P1IsMovingDown = true;
        Player1Movement?.Invoke(BarMovementDirection.DOWN);
    }

    private void Player1Stop(ref bool isMoving)
    {
        isMoving = !isMoving;
        if ((m_P1IsMovingDown == false && m_P1IsMovingUp == false) || (m_P1IsMovingDown && m_P1IsMovingUp))
        {
            Player1Movement?.Invoke(BarMovementDirection.STOP);
        }
        else if (m_P1IsMovingUp == false && m_P1IsMovingDown)
        {
            Player1Movement?.Invoke(BarMovementDirection.DOWN);
        }
        else if (m_P1IsMovingUp && m_P1IsMovingDown == false)
        {
            Player1Movement?.Invoke(BarMovementDirection.UP);
        }
    }

    private void Player2MoveUp()
    {
        m_P2IsMovingUp = true;
        Player2Movement?.Invoke(BarMovementDirection.UP);
    }

    private void Player2MoveDown()
    {
        m_P2IsMovingDown = true;
        Player2Movement?.Invoke(BarMovementDirection.DOWN);
    }

    private void Player2Stop(ref bool isMoving)
    {
        isMoving = !isMoving;
        if ((m_P2IsMovingDown == false && m_P2IsMovingUp == false) || (m_P2IsMovingDown && m_P2IsMovingUp))
        {
            Player2Movement?.Invoke(BarMovementDirection.STOP);
        }
        else if (m_P2IsMovingUp == false && m_P2IsMovingDown)
        {
            Player2Movement?.Invoke(BarMovementDirection.DOWN);
        }
        else if (m_P2IsMovingUp && m_P2IsMovingDown == false)
        {
            Player2Movement?.Invoke(BarMovementDirection.UP);
        }

    }

    public void Enable()
    {
        m_PCControls.Enable();
        Debug.Log("Enabling Controls");
    }

    public void Disable()
    {
        m_PCControls.Disable();
        Debug.Log("Disabling Controls");    
    }

}


