using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayersScoreHandler : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_P1Text;
        [SerializeField]
        private TextMeshProUGUI m_P2Text;

        private string m_P1Name = "Player 1";
        private string m_P2Name = "Player 2";

        private int m_P1Score = 0;
        private int m_P2Score = 0;

        private void Start()
        {
            UpdateDisplay();
        }

        public void Init(string player1Name = "Player 1", string player2Name = "Player 2")
        {
            m_P1Name = player1Name;
            m_P2Name = player2Name;
            Debug.Log($"Player1 name = {player1Name}, Player2 name = {player2Name}");
        }

        public void ResetScore()
        {
            m_P1Score = 0;
            m_P2Score = 0;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            m_P1Text.text = $"{m_P1Name}: " + m_P1Score;
            m_P2Text.text = $"{m_P2Name}: " + m_P2Score;
        }

        public void UpdatePlayer1Score(int score)
        {
            m_P1Score = score;
            UpdateDisplay();
        }

        public void UpdatePlayer2Score(int score)
        {
            m_P2Score = score;
            UpdateDisplay();
        }

        public void UpdatePlayer1Name(string name)
        {
            m_P1Name = name;
        }

        public void UpdatePlayer2Name(string name)
        {
            m_P2Name = name;
        }
    }
}