using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform playerTransform;
    private Vector3 forwardVec;
	void Start () {
        forwardVec = new Vector3();
	}
	
	// Update is called once per frame
	void Update () {
        if(playerTransform != null)
        {
            forwardVec.x = playerTransform.forward.x;
            forwardVec.z = playerTransform.forward.z;
            transform.position = playerTransform.position + (-forwardVec) * .5f;
        }
	}

    public void SetPlayerFollow(Transform t)
    {
        playerTransform = t;
    }
}
