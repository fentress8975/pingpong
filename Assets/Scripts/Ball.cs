using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private const int DirUpDown = 0;
    private const int DirLeftRight = 1;
    private const float DefaultSpeed = 5f;
    [SerializeField]
    private float m_BallBooster = 1.05f;
    [SerializeField]
    private float m_fSpeed = 5f;
    [SerializeField]
    private Direction m_Direction;
    private enum Direction
    {
        Left,
        Right
    }
    private Vector3 m_MovementVector;


    public void ChangeBoost(float boost)
    {
        if (boost < 1) { m_BallBooster = boost + 1; }
    }

    public void Restart(Vector3 startPOS)
    {
        transform.position = startPOS;
        StartCoroutine(Pause(1.5f));
        StartDirection();
    }

    public void StopBall()
    {
        m_fSpeed = 0;
    }


    private void StartDirection()
    {
        m_Direction = (Direction)Random.Range(0, 2);
        switch (m_Direction)
        {
            case Direction.Left:
                transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(-40, -110), 0));
                break;
            case Direction.Right:
                transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(40, 110), 0));
                break;
        }
    }

    private IEnumerator Pause(float time)
    {
        m_fSpeed = 0;
        yield return new WaitForSeconds(time);
        m_fSpeed = DefaultSpeed;
    }

    private void Move()
    {
        transform.position += m_fSpeed * Time.deltaTime * transform.forward;

    }

    private void CalculateAngle(in Collider collider, bool player)
    {
        Vector3 dirNormal = GetNormal(collider);
        Quaternion angle = Quaternion.LookRotation(Vector3.Reflect(transform.forward, dirNormal));
        if (player)
        {
            PlayerAiming(in collider, ref angle);
        }
        transform.rotation = angle;
        m_Direction = CheckDirection();
        Boost();
    }

    private Vector3 GetNormal(in Collider collider)
    {
        Ray ray = new(transform.position, transform.forward);
        collider.Raycast(ray, out RaycastHit hitInfo, 10f);
        return hitInfo.normal;
    }

    private Direction CheckDirection()
    {
        return (transform.rotation.eulerAngles.y > 0 && transform.rotation.eulerAngles.y <= 180) ? Direction.Right : Direction.Left;
    }

    private void PlayerAiming(in Collider collider, ref Quaternion angle)
    {
        Vector3 direction = transform.position - collider.transform.position;
        Quaternion angleCorrected = Quaternion.LookRotation(direction);
        AngleSmoothing(ref angleCorrected);
        angle = angleCorrected;
    }

    private void AngleSmoothing(ref Quaternion angleQ)
    {
        float angle = angleQ.eulerAngles.y;
        if (angle < 0) { angle += 360; }
        switch (m_Direction)
        {
            case Direction.Left:
                if (angle < 40)
                {
                    angle = 40f;
                    Debug.Log("New mirror angle = " + angle);
                }
                if (angle > 160)
                {
                    angle = 160f;
                    Debug.Log("New mirror angle = " + angle);
                }
                break;
            case Direction.Right:
                if (angle < 220)
                {
                    angle = 220f;
                    Debug.Log("New mirror angle = " + angle);
                }
                if (angle > 320)
                {
                    angle = 320f;
                    Debug.Log("New mirror angle = " + angle);
                }
                break;
        }
        angleQ = Quaternion.Euler(new Vector3(angleQ.eulerAngles.x, angle, angleQ.eulerAngles.z));
    }

    private void Boost()
    {
        m_fSpeed *= m_BallBooster;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Coll with " + other.name);

        CalculateAngle(other, isPlayer(in other));
    }

    private bool isPlayer(in Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerControls>(out PlayerControls controls))
        {
            Debug.Log("Collision with player!");
            return true;
        }
        return false;
    }

    private void Start()
    {
        StartDirection();

    }

    private void Update()
    {
        Move();
    }
}
