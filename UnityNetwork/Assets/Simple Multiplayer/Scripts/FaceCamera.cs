using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FaceCamera : NetworkBehaviour {

    private Camera mainCam;

	void Start () {
        
	}

    public override void OnStartAuthority()
    {
        mainCam = this.transform.parent.Find("Camera").GetComponent<Camera>();
    }

    void Update () {
        if(Camera.main != null)
        {
            //this.transform.LookAt(Camera.main.transform.position);
            //this.transform.Rotate(new Vector3(0, 180, 0));
        }
        else
        {
            
        }
	}
}
