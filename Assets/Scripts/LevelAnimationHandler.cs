using UnityEngine;
using UnityEngine.Events;

public class LevelAnimationHandler : MonoBehaviour
{
    public UnityEvent onReady = new();
    private ArenaAnimation m_Animations;


    private void LoadArenaAnimation()
    {
        m_Animations = FindObjectOfType<ArenaAnimation>();
    }

    public void StartArena(Transform player1, Transform player2, Ball ball)
    {
        LoadArenaAnimation();
        if (m_Animations == null)
        {
            StartGame();
        }
        else
        {
            m_Animations.onAnimationReady.AddListener(StartGame);
            m_Animations.StartAnimations(player1, player2);
        }

    }

    private void StartGame()
    {
        Debug.Log("StartGame");
        onReady.Invoke();
    }
}
