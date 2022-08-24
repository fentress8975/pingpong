using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OptionsAnimatorHandler : MonoBehaviour
{
    [SerializeField]
    private Animator m_OptionsAnimator;
    [SerializeField]
    private Button m_SwapSettingsButton;
    [SerializeField]
    private Button m_GoBackButton;
    [SerializeField]
    private Button m_OptionsButton;

    [SerializeField]
    private State m_State = State.PlayerSetting;

    private void Start()
    {
        m_OptionsButton.onClick.AddListener(FirstTimeOpen);
        m_SwapSettingsButton.onClick.AddListener(TryTransitSettigns);
    }

    private enum State
    {
        PlayerSetting,
        inTransition,
        GameSettings
    }

    public void FirstTimeOpen()
    {
        m_OptionsAnimator.SetTrigger("ShowPlayerSettings");
        m_OptionsButton.onClick.RemoveListener(FirstTimeOpen);
    }

    private void TryTransitSettigns()
    {
        switch (m_State)
        {
            case State.PlayerSetting:
                TransitSettings();
                break;
            case State.GameSettings:
                TransitSettings();
                break;
            case State.inTransition:
                break;
        }
    }

    private void TransitSettings()
    {
        m_OptionsAnimator.SetTrigger("SetTransition");
        //if (m_State == State.GameSettings)
        //{
        //    m_OptionsAnimator.SetTrigger("Hide");
        //    m_OptionsAnimator.SetTrigger("ShowPlayerSettings");
        //}
        //else
        //{
        //    m_OptionsAnimator.SetTrigger("SetTransition");
        //    m_OptionsAnimator.SetTrigger("SetActive");
        //}
        m_GoBackButton.interactable = false;
        var nextState = m_State == State.PlayerSetting ? State.GameSettings : State.PlayerSetting;
        m_State = State.inTransition;
        StartCoroutine(WaitForTransition(nextState));

    }

    private IEnumerator WaitForTransition(State nextState)
    {
        yield return new WaitForSeconds(m_OptionsAnimator.GetCurrentAnimatorStateInfo(0).length);
        m_State = nextState;
        m_GoBackButton.interactable = true;
    }

}
