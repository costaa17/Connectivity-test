using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Sting : PlayerAttack {

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Attack(GameObject other)
    {
        foreach (Transform child in other.transform.parent)
        {
            //Debug.Log("who?: " + child.name);
            if (child.CompareTag("Stingable"))
            {
                GameObject obj = child.parent.gameObject;
                NetworkIdentity ni = obj.GetComponent<NetworkIdentity>();
                Debug.Log("author: " + this.transform.Find("Name").GetComponent<TextMesh>().text);

                //This is bug due to object is given multiple authorities

                //Debug.Log("who pare?: " + obj.name);
                Debug.Log("is this ser: " + ni.isServer);
                Debug.Log("is this cli: " + ni.isClient);

                if (obj.CompareTag("Human"))
                {
                    CmdDealDamage(ni.netId, 0.5f, true);
                }

            }
        }
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
}
