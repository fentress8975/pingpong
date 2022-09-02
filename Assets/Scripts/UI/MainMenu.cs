using GameSystems.Scene;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public UnityEvent<MultiplayerMode, string, GameSettings> OnStartGameEvent;
        public UnityEvent<string> OnUpdatePlayer1Name;
        public UnityEvent<string> OnUpdatePlayer2Name;

        public string Player1Name
        {
            get => m_Player1Name;
            private set
            {
                m_Player1Name = value;
            }
        }

        public string Player2Name
        {
            get => m_Player2Name;
            private set
            {
                m_Player2Name = value;
            }
        }

        [SerializeField]
        private Button m_StartHotSeat;
        [SerializeField]
        private Button m_StartMP;
        [SerializeField]
        private SinglePlayerButton m_StartSingle;
        [SerializeField]
        private TMP_Dropdown m_LevelChoose;

        [SerializeField]
        private Button m_OpenOptionsButton;
        [SerializeField]
        private Button m_CloseOptionsButton;

        [SerializeField]
        private TMP_InputField m_Player1InputField;
        [SerializeField]
        private TMP_InputField m_Player2InputField;
        [SerializeField]
        private string m_Player1Name;
        [SerializeField]
        private string m_Player2Name;

        [SerializeField]
        private Button m_CloseAppButton;
        [SerializeField]
        private GameObject m_OptionsMenu;
        private OptionsMenu m_Options;
        [SerializeField]
        private GameObject m_FaceMenu;


        private void SubcribeButtons()
        {
            m_StartSingle.OnClickDifficulty.AddListener(StartSingleGame);
            m_StartHotSeat.onClick.AddListener(StartHotSeatGame);
            m_StartMP.onClick.AddListener(StartOnlineGame);
            m_OpenOptionsButton.onClick.AddListener(OpenOptions);
            m_CloseOptionsButton.onClick.AddListener(OpenOptions);
            m_CloseAppButton.onClick.AddListener(CloseApp);
        }

        private void Start()
        {
            m_OptionsMenu.SetActive(false);
            m_Options = m_OptionsMenu.GetComponent<OptionsMenu>();

            SubcribeButtons();

            m_LevelChoose.ClearOptions();
            m_LevelChoose.AddOptions(SceneChanger.Instance.GetSceneList());
            m_Player1InputField.onValueChanged.AddListener(SetPlayer1Name);
            m_Player2InputField.onValueChanged.AddListener(SetPlayer2Name);

        }

        private void StartSingleGame(AIDificulty difficulty)
        {
            Debug.Log("Starting SP Game. Difficulty = " + difficulty + " " + (int)difficulty);
            
            OnStartGameEvent?.Invoke(MultiplayerMode.Single, m_LevelChoose.captionText.text, m_Options.GetGameSettings((int)difficulty));
        }

        private void StartHotSeatGame()
        {
            Debug.Log("Starting HotSeat Game");
            OnStartGameEvent?.Invoke(MultiplayerMode.HOTSEAT, m_LevelChoose.captionText.text, m_Options.GetGameSettings());
        }

        private void StartOnlineGame()
        {
            OnStartGameEvent?.Invoke(MultiplayerMode.Online, m_LevelChoose.itemText.text, m_Options.GetGameSettings());
        }

        private void OpenOptions()
        {
            if (m_OptionsMenu.gameObject.activeInHierarchy)
            {
                m_FaceMenu.SetActive(true);
                m_OptionsMenu.SetActive(false);
            }
            else
            {
                m_FaceMenu.SetActive(false);
                m_OptionsMenu.SetActive(true);
            }
        }

        public void UpdateNames(string Player1, string Player2)
        {
            m_Player1InputField.text = Player1;
            m_Player2InputField.text = Player2;
        }

        public void SetPlayer1Name(string name)
        {
            m_Player1Name = name;
            OnUpdatePlayer1Name?.Invoke(m_Player1Name);
        }

        public void SetPlayer2Name(string name)
        {
            m_Player2Name = name;
            OnUpdatePlayer2Name?.Invoke(m_Player2Name);
        }

        private void CloseApp()
        {
#if UNITY_EDITOR

            EditorApplication.ExitPlaymode();

#endif

#if UNITY_STANDALONE

            Application.Quit();

#endif
        }

        private void OnEnable()
        {
            Debug.Log("Load player names");
            string player1Name = PlayerPrefs.GetString("Player1Name");
            if (player1Name == null || player1Name == "")
            {
                m_Player1Name = "Player1";
            }
            else
            {
                m_Player1Name = player1Name;
            }
            string player2Name = PlayerPrefs.GetString("Player2Name");
            if (player2Name == null || player2Name == "")
            {
                m_Player2Name = "Player2";
            }
            else
            {
                m_Player2Name = player2Name;
            }
            UpdateNames(m_Player1Name, m_Player2Name);
        }

        private void OnDisable()
        {
            Debug.Log("Save player names");
            string player1Name = m_Player1Name;
            PlayerPrefs.SetString("Player1Name", player1Name);
            string player2Name = m_Player2Name;
            PlayerPrefs.SetString("Player2Name", player2Name);
        }
    }

    public enum MultiplayerMode
    {
        Single,
        HOTSEAT,
        Online
    }
}