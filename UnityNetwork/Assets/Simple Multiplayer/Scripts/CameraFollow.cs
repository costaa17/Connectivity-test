using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform playerTransform;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(playerTransform != null)
        {
            transform.position = playerTransform.position;
        }
	}

    public void SetPlayerFollow(Transform t)
    {
        playerTransform = t;
    }
}
