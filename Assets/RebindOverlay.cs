using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RebindOverlay : MonoBehaviour
{
    public UnityEvent<bool> RemapingControls;


    private void OnDisable()
    {
        RemapingControls?.Invoke(false);
    }

    private void OnEnable()
    {
        RemapingControls?.Invoke(true);
    }
}
