using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerUnit : NetworkBehaviour {

    public GameObject ConnectionObject;
    public Camera cam;
    public JoystickControl joystickLeft, joystickRight;
    public GyroscopeController gyroControl;
    public Toggle toggleFVR;

    private float speed = 5;
    private float mouseSensitivity = 5;
    private Transform head; 

	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        joystickLeft = GameObject.Find("VirtualJoystickLeft").GetComponent<JoystickControl>();
        joystickRight = GameObject.Find("VirtualJoystickRight").GetComponent<JoystickControl>();
        head = transform.Find("Head");
        gyroControl = this.GetComponent<GyroscopeController>();
        toggleFVR = GameObject.Find("ToggleFVR").GetComponent<Toggle>();
        cam = GameObject.Find("FOVCamera").GetComponent<Camera>();
    }

    public override void OnStartClient()
    {
        NetworkInstanceId id = this.gameObject.GetComponent<NetworkIdentity>().netId;
        Debug.Log("id: " + this.gameObject.GetComponent<NetworkIdentity>().netId.Value);
        //Debug.Log("Client authority: " + this.gameObject.GetComponent<NetworkIdentity>().playerControllerId);


        // Get the PlayerConnectionObject by substract 1 from the netid
        // This is not the best strategy but it work for now
        // TODO: need a better way
        GameObject connectionObject = ClientScene.FindLocalObject(new NetworkInstanceId(id.Value - 1));
        ConnectionObject = connectionObject;
        //Debug.Log(connectionObject.name);
        ChangePlayerDisplayName(connectionObject.GetComponent<PlayerConnectionObject>().PlayerName);
    }


    public override void OnStartAuthority()
    {
        //Debug.Log("local(player unit): " + isLocalPlayer);
        //Debug.Log("hasauthor(player unit): " + hasAuthority);
    }

    void Update () {
        //Debug.Log("local(player unit): " + isLocalPlayer);
        //Debug.Log("hasauthor(player unit): " + hasAuthority);
        if (!hasAuthority)
        {
            return;
        }

        this.transform.Find("Plane").eulerAngles = GameObject.Find("Ground").transform.eulerAngles;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)
           || Input.GetKey(KeyCode.UpArrow)|| Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("Im movingg");
        }

        // Movement
#region Movement
        float moveX, moveY;
        if (joystickLeft.IsEnable)
        {
            //int mul = (gyroControl.IsEnable) ? -1 : 1;
            moveX = joystickLeft.GetX;
            moveY = joystickLeft.GetY;
        }
        else
        {
            moveX = Input.GetAxis("Horizontal");
            moveY = Input.GetAxis("Vertical");
        }
        //Debug.Log("x movement : " + moveX);
        //Debug.Log("y movement : " + moveY);
        transform.Translate(moveX * Time.deltaTime * speed, 0f , moveY * Time.deltaTime * speed);
        #endregion

        //Debug.Log("is gyro on: " + gyroControl.IsEnable);
        #region FOV movement
        if (gyroControl.IsEnable && toggleFVR.isOn)
        {
            //Vector3 rotation = new Vector3(0f, 0f, 0f);
            //transform.Rotate(rotation);
            
            // rotate camera 
            //cam.transform.localRotation = gyroControl.GetOrientation();

            Vector3 angle = gyroControl.GetOrientation().eulerAngles;
            //Debug.Log("gyro angle: " + angle);
            //Debug.Log("gyro quad: " + gyroControl.GetOrientation());
            // rotate object
            //float y = angle.y;
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, y, transform.eulerAngles.z);
            head.rotation = gyroControl.GetOrientation();
            cam.transform.localRotation = head.rotation;
            //Debug.Log("Cam dir: " + cam.transform.forward);
            //Debug.Log("object dir: " + transform.forward);
            transform.forward = new Vector3(cam.transform.forward.x, 0 , cam.transform.forward.z );
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, head.eulerAngles.y, transform.eulerAngles.z);
            //transform.eulerAngles = head.rotation.eulerAngles;

            //Debug.Log("y rotate rate: " + gyroControl.gyro.rotationRate.y);
            //Debug.Log("x rotate rate: " + gyroControl.gyro.rotationRate.x);
            float y = FilteredValue(gyroControl.gyro.rotationRate.y);
            float x = FilteredValue(gyroControl.gyro.rotationRate.x);
            //transform.RotateAround(transform.position, Vector3.up, -y * Time.deltaTime * mouseSensitivity * 10);
            //head.RotateAround(head.position, Vector3.right, -x * Time.deltaTime * mouseSensitivity * 10);
        }
        else if (!toggleFVR.isOn)
        {
            // Rotate object
            float yRot = joystickRight.GetX;
            Vector3 rotation = new Vector3(0f, yRot, 0f) * mouseSensitivity;
            transform.Rotate(rotation);
            //rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));


            // Rotate camera
            float xRot = joystickRight.GetY;
            Vector3 camRotation = new Vector3(xRot, 0, 0f) * mouseSensitivity;
            cam.transform.Rotate(-camRotation);
        }
        else
        {
            // Rotate object
            float yRot = Input.GetAxisRaw("Mouse X");
            Vector3 rotation = new Vector3(0f, yRot, 0f) * mouseSensitivity;
            transform.Rotate(rotation);
            cam.transform.eulerAngles = new Vector3(cam.transform.eulerAngles.x, transform.eulerAngles.y, cam.transform.eulerAngles.z);
            //rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));


            // Rotate camera
            float xRot = Input.GetAxisRaw("Mouse Y");
            Vector3 camRotation = new Vector3(xRot, 0, 0f) * mouseSensitivity;
            cam.transform.Rotate(-camRotation);
        }
        //Debug.Log("x rotate rate: " + gyroControl.gyro.rotationRate.x);

        Debug.Log("player tranform angle: " + transform.eulerAngles);

        #endregion

        // Mobile
#if UNITY_ANDROID || IOS

#endif

    }

    private float FilteredValue(float val)
    {
        if (val <= -0.01 || val >= 0.01) return val;
        else return 0;
    }

    public void SetConnectionObject(GameObject co)
    {
        ConnectionObject = co;
    }

    public void ChangePlayerDisplayName(string name)
    {
        this.transform.Find("Name").GetComponent<TextMesh>().text = name;
    }

}
