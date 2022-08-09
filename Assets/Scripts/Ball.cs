using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private const int DirUpDown = 0;
    private const int DirLeftRight = 1;
    private const float DefaultSpeed = 5f;

    [SerializeField]
    private float m_fSpeed = 5f;
    private Direction m_Direction;
    private Vector3 m_MovementVector;


    // Start is called before the first frame update
    private void Start()
    {
        StartDirection();

    }

    private void StartDirection()
    {
        int index = Random.Range(0, 2);
        if (index == 0)
        {
            m_Direction = Direction.Left;
            transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(40, 110), 0));


        }
        else
        {
            m_Direction = Direction.Right;
            transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(-40, -110), 0));

        }
    }

    public void Restart(Vector3 startPOS)
    {
        transform.position = startPOS;
        StartCoroutine(Pause(1.5f));
        StartDirection();

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

    private void CalculateAngle(in Collider collider)
    {
        float angleTarget = collider.transform.rotation.eulerAngles.y;
        if (angleTarget == 180 || angleTarget == 0)
        {
            //2*alpha-beta
            float angle = gameObject.transform.rotation.eulerAngles.y;
            angle = 90 * 2 - angle;
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
        else if (angleTarget == 270 || angleTarget == 90)
        {
            SwitchDirection();
            PlayerAiming(collider);
        }
        Boost();
    }

    private void SwitchDirection()
    {
        m_Direction = m_Direction == Direction.Left ? Direction.Right : Direction.Left;
    }

    private void PlayerAiming(in Collider collider)
    {
        Vector3 direction = transform.position - collider.transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
        AngleSmoothing();
    }

    private void AngleSmoothing()
    {
        float angle = transform.rotation.eulerAngles.y;

        switch (m_Direction)
        {
            case Direction.Left:
                if (angle < 40)
                {
                    angle = 40f;
                }
                if (angle > 160)
                {
                    angle = 160f;
                }
                break;
            case Direction.Right:
                if (angle < 220)
                {
                    angle = 220f;
                }
                if (angle > 320)
                {
                    angle = 320f;
                }
                break;
        }
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
    }

    private void Boost()
    {
        m_fSpeed *= 1.05f;
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Coll with " + other.name);

        CalculateAngle(other);
    }
    public enum Direction
    {
        Left,
        Right
    }
}
