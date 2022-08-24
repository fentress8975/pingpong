using TMPro;
using UnityEngine;

namespace UI
{
    public class NameChangerHint : MonoBehaviour
    {
        private TextMeshProUGUI m_HintText;
        [SerializeField]
        private TMP_InputField m_InputField;

        private void Start()
        {
            m_HintText = GetComponent<TextMeshProUGUI>();
            m_InputField.onSelect.AddListener(CloseHint);
        }

        private void CloseHint(string none)
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            m_InputField.onSelect.RemoveListener(CloseHint);
        }
    }
}