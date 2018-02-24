using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class IsObjectPickUp : NetworkBehaviour {

    [SyncVar]
    private bool isPickup;

    public bool IsPickup { get { return isPickup; } set { isPickup = value; } }

    void Start () {
		
	}

	void Update () {
		
	}
}
