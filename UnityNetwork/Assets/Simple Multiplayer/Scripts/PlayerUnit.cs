using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUnit : NetworkBehaviour {

    public GameObject ConnectionObject;
    public Camera cam;

    private float speed = 10;
    private float mouseSensitivity = 5;


	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    public override void OnStartClient()
    {
        NetworkInstanceId id = this.gameObject.GetComponent<NetworkIdentity>().netId;
        Debug.Log("id: " + this.gameObject.GetComponent<NetworkIdentity>().netId.Value);
        Debug.Log("Client authority: " + this.gameObject.GetComponent<NetworkIdentity>().clientAuthorityOwner);

        GameObject connectionObject = ClientScene.FindLocalObject(new NetworkInstanceId(id.Value - 1));
        ConnectionObject = connectionObject;
        Debug.Log(connectionObject.name);
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
        
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)
           || Input.GetKey(KeyCode.UpArrow)|| Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("Im movingg");
        }

        // Movement
        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * speed,
                              0, Input.GetAxis("Vertical") * Time.deltaTime * speed);


        // Rotate object
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, yRot, 0f) * mouseSensitivity;
        transform.Rotate(rotation);
        //rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));


        // Rotate camera
        float xRot = Input.GetAxisRaw("Mouse Y");
        Vector3 camRotation = new Vector3(xRot, 0, 0f) * mouseSensitivity;
        cam.transform.Rotate(-camRotation);


        // Mobile
#if UNITY_ANDROID || IOS
        if(Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if(Input.GetTouch(0).position.x > Screen.width / 2)
            {
                transform.Translate(1 * Time.deltaTime * speed, 0, 0);
            }else if(Input.GetTouch(0).position.x <= Screen.width / 2)
            {
                transform.Translate(-1 * Time.deltaTime * speed, 0, 0);
            }else if (Input.GetTouch(0).position.y > Screen.height / 2)
            {
                transform.Translate(0, 0, 1 * Time.deltaTime * speed);
            }
            else if (Input.GetTouch(0).position.y < Screen.height / 2)
            {
                transform.Translate(0, 0, -1 * Time.deltaTime * speed);
            }
        }
#endif

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
