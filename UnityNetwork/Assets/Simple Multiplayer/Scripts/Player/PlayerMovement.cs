using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public abstract class PlayerMovement : NetworkBehaviour {

    public Camera cam;
    public JoystickControl joystickLeft, joystickRight;
    public GyroscopeController gyroControl;
    public Toggle toggleFVR;

    private float speed = 5;
    private float mouseSensitivity = 5;
    private Transform head;

    public void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        joystickLeft = GameObject.Find("VirtualJoystickLeft").GetComponent<JoystickControl>();
        joystickRight = GameObject.Find("VirtualJoystickRight").GetComponent<JoystickControl>();
        head = transform.Find("Head");
        gyroControl = this.GetComponent<GyroscopeController>();
        toggleFVR = GameObject.Find("ToggleFVR").GetComponent<Toggle>();
        cam = GameObject.Find("FOVCamera").GetComponent<Camera>();
    }

    public override void OnStartAuthority()
    {
        GameObject.Find("JumpButton").GetComponent<Button>().onClick.AddListener(Jump);
    }

    // Update is called once per frame
    public void Update () {

        if (!hasAuthority)
        {
            return;
        }

        // this.transform.Find("Plane").eulerAngles = GameObject.Find("Ground").transform.eulerAngles;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)
           || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("Im movingg");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // Movement
        #region Movement
        Movement();
        #endregion

        //Debug.Log("is gyro on: " + gyroControl.IsEnable);
        #region POV movement
        if (gyroControl.IsEnable && toggleFVR.isOn)
        {
            //POV movement with VR

            // rotate object & object

            Quaternion gyroRotation = gyroControl.GetOrientation();
            cam.transform.localRotation = gyroRotation;
            transform.forward = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
            head.eulerAngles = new Vector3(-cam.transform.eulerAngles.x, head.eulerAngles.y, head.eulerAngles.z);

            //Debug.Log("y rotate rate: " + gyroControl.gyro.rotationRate.y);
            //Debug.Log("x rotate rate: " + gyroControl.gyro.rotationRate.x);
            //float y = FilteredValue(gyroControl.gyro.rotationRate.y);
            //float x = FilteredValue(gyroControl.gyro.rotationRate.x);
            //transform.RotateAround(transform.position, Vector3.up, -y * Time.deltaTime * mouseSensitivity * 10);
            //head.RotateAround(head.position, Vector3.right, -x * Time.deltaTime * mouseSensitivity * 10);
        }
        else if (!toggleFVR.isOn)
        {
            // POV movement without VR

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
            // Movement PC

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

        //Debug.Log("player tranform angle: " + transform.eulerAngles);

        #endregion

        // Mobile
#if UNITY_ANDROID || IOS

#endif

    }

    private void CameraRotationBound()
    {

    }

    private float FilteredValue(float val)
    {
        if (val <= -0.01 || val >= 0.01) return val;
        else return 0;
    }

    public abstract void Jump();
    public abstract void Movement();
}
