using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerUnit : NetworkBehaviour {

    public GameObject ConnectionObject;

	void Start () {
        
    }

    public override void OnStartClient()
    {
        NetworkInstanceId id = this.gameObject.GetComponent<NetworkIdentity>().netId;
        //Debug.Log("id: " + this.gameObject.GetComponent<NetworkIdentity>().netId.Value);
        //Debug.Log("Client authority: " + this.gameObject.GetComponent<NetworkIdentity>().playerControllerId);


        // Get the PlayerConnectionObject by substract 1 from the netid
        // This is not the best strategy but it work for now
        // TODO: need a better way
        GameObject connectionObject = ClientScene.FindLocalObject(new NetworkInstanceId(id.Value - 1));
        ConnectionObject = connectionObject;
        //Debug.Log(connectionObject.name);
        ChangePlayerDisplayName(connectionObject.GetComponent<PlayerConnectionObject>().PlayerName);
    }


    public override void OnStartAuthority()
    {
        //Debug.Log("local(player unit): " + isLocalPlayer);
        //Debug.Log("hasauthor(player unit): " + hasAuthority);
        NetworkManager.singleton.gameObject.GetComponent<GameObjectList>().SetMainPlayerUnit(this.gameObject);
    }

    void Update () {


    }

    public void SetConnectionObject(GameObject co)
    {
        ConnectionObject = co;
    }

    public void ChangePlayerDisplayName(string name)
    {
        this.transform.Find("Name").GetComponent<TextMesh>().text = name;
    }

}
