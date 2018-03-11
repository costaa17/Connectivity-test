using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameObjectListManager : MonoBehaviour {

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
        Debug.Log("set player unit");
        mainClientUnit = cUnit;
        GameObject.Find("JumpButton").GetComponent<FlyJumpButton>().SetPlayerMovement(mainClientUnit.GetComponent<PlayerMovement>());

        GameObject.Find("AttackButton").GetComponent<AttackButton>()
                                       .SetAttackTriggerBox(mainClientUnit.transform.Find("AttackCollider")
                                       .GetComponent<AttackTriggerBox>());

        GameObject.Find("AttackButton").GetComponent<Button>().onClick
                                                              .AddListener(mainClientUnit.transform.Find("AttackCollider")
                                                              .GetComponent<AttackTriggerBox>().OnClick);
        GameObject.Find("StatusPanel").GetComponent<StatusDisplay>().playerStatus = mainClientUnit.GetComponent<PlayerStatus>();

       
    }
}
