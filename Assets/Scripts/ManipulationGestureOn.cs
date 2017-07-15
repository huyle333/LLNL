using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ManipulationGestureOn : MonoBehaviour, IManipulationHandler
{
    public void OnManipulationStarted(ManipulationEventData eventData)
    {
    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        // Do a raycast into the world from your head direction.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        // Use the manipulation gesture to move the object.
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo, Mathf.Infinity))
        {
            if (hitInfo.transform.name == "Graph1(Clone)")
            {
                HuyGrapher1 graph1Script = hitInfo.transform.gameObject.GetComponent<HuyGrapher1>();
                graph1Script.NetworkTransformUpdate(eventData);
            } else
            {
                hitInfo.transform.position += eventData.CumulativeDelta;
            }
        }
    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
    }

    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
    }
}
