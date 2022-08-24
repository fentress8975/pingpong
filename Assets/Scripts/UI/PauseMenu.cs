using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        public UnityEvent SwitchToMenu;

        [SerializeField]
        private Button m_ExitGame;
        [SerializeField]
        private Button m_BackToMenu;

        private void Awake()
        {
            m_ExitGame.onClick.AddListener(ExitGame);
            m_BackToMenu.onClick.AddListener(SwitchSceneToMenu);

        }

        public void OpenPauseMenu()
        {
            EnablePauseMenu();
        }

        public void ClosePauseMenu()
        {
            DisablePauseMenu();
        }

        private void EnablePauseMenu()
        {
            gameObject.SetActive(true);
            Debug.Log("PauseMenuOn");
        }

        private void DisablePauseMenu()
        {
            gameObject.SetActive(false);
            Debug.Log("PauseMenuOff");
        }

        private void SwitchSceneToMenu()
        {
            Debug.Log("Goint To MainMenu");
            SwitchToMenu?.Invoke();
        }

        private void ExitGame()
        {
#if UNITY_EDITOR

            EditorApplication.ExitPlaymode();

#endif

#if UNITY_STANDALONE

            Application.Quit();

#endif
        }
    }
}
