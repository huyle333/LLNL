using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using HoloToolkit.Examples.SharingWithUNET;
using HoloToolkit.Unity.InputModule;

/// <summary>
/// ONE DIMENSIONAL GRAPHS. catlikecoding.com
/// </summary>
public class HuyGrapher1 : NetworkBehaviour {

    /// <summary>
    /// The transform of the shared world anchor.
    /// </summary>
    private Transform sharedWorldAnchorTransform;

    /// <summary>
    /// The position relative to the shared world anchor.
    /// </summary>
    [SyncVar(hook = "OnChangeLocalPosition")]
    private Vector3 localPosition;

    /// <summary>
    /// The rotation relative to the shared world anchor.
    /// </summary>
    [SyncVar(hook = "OnChangeLocalRotation")]
    private Quaternion localRotation;

    /// <summary>
    /// Sets the localPosition and localRotation on clients.
    /// </summary>
    /// <param name="postion">the localPosition to set</param>
    /// <param name="rotation">the localRotation to set</param>
    [Command]
    public void CmdTransform(Vector3 postion, Quaternion rotation)
    {
        if (!isLocalPlayer)
        {
            localPosition = postion;
            localRotation = rotation;
        }
    }

    [ClientRpc]
    public void RpcPosition(Vector3 postion)
    {
        transform.localPosition = postion;
    }

    [ClientRpc]
    public void RpcRotation(Quaternion rotation)
    {
        transform.localRotation = rotation;
    }

    private void Start()
    {
        if (SharedCollection.Instance == null)
        {
            Debug.LogError("This script required a SharedCollection script attached to a gameobject in the scene");
            Destroy(this);
            return;
        }

        sharedWorldAnchorTransform = SharedCollection.Instance.gameObject.transform;
        transform.SetParent(sharedWorldAnchorTransform);

        if (isServer)
        {
            localPosition = transform.localPosition;
            localRotation = transform.localRotation;
        }
    }

    public enum FunctionOption
    {
        Linear,
        Exponential,
        Parabola,
        Sine
    }

    private delegate float FunctionDelegate(float x);
    private static FunctionDelegate[] functionDelegates = {
        Linear,
        Exponential,
        Parabola,
        Sine
    };

    public FunctionOption function;

    [Range(10, 100)]
    public int resolution = 10;

    private int currentResolution;
    private ParticleSystem.Particle[] points;

    private void CreatePoints()
    {
        currentResolution = resolution;
        points = new ParticleSystem.Particle[resolution];
        float increment = 1f / (resolution - 1);
        for (int i = 0; i < resolution; i++)
        {
            float x = i * increment;
            points[i].position = new Vector3(x, 0f, 0f);
            points[i].color = new Color(x, 0f, 0f);
            points[i].size = 0.1f;
        }
    }

    void Update()
    {
        if (currentResolution != resolution || points == null)
        {
            CreatePoints();
        }
        FunctionDelegate f = functionDelegates[(int)function];
        for (int i = 0; i < resolution; i++)
        {
            Vector3 p = points[i].position;
            p.y = f(p.x);
            points[i].position = p;
            Color c = points[i].color;
            c.g = p.y;
            points[i].color = c;
        }
        gameObject.GetComponent<ParticleSystem>().SetParticles(points, points.Length);
    }

    public void NetworkTransformUpdate(ManipulationEventData eventData)
    {
        // Depending on if you are host or client, either setting the SyncVar (client) 
        // or calling the Cmd (host) will update the other users in the session.
        // So we have to do both.
        /*
        localPosition = transform.localPosition;
        localRotation = transform.localRotation;
        CmdTransform(localPosition, localRotation);
        */
        if (!isServer)
        {
            return;
        }

        this.transform.position += eventData.CumulativeDelta;
        localPosition = transform.localPosition;
        localRotation = transform.localRotation;

        RpcPosition(this.transform.localPosition);
        RpcRotation(this.transform.localRotation);
    }

    void OnChangeLocalPosition (Vector3 newLocalPosition)
    {
    }

    void OnChangeLocalRotation (Quaternion newLocalRotation)
    {
    }

    private static float Linear(float x)
    {
        return x;
    }

    private static float Exponential(float x)
    {
        return x * x;
    }

    private static float Parabola(float x)
    {
        x = 2f * x - 1f;
        return x * x;
    }

    private static float Sine(float x)
    {
        return 0.5f + 0.5f * Mathf.Sin(2 * Mathf.PI * x + Time.timeSinceLevelLoad);
    }
}