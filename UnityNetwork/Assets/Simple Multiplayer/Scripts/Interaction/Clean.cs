using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class Clean : NetworkBehaviour {

    public Transform fovCam;
    private const float OBJECT_INTERACT_DISTANCE = 2;
    private bool isScreenTouch;
    private List<int> fingerTouchIDUI;
    private int screenTouchCount;

    void Start () {
        fovCam = GameObject.Find("FOVCamera").transform;
        fingerTouchIDUI = new List<int>();
    }
	
	void Update () {
        if (!this.transform.GetComponent<NetworkIdentity>().hasAuthority)
        {
            return;
        }

        CheckCleanObject();
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

    }

    private void CheckCleanObject()
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
                if ((Input.GetKey(KeyCode.E) ) && child.CompareTag("Cleanable"))
#elif UNITY_IOS || UNITY_ANDROID
                if (isScreenTouch && child.CompareTag("Cleanable"))
#endif
                {
                    GameObject go = rayHit.transform.gameObject;
                    if (!go.GetComponent<NetworkIdentity>().hasAuthority)
                    {
                        CmdAddLocalAuthority(go);
                    }
                    go.transform.GetComponent<StandingWater>().CmdClean(0.5f);
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
        Debug.Log("add authority");
    }
}
