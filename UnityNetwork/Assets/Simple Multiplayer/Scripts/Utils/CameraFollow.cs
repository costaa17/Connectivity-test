using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform playerTransform;
    private Vector3 forwardVec;
	void Start () {
        forwardVec = new Vector3();
	}
	
	void Update () {
        if(playerTransform != null)
        {
            forwardVec.x = playerTransform.forward.x;
            forwardVec.z = playerTransform.forward.z;
            transform.position = playerTransform.position;
        }
	}

    public void SetPlayerFollow(Transform t)
    {
        playerTransform = t;
    }
}
