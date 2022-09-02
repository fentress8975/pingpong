using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]

public class AISensor : MonoBehaviour
{
    public UnityEvent<Vector3> OnNewBallPosition = new OnNewBallPosition();

    private bool isInit = false;

    public void Init()
    {
        if (isInit == false)
        {
            isInit = true;
        }
        else
        {
        Debug.Log("AI Sensor already init");
        }
    }

    public void SetNewBallPos(GameObject sender, Vector3 pos)
    {
        if (isBall(sender))
        {
            OnNewBallPosition?.Invoke(pos);
        }
        else
        {
            Debug.Log("Sensor Detect smt, but this not a ball");
        }

    }

    private bool isBall(GameObject target)
    {
        if (target.TryGetComponent<Ball>(out Ball ball))
        {
            return true;
        }
        return false;
    }
}

public class OnNewBallPosition : UnityEvent<Vector3>
{
    public OnNewBallPosition() : base() { }
}
