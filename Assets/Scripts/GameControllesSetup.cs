using UnityEngine;
using UnityEngine.InputSystem;

public class GameControllesSetup : MonoBehaviour
{

    public void SpawnHotSeatPlayers(out PlayerControls Player1, out PlayerControls Player2)
    {
        GameObject go = Instantiate(ControlsManager.Instance.HotSeatControls);
        HotSeatControls controls = go.GetComponent<HotSeatControls>();
        Player1 = controls.Player1;
        Player2 = controls.Player2;
    }

    public void SpawnPlayer(out PlayerControls Player1, out AIController Player2)
    {
        GameObject player1 = Instantiate(ControlsManager.Instance.PlayerControls);
        GameObject ai = Instantiate(ControlsManager.Instance.PlayerControls);
        Player1 =  player1.GetComponent<PlayerControls>();

        Destroy(ai.GetComponent<PlayerControls>());
        Destroy(ai.GetComponent<PlayerInput>());
        Player2 = ai.AddComponent<AIController>();
    }
    
}
