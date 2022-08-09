using UnityEngine;


public class PingPongController : MonoBehaviour
{
    public const string tagPlayer1 = "Player1";
    public const string tagPlayer2 = "Player2";

    private GameObject m_Bar;
    [SerializeField]
    private float m_fSpeed = 5f;

    private string m_PlayerTag;

    private void Start()
    {
        m_Bar = gameObject;
        m_PlayerTag = gameObject.tag;
    }

    private void Move()
    {
        if (m_PlayerTag == tagPlayer1)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                m_Bar.transform.position += m_fSpeed * Time.deltaTime * Vector3.forward;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                m_Bar.transform.position += Vector3.back * m_fSpeed * Time.deltaTime;
            }
        }

        if (m_PlayerTag == tagPlayer2)
        {
            if (Input.GetKey(KeyCode.W))
            {
                m_Bar.transform.position += m_fSpeed * Time.deltaTime * Vector3.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                m_Bar.transform.position += Vector3.back * m_fSpeed * Time.deltaTime;
            }
        }
    }

    private void BorderCheck()
    {
        if(transform.position.z > 5)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 5);
        }
        if (transform.position.z < -5)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -5);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent<Border>(out Border border))
        {
            Debug.Log("border");
            BorderCheck();
        }
    }

    private void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {

    }
}
