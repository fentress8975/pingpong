using System.Collections;
using UnityEngine;


public class AIController : MonoBehaviour
{
    private float m_fSpeed = 6f;
    private AISensor m_Sensor;
    [SerializeField]
    private float m_TargetPosZ;

    private BarMovementDirection m_Direction;
    private enum BarState
    {
        Active,
        Stuned
    }

    private BarState m_State = BarState.Active;


    private void Start()
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/AI/Sensor"), transform);
        if(go.TryGetComponent<AISensor>(out m_Sensor))
        {
            m_Sensor.Init(gameObject);
            m_Sensor.OnNewBallPosition.AddListener(SetNewBallPos);
        }
        else
        {
            Debug.Log("AI Sensor not found");
        }
        
    }

    private void SetNewBallPos(Vector3 pos)
    {
        if (isNewPos(pos))
        {
            m_TargetPosZ = pos.z;
            SetDirection();
            MoveToPosition();
        }
    }

    private void SetDirection()
    {
        if (m_TargetPosZ > transform.position.z)
        {
            m_Direction = BarMovementDirection.UP;
        }
        else
        {
            m_Direction = BarMovementDirection.DOWN;
        }
    }

    private bool isNewPos(Vector3 pos)
    {
        if (m_TargetPosZ == pos.z) { return false; }
        return true;
    }

    private void MoveToPosition()
    {
        if (transform.position.z != m_TargetPosZ)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, m_TargetPosZ), m_fSpeed * Time.deltaTime);

        }
        else
        {
            //Debug.Log("AI stop Moving");
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
        Debug.Log($"{gameObject.name} Stunned");
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
        MoveToPosition();

    }

    private void Update()
    {
        switch (m_State)
        {
            case BarState.Active:
                MoveToPosition();
                break;
            case BarState.Stuned:
                break;
        }
    }
}
