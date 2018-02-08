using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GyroscopeController : NetworkBehaviour {

    public bool IsEnable { get; private set; }
    public Gyroscope gyro;

    private Quaternion rotation;
    private GameObject camParent;
    private CameraFollow cameraFollow;

    void Start () {

        
	}

    public override void OnStartAuthority()
    {
        cameraFollow = GameObject.Find("FOVCamera").GetComponent<CameraFollow>();
        cameraFollow.SetPlayerFollow(this.transform.Find("Head"));
        if (!CheckGyroscopeSupport()) return;
        Debug.Log("run gyro");

    }

    
    void Update () {
        if (!IsEnable) { return;  }
        //Debug.Log("Gyro use: " + IsEnable);
        //Debug.Log("Gyro atttitude: " + gyro.attitude + " gyro orientation: " + GetOrientation());
	}

    private bool CheckGyroscopeSupport()
    {
        //SystemInfo.supportsGyroscope
        if (SystemInfo.supportsGyroscope)
        {
            camParent = new GameObject("CamParent");
            Transform head = transform.Find("Head");
            camParent.transform.position = head.position;
            //camParent.transform.parent = this.transform;
            GameObject.Find("FOVCamera").transform.SetParent(camParent.transform);


            camParent.transform.eulerAngles = Vector3.right * 90;

            gyro = Input.gyro;
            gyro.enabled = true;
            IsEnable = true;
            rotation = new Quaternion(0, 0, -1, -1); 
            return true;
        }
        return false;
    }

    public Quaternion GetOrientation()
    {
        //return new Quaternion(gyro.attitude.y, -gyro.attitude.z, gyro.attitude.x, gyro.attitude.w);
        //return new Quaternion(gyro.attitude.z, -gyro.attitude.y, -gyro.attitude.x, gyro.attitude.w);
        return new Quaternion(gyro.attitude.x, gyro.attitude.y, -gyro.attitude.z, -gyro.attitude.w);
    }
}
