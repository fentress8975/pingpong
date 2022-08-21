using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private Animator m_Animator;
    [SerializeField]
    private Button m_SwapSettingsButton;

    private State m_State = State.active;

    private enum State
    {
        active,
        inTransition
    }

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_SwapSettingsButton.onClick.AddListener(TransitSettings);
    }

    private void TransitSettings()
    {
        switch (m_State)
        {
            case State.active:
                m_Animator.SetTrigger("SetTransition");
                m_State = State.inTransition;
                StartCoroutine(WaitForTransition());
                break;
            case State.inTransition:
                break;
        }
        
    }

    private IEnumerator WaitForTransition()
    {
        yield return new WaitForSeconds(m_Animator.GetCurrentAnimatorStateInfo(0).length);
        m_State = State.active;
    }

    private void OnEnable()
    {
        m_Animator.SetTrigger("SetActive");
    }

    private void OnDisable()
    {
        m_Animator.Play("Base Layer.Start", 0, 10);
    }
}
