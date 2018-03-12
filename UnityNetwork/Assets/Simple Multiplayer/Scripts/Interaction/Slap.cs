using System;
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

                //Debug.Log("who pare?: " + obj.name);
                Debug.Log("is this ser: " + ni.isServer);
                Debug.Log("is this cli: " + ni.isClient);
                interactObject = obj;
                float force = 50;
                if (obj.CompareTag("Human"))
                {
                    //obj.GetComponent<HumanStatus>().CmdDealDamage(1, false);
                    CmdDealDamage(ni.netId, 1, false);
                    CmdPush(ni.netId, force);
                }
                else if (obj.CompareTag("Mosquito"))
                {
                    force = 100;
                    //obj.GetComponent<MosquitoStatus>().CmdDealDamage(1,false);
                    CmdDealDamage(ni.netId, 1, false);
                    CmdPush(ni.netId, force);
                }
                else
                {
                    force = 50;
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
        //Debug.Log("cmd push call");
        if (!isPlayer)
        {
            CmdAddLocalAuthority(go);
            RpcPush(go, force);
            CmdRemoveLocalPlayerAuthority(go);
        }
        else
        {
            RpcPush(go, force);
        }
    }

    //NetworkInstanceId id
    [ClientRpc]
    public void RpcPush(GameObject go, float force)
    {
        //GameObject go = NetworkServer.FindLocalObject(id);
        //Debug.Log("rpc push call");
        Rigidbody rb = go.GetComponent<Rigidbody>();
        Vector3 dir = this.transform.position - go.transform.position;
        rb.AddForce(-dir * force);
    }

    [Command]
    public void CmdDealDamage(NetworkInstanceId ni, float damage, bool isCon)
    {
        GameObject go = NetworkServer.FindLocalObject(ni);
        RpcDealDamage(go, damage, isCon);
    }
    [ClientRpc]
    public void RpcDealDamage(GameObject go, float damage, bool isCon)
    {
        go.GetComponent<PlayerStatus>().DealDamage(damage, isCon);
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
