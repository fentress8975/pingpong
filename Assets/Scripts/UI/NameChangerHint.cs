using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class NameChangerHint : MonoBehaviour
{
    private TextMeshProUGUI m_HintText;
    [SerializeField]
    private TMP_InputField m_InputField;


    void Start()
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
