using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlsRebinder : MonoBehaviour
{
    private 


    // Start is called before the first frame update
    void Start()
    {
        m_ActionLabel.text = m_Action.action.name;
        m_BindingText.text = m_Action.action.GetBindingDisplayString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void UpdateDisplay()
    {
        m_ActionLabel.text = m_Action.action.name;
        m_BindingText.text = m_Action.action.GetBindingDisplayString();
    }



    [Tooltip("Reference to action that is to be rebound from the UI.")]
    [SerializeField]
    private InputActionReference m_Action;


    [Tooltip("Text label that will receive the name of the action. Optional. Set to None to have the "
        + "rebind UI not show a label for the action.")]
    [SerializeField]
    private Text m_ActionLabel;

    [Tooltip("Text label that will receive the current, formatted binding string.")]
    [SerializeField]
    private Text m_BindingText;

    [Tooltip("Optional UI that will be shown while a rebind is in progress.")]
    [SerializeField]
    private GameObject m_RebindOverlay;

    [Tooltip("Optional text label that will be updated with prompt for user input.")]
    [SerializeField]
    private Text m_RebindText;
}
