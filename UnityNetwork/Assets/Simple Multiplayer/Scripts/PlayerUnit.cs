using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUnit : NetworkBehaviour {

    private PlayerConnectionObject ConnectionObject;
    public float speed = 10;

	void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        //Debug.Log(Network.connections.Length);
        //Debug.Log("co " + ConnectionObject.PlayerName);
        // this.transform.Find("name").GetComponent<TextMesh>().text;

        if (!hasAuthority)
        {
            return;
        }
        // Debug.Log(hasAuthority);
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)
           || Input.GetKey(KeyCode.UpArrow)|| Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("Im movingg");
        }
        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * speed,
                              0, Input.GetAxis("Vertical") * Time.deltaTime * speed);

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

    public void SetConnectionObject(PlayerConnectionObject co)
    {
        ConnectionObject = co;
    }


}
