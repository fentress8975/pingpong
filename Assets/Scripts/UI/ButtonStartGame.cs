using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ButtonStartGame : MonoBehaviour
    {
        [SerializeField]
        private Button m_StartButton;
        [SerializeField]
        private Animator m_Animator;

        private enum OptionsState { disable, enable }
        private OptionsState m_State = OptionsState.disable;

        private List<Button> m_Buttons = new();

        private void Start()
        {
            m_StartButton.onClick.AddListener(TriggerOptions);

            foreach (var go in GetComponentsInChildren<Button>(true))
            {
                if (go == m_StartButton) continue;
                m_Buttons.Add(go);
            }
        }

        public void TriggerOptions()
        {
            switch (m_State)
            {
                case OptionsState.disable:
                    ShowOptions();
                    break;
                case OptionsState.enable:
                    StartCoroutine(HideOptions());
                    break;
            }
        }

        private void ShowOptions()
        {
            foreach (var go in m_Buttons)
            {
                go.gameObject.SetActive(true);
            }
            m_Animator.SetTrigger("SetActive");
            m_State = OptionsState.enable;
        }

        private IEnumerator HideOptions()
        {
            m_Animator.SetTrigger("SetActive");
            yield return new WaitForSeconds(m_Animator.GetCurrentAnimatorStateInfo(0).length);
            foreach (var go in GetComponentsInChildren<Button>(true))
            {
                if (go == m_StartButton) continue;
                go.gameObject.SetActive(false);
            }
            m_State = OptionsState.disable;
        }

        private void OnEnable()
        {
            switch (m_State)
            {
                case OptionsState.disable:
                    m_Animator.Play("Base Layer.Start", 0, 10);
                    break;
                case OptionsState.enable:
                    m_Animator.Play("Base Layer.ShowButtons", 0, 10);
                    break;
            }
        }
    }
}