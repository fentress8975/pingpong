using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class ArenaAnimation : MonoBehaviour
{
    [SerializeField]
    private float BarYPos;
    public UnityEvent onAnimationReady = new();

    private Transform m_Player1;
    private Transform m_Player2;

    [SerializeField]
    private Animator m_Lights;
    [SerializeField]
    private Animator m_Objects;

    public void StartAnimations(Transform player1, Transform player2)
    {
        m_Player1 = player1;
        m_Player2 = player2;
        LightsAnimation();
        ObjectsAnimation();
    }


    private void LightsAnimation()
    {
        m_Lights.SetTrigger("Start");
    }

    private void ObjectsAnimation()
    {
        m_Objects.SetTrigger("OpenDoors");
        StartCoroutine(WaitForDoorTransition());
    }

    private void MovePlayersUp()
    {
        m_Player1.position = Vector3.MoveTowards(m_Player1.position, new Vector3(m_Player1.position.x, BarYPos, m_Player1.position.z), 1f*Time.deltaTime);
        m_Player2.position = Vector3.MoveTowards(m_Player2.position, new Vector3(m_Player2.position.x, BarYPos, m_Player2.position.z), 1f * Time.deltaTime);
    }

    private IEnumerator WaitForDoorTransition()
    {
        yield return new WaitForSeconds(m_Objects.GetCurrentAnimatorStateInfo(0).length);
        MovePlayersUp();
        while(m_Player1.position.y != BarYPos || m_Player2.position.y != BarYPos)
        {
            MovePlayersUp();
            yield return null;
        }
        m_Objects.SetTrigger("CloseDoors");
        yield return new WaitForSeconds(m_Objects.GetCurrentAnimatorStateInfo(0).length);
        ArenaReady();
    }

    private void ArenaReady()
    {
        Debug.Log("LevelAnimations: DONE");
        onAnimationReady?.Invoke();

    }
}
