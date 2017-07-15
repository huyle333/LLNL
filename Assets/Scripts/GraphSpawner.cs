using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GraphSpawner : NetworkBehaviour {

    public GameObject graphPrefab;

    public override void OnStartServer()
    {
        GameObject graph = (GameObject)Instantiate(graphPrefab);
        NetworkServer.Spawn(graph);
    }
}
