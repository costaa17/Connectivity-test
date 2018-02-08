using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnectionObject : NetworkBehaviour
{

    public GameObject PlayerUnitPrefab;

    // SyncVar are variables where if their value changes on the server,
    // then all clients are automatically informed of the new value.
    [SyncVar(hook ="OnPlayerNameChanged")]
    public string PlayerName = "Player";

    public GameObject PlayerUnit;

    void Start()
    {
        // Is this my own local object
        if (!isLocalPlayer)
        {
            // this object belong to other player
            return;
        }

        // Spawn an object to the world
        CmdSpawnMyUnit();
        //this.GetComponent<NetworkIdentity>().connectionToServer.playerControllers;
        //Debug.Log("id: " + this.gameObject.GetComponent<NetworkIdentity>().netId);
    }
    
    void Update()
    {
   
    }

    void OnPlayerNameChanged(string newName)
    {
        Debug.Log("OnPlayerNamechanged: Oldname: " + PlayerName + " . New name: " + newName);
        PlayerName = newName;
    }

    // COMMANDS
    // Commande are specials functions that ONLY get executed on the server
    [Command]
    void CmdSpawnMyUnit()
    {
        Debug.Log("PlayerObject: I am alive !!!");
        PlayerUnit = Instantiate(PlayerUnitPrefab);
        NetworkManager.singleton.GetComponent<GameObjectList>().gameObjectList.Add(PlayerUnit);
        CmdChangePlayerName(PlayerName);
        NetworkServer.SpawnWithClientAuthority(PlayerUnit, connectionToClient);

    }


    [Command]
    void CmdChangePlayerName(string name)
    {
        PlayerName = name + netId;
        RpcChangePlayerNameTag();
        Debug.Log("name changed");
    }


    ///////////////RPC
    // RPCs are special functions that ONLY get executed on the clients

    [ClientRpc]
    void RpcChangePlayerNameTag()
    {
        
    }
}
