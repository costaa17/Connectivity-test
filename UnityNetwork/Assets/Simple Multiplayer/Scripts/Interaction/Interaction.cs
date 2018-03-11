using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public abstract class Interaction : NetworkBehaviour{

    public Transform fovCam;
    private const float OBJECT_INTERACT_DISTANCE = 2;
    private bool isScreenTouch, previousTouch;
    private List<int> fingerTouchIDUI;
    private int screenTouchCount;
    private string tag;
    private bool isHoldAllow;

    public void Start()
    {
        fovCam = GameObject.Find("FOVCamera").transform;
        fingerTouchIDUI = new List<int>();
    }

    public void Update()
    {
        if (!this.transform.GetComponent<NetworkIdentity>().hasAuthority)
        {
            return;
        }


        //Debug.Log("is screen touch: " + isScreenTouch);
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                int id = touch.fingerId;
                if (touch.phase == TouchPhase.Began)
                {
                    if (!EventSystem.current.IsPointerOverGameObject(id))
                    {
                        isScreenTouch = true;
                        Debug.Log("should not touching an UI");
                    }
                    else
                    {
                        Debug.Log("touching an UI");
                        fingerTouchIDUI.Add(id);
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (!fingerTouchIDUI.Contains(touch.fingerId))
                    {
                        screenTouchCount++;
                        isScreenTouch = false;
                    }
                    else
                    {
                        fingerTouchIDUI.Remove(touch.fingerId);
                    }
                }

            }
        }
        CheckObject();

        previousTouch = isScreenTouch;
    }

    private void CheckObject()
    {
        RaycastHit rayHit;
        Ray ray = new Ray(fovCam.position, fovCam.forward);

        Debug.DrawRay(fovCam.position, fovCam.forward * OBJECT_INTERACT_DISTANCE);
        if (Physics.Raycast(ray, out rayHit, OBJECT_INTERACT_DISTANCE))
        {
            //Debug.Log("(clean)object detected: " + rayHit.transform.name);
            foreach (Transform child in rayHit.transform)
            {
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
                //|| Input.GetMouseButton(0)
                if ((Input.GetKeyDown(KeyCode.E) ) && child.CompareTag(tag))
#elif UNITY_IOS || UNITY_ANDROID
                if (IsClickHold(isHoldAllow) && child.CompareTag(tag))
#endif
                {
                    GameObject go = rayHit.transform.gameObject;
                    if (!go.GetComponent<NetworkIdentity>().hasAuthority)
                    {
                        CmdAddLocalAuthority(go);
                    }
                    Functionality(go);
                    //go.transform.GetComponent<StandingWater>().CmdAddEgg();
                }
            }
        }
    }

    public void SetTag(string tag)
    {
        this.tag = tag;
    }

    public void SetHoldAllow(bool value)
    {
        isHoldAllow = value;
    }

    private bool IsClickHold(bool value)
    {
        if (value) return isScreenTouch;
        else return isScreenTouch && !previousTouch;
    }

    public abstract void Functionality(GameObject go);
    

    [Command]
    public void CmdAddLocalAuthority(GameObject go)
    {
        GameObject goClient = NetworkServer.FindLocalObject(go.GetComponent<NetworkIdentity>().netId);
        NetworkIdentity ni = goClient.GetComponent<NetworkIdentity>();
        PlayerConnectionObject pcu = this.transform.GetComponent<PlayerUnit>().ConnectionObject.GetComponent<PlayerConnectionObject>();
        ni.AssignClientAuthority(pcu.connectionToClient);
        Debug.Log("add authority");
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
