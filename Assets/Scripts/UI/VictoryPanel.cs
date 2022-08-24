using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


namespace UI
{
    public class VictoryPanel : MonoBehaviour
    {
        public UnityEvent OnRematch;
        public UnityEvent OnSwitchToMainMenu;


        [SerializeField]
        private TextMeshProUGUI m_VictoryText;
        [SerializeField]
        private Button m_BackToMenuButton;
        [SerializeField]
        private Button m_RematchButton;

        private void Start()
        {
            m_BackToMenuButton.onClick.AddListener(SwitchSceneToMenu);
            m_RematchButton.onClick.AddListener(RematchGame);
        }

        public void ShowVictoryScreen(string player)
        {
            m_VictoryText.SetText("Congrats " + player);
        }

        private void SwitchSceneToMenu()
        {
            Debug.Log(gameObject.name + " Go to mainmenu");
            OnSwitchToMainMenu?.Invoke();
        }

        private void RematchGame()
        {
            Debug.Log(gameObject.name + " Rematch game");
            OnRematch?.Invoke();
        }
    }
}
