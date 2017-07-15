using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Examples.SharingWithUNET;

public class Utilities : MonoBehaviour {
    
    // Triggered by PlayerController to store the local player gameobject.
    public GameObject localPlayer { set; get; }

    // The Navigation script that we are going to shut off after 18 seconds because we only want either Manipulation or Navigation.
    // Only one of them because the gestures are basically the same, so they conflict with other.
    // The difference is that the manipulation gesture can move things on the Z axis, while navigation gesture is x and y.
    private NavigationGestureOn navigationGestureScript;

    private float waitTime = 18f;
    private float timeToCheck;

    // 1. Find the local player after 18 seconds.
    private bool localPlayerFound = false;
    // 2. Disable the navigation gesture script because we only want the manipulation script at the moment on.
    private bool initialNavigationGestureDisableDone = false;

    private void Start()
    {
        // Wait 7 seconds before trying to get the local player.
        timeToCheck = Time.time + waitTime;
    }

    private void Update()
    {
        // The initial state is manipulation. Manipulation moves stuff. Navigation rotates stuff.
        if (Time.time > timeToCheck)
        {
            if (navigationGestureScript == null)
            {
                // localPlayer should have been set by 18 seconds by the PlayerController script.
                // so we set the navigationGestureScript.
                navigationGestureScript = localPlayer.GetComponent<NavigationGestureOn>();
            }
            else
            {
                // The next frame after navigationGestureScript is set, say that localPlayerFound is true.
                localPlayerFound = true;
            }

            if (localPlayerFound && !initialNavigationGestureDisableDone)
            {
                navigationGestureScript.enabled = false;
                // initialNavigationGestureDisableDone. We disabled the navigationGestureScript, so now we say hey it's done.
                // Now, when the update function happens again, this conditional will not succeed.
                // We just want to disable the navigation script once, so that the manipulation script can do its business alone.
                initialNavigationGestureDisableDone = true;
            }
        }
    }

    /// <summary>
    /// Air tap will switch from Manipulation to Navigation and vice versa.
    /// </summary>
    public void SwitchGestures()
    {
        ManipulationGestureOn manipulationGestureScript = localPlayer.GetComponent<ManipulationGestureOn>();
        NavigationGestureOn navigationGestureScript = localPlayer.GetComponent<NavigationGestureOn>();

        if (!navigationGestureScript.enabled)
        {
            manipulationGestureScript.enabled = false;
            navigationGestureScript.enabled = true;
            Debug.Log("SWITCH TO NAVIGATION.");
        }
        else if (!manipulationGestureScript.enabled)
        {
            navigationGestureScript.enabled = false;
            manipulationGestureScript.enabled = true;
            Debug.Log("SWITCH TO MANIPULATION");
        }
    }

    /// <summary>
    /// Graph voice commands. One Dimensional, Two Dimensional, or Three Dimensional.
    /// </summary>
    /// <param name="graphName"></param>
    public void GraphSetActive(string graphName)
    {
        SharedCollection.Instance.gameObject.transform.Find("Graph1(Clone)").gameObject.SetActive(false);
        SharedCollection.Instance.gameObject.transform.Find("Graph2").gameObject.SetActive(false);
        SharedCollection.Instance.gameObject.transform.Find("Graph3").gameObject.SetActive(false);

        SharedCollection.Instance.gameObject.transform.Find(graphName).gameObject.SetActive(true);
    }

    /// <summary>
    /// Graph type voice commands. Linear, Exponential, Parabola, Sine, Ripple.
    /// </summary>
    /// <param name="graphType"></param>
    public void GraphTypeSetActive(string graphType)
    {
        GameObject graph1 = SharedCollection.Instance.gameObject.transform.Find("Graph1(Clone)").gameObject;
        GameObject graph2 = SharedCollection.Instance.gameObject.transform.Find("Graph2").gameObject;
        GameObject graph3 = SharedCollection.Instance.gameObject.transform.Find("Graph3").gameObject;

        if (graph1.activeSelf)
        {
            HuyGrapher1 grapher1 = graph1.GetComponent<HuyGrapher1>();
            if (graphType == "Linear")
            {
                grapher1.function = HuyGrapher1.FunctionOption.Linear;
            }
            else if (graphType == "Exponential")
            {
                grapher1.function = HuyGrapher1.FunctionOption.Exponential;
            }
            else if (graphType == "Parabola")
            {
                grapher1.function = HuyGrapher1.FunctionOption.Parabola;
            }
            else if (graphType == "Sine")
            {
                grapher1.function = HuyGrapher1.FunctionOption.Sine;
            }
        }

        if (graph2.activeSelf)
        {
            HuyGrapher2 grapher2 = graph2.GetComponent<HuyGrapher2>();
            if (graphType == "Linear")
            {
                grapher2.function = HuyGrapher2.FunctionOption.Linear;
            }
            else if (graphType == "Exponential")
            {
                grapher2.function = HuyGrapher2.FunctionOption.Exponential;
            }
            else if (graphType == "Parabola")
            {
                grapher2.function = HuyGrapher2.FunctionOption.Parabola;
            }
            else if (graphType == "Sine")
            {
                grapher2.function = HuyGrapher2.FunctionOption.Sine;
            }
            else if (graphType == "Ripple")
            {
                grapher2.function = HuyGrapher2.FunctionOption.Ripple;
            }
        }

        if (graph3.activeSelf)
        {
            HuyGrapher3 grapher3 = graph3.GetComponent<HuyGrapher3>();
            if (graphType == "Linear")
            {
                grapher3.function = HuyGrapher3.FunctionOption.Linear;
            }
            else if (graphType == "Exponential")
            {
                grapher3.function = HuyGrapher3.FunctionOption.Exponential;
            }
            else if (graphType == "Parabola")
            {
                grapher3.function = HuyGrapher3.FunctionOption.Parabola;
            }
            else if (graphType == "Sine")
            {
                grapher3.function = HuyGrapher3.FunctionOption.Sine;
            }
            else if (graphType == "Ripple")
            {
                grapher3.function = HuyGrapher3.FunctionOption.Ripple;
            }
        }
    }
}
