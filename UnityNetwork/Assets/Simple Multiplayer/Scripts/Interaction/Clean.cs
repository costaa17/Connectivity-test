using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Clean : MonoBehaviour {

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
        CheckCleanObject();
        Debug.Log("is screen touch: " + isScreenTouch);
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
            //Debug.Log("object detected: " + rayHit.collider.gameObject.name);
            //Debug.Log("nubber child ocunt: " + rayHit.transform.childCount);
            foreach (Transform child in rayHit.transform)
            {
                
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
                if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && child.CompareTag("Cleanable"))
#elif UNITY_IOS || UNITY_ANDROID
                if (isScreenTouch && child.CompareTag("Cleanable"))
#endif
                {
                    rayHit.transform.GetComponent<StandingWater>().Clean(0.5f);
                }
            }
        }
    }
}
