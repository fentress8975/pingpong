using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Scroller : MonoBehaviour
    {
        [SerializeField]
        private RawImage m_Image;

        [SerializeField]
        private float m_SpeedX = 0.1f;
        [SerializeField]
        private float m_SpeedY = 0.1f;

        private void Start()
        {
            m_Image = GetComponent<RawImage>();
            StartCoroutine(MoveBG());
        }

        private IEnumerator MoveBG()
        {
            float x = 0;
            float y = 0;
            float h = m_Image.uvRect.height;
            float w = m_Image.uvRect.width;
            while (true)
            {
                x = Mathf.MoveTowards(x, 1, m_SpeedX * Time.deltaTime);
                y = Mathf.MoveTowards(y, 1, m_SpeedY * Time.deltaTime);
                m_Image.uvRect = new Rect(x, y, w, h);
                if (x == 1) { x = 0; }
                if (y == 1) { y = 0; }
                yield return null; //Я снова попался на это
            }

        }
    }
}