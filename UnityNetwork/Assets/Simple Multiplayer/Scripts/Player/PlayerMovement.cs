using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public abstract class PlayerMovement : NetworkBehaviour {

    public float minViewAngle;
    public float maxViewAngle; 

    public Camera cam;
    public JoystickControl joystickLeft, joystickRight;
    public GyroscopeController gyroControl;
    public Toggle toggleFVR;

    
    private FlyJumpButton fjButton;
    private bool isGround;
    private float speed = 5; 
    private float mouseSensitivity = 5;
    private Transform head;

    public float Speed { get { return speed; } set { speed = value;  } }
    public float MouseSensitivity { get { return mouseSensitivity; } set { mouseSensitivity = value; } }
    public FlyJumpButton FJButton { get { return fjButton; } }
    public bool IsGround { get { return isGround; } set { isGround = value; } }

    public void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        joystickLeft = GameObject.Find("VirtualJoystickLeft").GetComponent<JoystickControl>();
        joystickRight = GameObject.Find("VirtualJoystickRight").GetComponent<JoystickControl>();
        head = transform.Find("Head");
        gyroControl = this.GetComponent<GyroscopeController>();
        toggleFVR = GameObject.Find("ToggleFVR").GetComponent<Toggle>();
        cam = GameObject.Find("FOVCamera").GetComponent<Camera>();
        fjButton = GameObject.Find("JumpButton").GetComponent<FlyJumpButton>();
    }

    public override void OnStartAuthority()
    {
        GameObject.Find("JumpButton").GetComponent<Button>().onClick.AddListener(Jump);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        if (go.CompareTag("Ground"))
        {
            isGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject go = collision.gameObject;
        if (go.CompareTag("Ground"))
        {
            isGround = false;
        }
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

        // Movement
        #region Movement
        Movement();
        #endregion


        //Debug.Log("is gyro on: " + gyroControl.IsEnable);
        #region POV movement
        if (gyroControl.IsEnable && toggleFVR.isOn)
        {
            //POV movement with VR

            // rotate cam & object

            Quaternion gyroRotation = gyroControl.GetOrientation();
            cam.transform.localRotation = gyroRotation;
            transform.forward = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
            head.eulerAngles = new Vector3(-cam.transform.eulerAngles.x, head.eulerAngles.y, head.eulerAngles.z);

        }
        else if (!toggleFVR.isOn)
        {
            // POV movement without VR

            // Rotate camera up-down
            float xRot = joystickRight.GetY;
            RotateCamUpDown(xRot);

            // Rotate object left-right
            float yRot = joystickRight.GetX;
            Vector3 rotation = new Vector3(0f, yRot, 0f) * mouseSensitivity;
            transform.Rotate(rotation);
            cam.transform.eulerAngles = new Vector3(cam.transform.eulerAngles.x,
                                        transform.eulerAngles.y,
                                        cam.transform.eulerAngles.z);
        }
        else
        {
            // Movement PC



            // Rotate camera up-down
            float xRot = Input.GetAxisRaw("Mouse Y");
            RotateCamUpDown(xRot);


            // Rotate object left-right
            float yRot = Input.GetAxisRaw("Mouse X");
            Vector3 rotation = new Vector3(0f, yRot, 0f) * mouseSensitivity;
            transform.Rotate(rotation);
            cam.transform.eulerAngles = new Vector3(cam.transform.eulerAngles.x, 
                                                    transform.eulerAngles.y,
                                                    cam.transform.eulerAngles.z);

        }
        //Debug.Log("x rotate rate: " + gyroControl.gyro.rotationRate.x);
        //Debug.Log("player tranform angle: " + transform.eulerAngles);

        #endregion

        // Mobile
#if UNITY_ANDROID || IOS

#endif

    }

    // Rotate camera with bound
    private void RotateCamUpDown(float value)
    {
        Vector3 camRotation = new Vector3(value, 0, 0f) * mouseSensitivity;
        cam.transform.Rotate(-camRotation);
        Vector3 camEuler = cam.transform.eulerAngles;
        float angle = camEuler.x > 270 ? camEuler.x - 360 : camEuler.x;
        cam.transform.rotation = Quaternion.Euler(Mathf.Clamp(angle, minViewAngle, maxViewAngle), 0, 0);
    }

    //Unuse
    private bool IsCameraRotationBounded(Transform trans)
    {
        Vector3 camForward = trans.forward;
        Vector3 planeVector = new Vector3(camForward.x, 0, camForward.z).normalized;
        float angle = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(planeVector, camForward));
        Debug.Log("angle between cam forward and ground forward: " + angle);

        return angle >= 85;
    }

    //Unuse
    private float FilteredValue(float val)
    {
        if (val <= -0.01 || val >= 0.01) return val;
        else return 0;
    }

    public abstract void Jump();
    public abstract void Movement();
}
