using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField]
        private Slider m_PointsToWinSlider;
        [SerializeField]
        private Slider m_BallBoosterSlider;


        public GameSettings GetGameSettings(int difficulty = 0)
        {
            return new GameSettings(m_BallBoosterSlider.value, m_PointsToWinSlider.value, difficulty);

        }
    }
}