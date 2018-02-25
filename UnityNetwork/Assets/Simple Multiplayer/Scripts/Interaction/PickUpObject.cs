using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class PickUpObject : NetworkBehaviour {

    public Transform fovCam;
    
    private const float OBJECT_INTERACT_DISTANCE = 2.5f;
    private GameObject objectPickup;
    private bool isHoldingObject;
    private bool isScreenTouch;
    private int screenTouchCount;
    private List<int> fingerTouchIDUI;

    void Start () {
        fovCam = GameObject.Find("FOVCamera").transform;
        fingerTouchIDUI = new List<int>();
    }
	
	void Update () {
        if (!this.transform.parent.GetComponent<NetworkIdentity>().hasAuthority)
        {
            return;
        }

        if (isHoldingObject)
        {
            MovingObject();
            CheckDropObject();
        }
        else
        {
            CheckPickupObject();
        }
        //Debug.Log("screen touch count: " + screenTouchCount);
        //Debug.Log("is screen touch: " + isScreenTouch);
        if (Input.touchCount > 0)
        {
            foreach(Touch touch in Input.touches)
            {
                int id = touch.fingerId;
                if (touch.phase == TouchPhase.Began)
                {
                    if (!EventSystem.current.IsPointerOverGameObject(id))
                    {
                        isScreenTouch = true;
                    }
                    else
                    { 
                        fingerTouchIDUI.Add(id);
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (!fingerTouchIDUI.Contains(touch.fingerId))
                    {
                        if(isHoldingObject) screenTouchCount++;
                        isScreenTouch = false;
                    }
                    else
                    {
                        fingerTouchIDUI.Remove(touch.fingerId);
                    }
                }
               
            }
        }
    }

    private void MovingObject()
    {
        if (objectPickup != null)
        {
            objectPickup.transform.position = fovCam.transform.position + fovCam.forward * (OBJECT_INTERACT_DISTANCE - 0.1f);

            // lerping making position shaky
            //Vector3.Lerp(objectPickup.transform.position, 
            //                                         fovCam.transform.position + fovCam.forward * (OBJECT_PICKUP_DISTANCE - 0.1f),
            //                                           .5f);
        }
    }

    private void CheckDropObject()
    {
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN 
        //|| Input.GetMouseButtonDown(0)
        if ((Input.GetKeyDown(KeyCode.E) ) && objectPickup != null)
#elif UNITY_IOS || UNITY_ANDROID
        if (!isScreenTouch && screenTouchCount > 1 && objectPickup != null)
#endif
        {
            isHoldingObject = false;
            objectPickup.GetComponent<IsObjectPickUp>().IsPickup = false;
            CmdRemoveLocalPlayerAuthority(objectPickup);
            objectPickup = null;
            screenTouchCount = 0;
            Debug.Log("release object");
        }
    }

    private void CheckPickupObject()
    {
        RaycastHit rayHit;
        Ray ray = new Ray(fovCam.position, fovCam.forward);

        Debug.DrawRay(fovCam.position, fovCam.forward * OBJECT_INTERACT_DISTANCE);
        if (Physics.Raycast(ray, out rayHit, OBJECT_INTERACT_DISTANCE))
        {
            //Debug.Log("(pickup)object detected: " + rayHit.transform.name);
            foreach (Transform child in rayHit.transform)
            {
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN 
                // || Input.GetMouseButtonDown(0)
                if ((Input.GetKeyDown(KeyCode.E)) && child.CompareTag("Pickupable"))
#elif UNITY_IOS || UNITY_ANDROID
                if ( isScreenTouch && child.CompareTag("Pickupable"))
#endif
                {
                    Debug.Log("pick up object");
                    GameObject go = rayHit.transform.gameObject;
                    if (!go.GetComponent<IsObjectPickUp>().IsPickup)
                    {
                        CmdAddLocalAuthority(go);
                        objectPickup = go;
                        go.GetComponent<IsObjectPickUp>().IsPickup = true;
                        isHoldingObject = true;
                    }
                }
            }
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
