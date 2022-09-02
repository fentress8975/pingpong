using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private const float DefaultSpeed = 5f;
    private const float MaxSpeed = 40f;

    [SerializeField]
    private int m_RayStep = 1;
    [SerializeField]
    private float m_BallBooster = 1.05f;
    [SerializeField]
    private float m_fSpeed = 5f;
    [SerializeField]
    private Direction m_Direction;
    [SerializeField]
    private Vector3 m_StartPos;
    private enum Direction
    {
        Left,
        Right
    }
    private Vector3 m_MovementVector;

    public void Init(int difficulty)
    {
        m_RayStep = difficulty;
        Restart();
    }

    public void SetStartPos(Vector3 pos)
    {
        m_StartPos = pos;
    }

    public void ChangeBoost(float boost)
    {
        if (boost < 1) { m_BallBooster = boost + 1; }
    }

    public void Restart(Vector3 startPOS)
    {
        transform.position = startPOS;
        StartCoroutine(Pause(1.5f));
        StartDirection();
        CastRays(m_RayStep);
    }

    public void Restart()
    {
        transform.position = m_StartPos;
        StartCoroutine(Pause(1.5f));
        StartDirection();
        CastRays(m_RayStep);
    }

    public void StopBall()
    {
        m_fSpeed = 0;
    }

    public void StopBall(float time)
    {
        Pause(time);
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
        AngleSmoothing(ref angle);
        m_Direction = CheckDirection();
        transform.rotation = angle;
        Boost();
        CastRays(m_RayStep);
    }

    private Vector3 GetNormal(in Collider collider)
    {
        Ray ray = new(transform.position, transform.forward);
        if (collider.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
        {
            return hitInfo.normal;
        }
        else
        {
            Vector3 hitPos = collider.ClosestPointOnBounds(transform.position);
            ray.origin = hitPos - transform.forward;
            if (collider.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                return hitInfo.normal;
            }
        }
        Debug.Log("GetNormal Error" + hitInfo.normal);
        Restart(new Vector3(-10, 0, -8));
        return Vector3.zero;
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
                if (angle == 90)
                {
                    angle += Random.Range(-20f, 20f);
                }
                else
                {
                    if (angle < 40)
                    {
                        angle = 40f;
                    }
                    if (angle > 160)
                    {
                        angle = 160f;
                    }
                }
                break;
            case Direction.Right:
                if (angle == 90)
                {
                    angle += Random.Range(-20f, 20f);
                }
                else
                {
                    if (angle < 220)
                    {
                        angle = 220f;
                    }
                    if (angle > 320)
                    {
                        angle = 320f;
                    }
                }
                break;
        }
        angleQ = Quaternion.Euler(new Vector3(angleQ.eulerAngles.x, angle, angleQ.eulerAngles.z));
    }

    private void Boost()
    {
        if (m_fSpeed < MaxSpeed)
        {
            m_fSpeed *= m_BallBooster;
        }
        if (m_fSpeed > MaxSpeed)
        {
            m_fSpeed = MaxSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 lel = transform.position;
        if (other.gameObject.TryGetComponent<AISensor>(out AISensor controls))
        {
            return;
        }
        if (isAI(in other) || isPlayer(in other))
        {
            CalculateAngle(other, true);

        }
        else
        {
            CalculateAngle(other, false);
        }
    }


    private bool isPlayer(in Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerControls>(out PlayerControls controls))
        {
            return true;
        }
        return false;
    }

    private bool isAI(in Collider other)
    {
        if (other.gameObject.TryGetComponent<AIController>(out AIController controls))
        {
            return true;
        }
        return false;
    }

    private void CastRays(int count)
    {
        //If we play HotSeat or Mp, skip raycast
        if(m_RayStep == 0)
        {
            return;
        }
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;
        for (int i = 0; i < count; i++)
        {
            if (CastRayToSensor(origin, direction, out origin, out direction))
            {
                return;
            }
        }
    }

    private bool CastRayToSensor(Vector3 origin, Vector3 direction, out Vector3 hitPos, out Vector3 directionToHit)
    {
        Ray ray = new(origin, direction);
        RaycastHit raycastHit;
        hitPos = Vector3.zero;
        directionToHit = Vector3.zero;
        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
        {
            if (raycastHit.collider.gameObject.TryGetComponent<AISensor>(out AISensor sensor))
            {
                Debug.DrawRay(origin, direction * 100, Color.red, 0.5f);
                sensor.SetNewBallPos(gameObject, raycastHit.point);
                return true;
            }
            else
            {
                Debug.DrawRay(origin, direction * 100, Color.blue, 0.5f);
                Vector3 dirNormal = raycastHit.normal;
                directionToHit = Vector3.Reflect(direction, dirNormal);
                hitPos = raycastHit.point;
                return false;
            }
        }
        else
        {
            return true;
        }
    }


    private void Start()
    {
        StopBall();
        
    }

    private void Update()
    {
        Move();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * 10);
    }
}
