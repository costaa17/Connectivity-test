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
        Debug.Log("who: " + other.transform.parent.name);
        foreach (Transform child in other.transform.parent)
        {
            //Debug.Log("who?: " + child.name);
            if (child.CompareTag("Slapable"))
            {
                GameObject obj = child.parent.gameObject;
                NetworkInstanceId netId = obj.GetComponent<NetworkIdentity>().netId;
                Debug.Log("author: " + this.transform.Find("Name").GetComponent<TextMesh>().text);
                
                //This is bug due to object is given multiple authorities
                
                
                //Debug.Log("who pare?: " + obj.name);

                interactObject = obj;
                float force = 100;
                if (obj.CompareTag("Human"))
                {
                    CmdPush(netId, force);
                }
                else if (obj.CompareTag("Mosquito"))
                {
                    force = 100;
                    CmdPush(netId, force);
                }
                else
                {
                    force = 100;
                    CmdPush(netId, force);
                    //Rigidbody rb = obj.GetComponent<Rigidbody>();
                    //Vector3 dir = this.transform.position - obj.transform.position;
                    //if (rb != null) rb.AddForce(-dir * force);
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
        NetworkConnection nc = go.GetComponent<NetworkIdentity>().connectionToClient;

        if (!isPlayer)
        {
            CmdAddLocalAuthority(go);
            Rigidbody rb = go.GetComponent<Rigidbody>();
            Vector3 dir = this.transform.position - go.transform.position;
            rb.AddForce(-dir * force);
            CmdRemoveLocalPlayerAuthority(go);
        }
        else
        {
            Rigidbody rb = go.GetComponent<Rigidbody>();
            Vector3 dir = this.transform.position - go.transform.position;
            rb.AddForce(-dir * force);
            //NetworkIdentity ni = go.GetComponent<NetworkIdentity>();
            //ni.AssignClientAuthority(nc);
        }
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
