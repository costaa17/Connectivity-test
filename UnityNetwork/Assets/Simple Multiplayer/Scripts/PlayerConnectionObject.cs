using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnectionObject : NetworkBehaviour
{

    public GameObject PlayerUnitPrefab;

    // SyncVar are variables where if their value changes on the server,
    // then all clients are automatically informed of the new value.
    [SyncVar]
    public string PlayerName = "Default";

    private GameObject PlayerUnit;

    void Start()
    {
        // Is this my own local object
        if (!isLocalPlayer)
        {
            // this objecct belong to other player
            return;
        }

        // Spawn an object to the world
        CmdSpawnMyUnit();
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("number of connections: " + Network.connections.Length);
    }


    // COMMANDS
    // Commande are specials functions that ONLY get executed on the server
    [Command]
    void CmdSpawnMyUnit()
    {
        Debug.Log("PlayerObject: I am alive !!!");
        PlayerUnit = Instantiate(PlayerUnitPrefab);

        PlayerUnit.GetComponent<PlayerUnit>().SetConnectionObject(this);
        NetworkServer.SpawnWithClientAuthority(PlayerUnit, connectionToClient);
        CmdChangePlayerName(PlayerName);
    }


    [Command]
    void CmdChangePlayerName(string name)
    {
        PlayerName = name + Random.Range(1,100);
        PlayerUnit.transform.Find("name").GetComponent<TextMesh>().text = PlayerName;
        Debug.Log("name changed");
    }


    ///////////////RPC
    // RPCs are special functions that ONLY get exrcuted on the clients

    //[ClientRpc]
    //void RpcChangePlayerName(string name)
    //{
    //    PlayerName = name;
    //}
}
