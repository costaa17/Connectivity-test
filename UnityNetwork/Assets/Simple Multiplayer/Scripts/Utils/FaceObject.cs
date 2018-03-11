using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FaceObject : MonoBehaviour {

    public GameObject mainObject;
    public List<GameObject> PlayerUnitList;

    void Start () {
        PlayerUnitList = new List<GameObject>();
        //Debug.Log("number of keys: " + ClientScene.objects.Keys.Count);
        //Debug.Log("number of fabs: " + NetworkManager.singleton.spawnPrefabs.Count);
        
        //foreach (var value in ClientScene.objects.Values)
        //{
        //    GameObject go = value.gameObject;
        //    if (go.CompareTag("PlayerUnit"))
        //    {
        //        if (go.transform.GetComponent<PlayerUnit>().hasAuthority)
        //        {
        //            mainObject = go;
        //            NetworkManager.singleton.gameObject.GetComponent<GameObjectList>().SetMainPlayerUnit(mainObject);
                    
        //            continue;
        //        }
        //        PlayerUnitList.Add(go);
        //    }
        //    //Debug.Log("Game object's name : " + go.name + " Game object id: " + go.GetComponent<NetworkIdentity>().netId);
        //}
    }

    void Update () {
        FollowCamera();
    }

    private void FollowCamera()
    {
        GameObject mainUnit = NetworkManager.singleton.gameObject.GetComponent<GameObjectListManager>().mainClientUnit;
        //Debug.Log("main unit: " + mainUnit);
        if (mainUnit != null)
        {
            this.gameObject.transform.LookAt(mainUnit.transform.position);
            this.gameObject.transform.Rotate(new Vector3(0, 180, 0));
        }
    }
}
