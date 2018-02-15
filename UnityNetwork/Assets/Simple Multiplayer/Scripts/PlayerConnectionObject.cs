using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerConnectionObject : NetworkBehaviour
{

    public GameObject[] PlayerUnitPrefab;

    // SyncVar are variables where if their value changes on the server,
    // then all clients are automatically informed of the new value.
    [SyncVar(hook ="OnPlayerNameChanged")]
    public string PlayerName = "Player";

    public GameObject PlayerUnit;

    public enum PlayerType { Human = 0, Mosquito = 1};

    private PlayerType playerType = PlayerType.Human;
    void Start()
    {
        // Is this my own local object
        if (!isLocalPlayer)
        {
            // this object belong to other player
            return;
        }

        Dropdown dropDown = GameObject.Find("CharacterSelection").GetComponent<Dropdown>();
        int type = dropDown.value == 2 ? 2 - Random.Range(1,3) : dropDown.value;
        if(type == 0)
        {
            playerType = PlayerType.Human;
        }
        else
        {
            playerType = PlayerType.Mosquito;
        }

        // Spawn an object to the world
        CmdSpawnMyUnit(type);
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
    void CmdSpawnMyUnit(int type)
    {
        Debug.Log("PlayerObject: I am alive !!!");
        PlayerUnit = Instantiate(PlayerUnitPrefab[type]);
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
