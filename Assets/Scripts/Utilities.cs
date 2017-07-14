using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour {
    
    public GameObject localPlayer { set; get; }
    private NavigationGestureOn navigationGestureScript;
    private bool localPlayerFound = false;
    private bool navigationGestureInitialDelay = false;

    private void Update()
    {
        // Initially, navigation gesture will be off.
        if (navigationGestureScript == null)
        {
            navigationGestureScript = localPlayer.GetComponent<NavigationGestureOn>();
        } else
        {
            localPlayerFound = true;
        }

        if (localPlayerFound && !navigationGestureInitialDelay)
        {
            navigationGestureScript.enabled = false;
            Debug.Log("HELLO WORLD");
            navigationGestureInitialDelay = true;
        }
    }

    public void TurnOffGesture()
    {
        Debug.Log("TURN OFF GESTURE");
        ManipulationGestureOn manipulationGestureScript = localPlayer.GetComponent<ManipulationGestureOn>();
        NavigationGestureOn navigationGestureScript = localPlayer.GetComponent<NavigationGestureOn>();

        if (!navigationGestureScript.enabled)
        {
            manipulationGestureScript.enabled = false;
            navigationGestureScript.enabled = true;
            Debug.Log("SWITCH");
        }
        else if (!manipulationGestureScript.enabled)
        {
            navigationGestureScript.enabled = false;
            manipulationGestureScript.enabled = true;
            Debug.Log("SWITCH BACK");
        }
    }
}
