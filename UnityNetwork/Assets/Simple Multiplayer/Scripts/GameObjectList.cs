using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameObjectList : NetworkBehaviour {

    public List<GameObject> gameObjectList;
    public GameObject mainClientUnit;

	void Start () {
        gameObjectList = new List<GameObject>();	

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetMainPlayerUnit(GameObject cUnit)
    {
        mainClientUnit = cUnit;
        GameObject.Find("JumpButton").GetComponent<FlyJumpButton>().SetPlayerMovemment(cUnit.GetComponent<PlayerMovement>());
    }
}
