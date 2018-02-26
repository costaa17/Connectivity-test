﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Slap : PlayerAttack {

    private float timeElapsed;
    private float timeMax = .5f;
    private GameObject interactObject;

    void Start () {
		
	}
	
	void Update () {
        if (!this.transform.GetComponent<NetworkIdentity>().hasAuthority)
        {
            return;
        }

        if (interactObject != null)
        {
            timeElapsed += Time.deltaTime;
        }

        if(timeElapsed >= timeMax)
        {
            timeElapsed = 0;
            //CmdRemoveLocalPlayerAuthority(interactObject);
            interactObject = null;
        }
	}

    public override void Attack(GameObject other)
    {
        //Debug.Log("who: " + other.transform.parent.name);
        foreach (Transform child in other.transform.parent)
        {
            //Debug.Log("who?: " + child.name);
            if (child.CompareTag("Slapable"))
            {
                GameObject obj = child.parent.gameObject;
                NetworkIdentity ni = obj.GetComponent<NetworkIdentity>();
                Debug.Log("author: " + this.transform.Find("Name").GetComponent<TextMesh>().text);

                //This is bug due to object is given multiple authorities

                //Debug.Log("who pare?: " + obj.name);
                Debug.Log("is this ser: " + ni.isServer);
                Debug.Log("is this cli: " + ni.isClient);
                interactObject = obj;
                float force = 100;
                if (obj.CompareTag("Human"))
                {
                    //if (ni.isClient && !ni.isServer) CmdPush(ni.netId, force);
                    //else if (ni.isServer) RpcPush(ni.netId, force);
                    CmdPush(ni.netId, force);
                }
                else if (obj.CompareTag("Mosquito"))
                {
                    force = 100;
                    //if (ni.isClient && !ni.isServer) CmdPush(ni.netId, force);
                    //else if (ni.isServer) RpcPush(ni.netId, force);
                    CmdPush(ni.netId, force);
                }
                else
                {
                    force = 100;
                    //if (ni.isClient && !ni.isServer) CmdPush(ni.netId, force);
                    //else if (ni.isServer) RpcPush(ni.netId, force);
                    CmdPush(ni.netId, force);
                }
            }
        }

    }

    [Command]
    public void CmdPush(NetworkInstanceId id, float force)
    {
        GameObject go = NetworkServer.FindLocalObject(id);
        bool isPlayer = false;
        if(go.CompareTag("Human") || go.CompareTag("Mosquito"))
        {
            isPlayer = true;
        }
        NetworkIdentity ni = go.GetComponent<NetworkIdentity>();
        //NetworkConnection nc = ni.connectionToClient;
        Debug.Log("cmd push call");
        if (!isPlayer)
        {
            CmdAddLocalAuthority(go);
            //Rigidbody rb = go.GetComponent<Rigidbody>();
            //Vector3 dir = this.transform.position - go.transform.position;
            //rb.AddForce(-dir * force);
            RpcPush(go, force);
            CmdRemoveLocalPlayerAuthority(go);
        }
        else
        {
            RpcPush(go, force);
            //Rigidbody rb = go.GetComponent<Rigidbody>();
            //Vector3 dir = this.transform.position - go.transform.position;
            //rb.AddForce(-dir * force);

            //ni.AssignClientAuthority(nc);
        }
    }

    //NetworkInstanceId id
    [ClientRpc]
    public void RpcPush(GameObject go, float force)
    {
        
        //GameObject go = NetworkServer.FindLocalObject(id);
        Debug.Log("rpc push call");
        Rigidbody rb = go.GetComponent<Rigidbody>();
        //Rigidbody rb = this.transform.GetComponent<Rigidbody>();
        Vector3 dir = this.transform.position - go.transform.position;
        rb.AddForce(-dir * force);


    }
    [Command]
    public void CmdAddLocalAuthority(GameObject go)
    {
        GameObject goClient = NetworkServer.FindLocalObject(go.GetComponent<NetworkIdentity>().netId);
        NetworkIdentity ni = goClient.GetComponent<NetworkIdentity>();
        PlayerConnectionObject pcu = this.transform.GetComponent<PlayerUnit>().ConnectionObject.GetComponent<PlayerConnectionObject>();
        ni.AssignClientAuthority(pcu.connectionToClient);
        //Debug.Log("add authority");
    }

    [Command]
    void CmdRemoveLocalPlayerAuthority(GameObject go)
    {
        GameObject goClient = NetworkServer.FindLocalObject(go.GetComponent<NetworkIdentity>().netId);
        NetworkIdentity ni = goClient.GetComponent<NetworkIdentity>();
        ni.RemoveClientAuthority(ni.clientAuthorityOwner);
        //Debug.Log("remove authority");
    }

}
