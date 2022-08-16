using UnityEngine;

public class HotSeatControllesSetup : MonoBehaviour
{

    public void SpawnPlayers(out PlayerControls Player1, out PlayerControls Player2)
    {
        GameObject go = Instantiate(ControlsManager.Instance.HotSeatControls);
        HotSeatControls controls = go.GetComponent<HotSeatControls>();
        Player1 = controls.Player1;
        Player2 = controls.Player2;
    }

}
