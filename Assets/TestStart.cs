using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStart : MonoBehaviour
{
    [SerializeField]
    Transform object1;
    [SerializeField]
    Transform object2;
    [SerializeField]
    ArenaAnimation m_animation;

    void Start()
    {
        m_animation.StartAnimations(object1, object2);
        m_animation.onAnimationReady.AddListener(AnimReady);
    }

    private void AnimReady()
    {
        Debug.Log("OK");
    }
}
