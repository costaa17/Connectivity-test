using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickUpObject : MonoBehaviour {

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
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && objectPickup != null)
#elif UNITY_IOS || UNITY_ANDROID
        if (!isScreenTouch && screenTouchCount > 1 && objectPickup != null)
#endif
        {
            isHoldingObject = false;
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
            //Debug.Log("object detected: " + rayHit.collider.gameObject.name);
            foreach (Transform child in rayHit.transform)
            {
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN 
                if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && child.CompareTag("Pickupable"))
#elif UNITY_IOS || UNITY_ANDROID
                if ( isScreenTouch && child.CompareTag("Pickupable"))
#endif
                {
                    Debug.Log("pick up object");
                    objectPickup = rayHit.collider.gameObject;
                    isHoldingObject = true;
                }
            }
        }
    }
}
