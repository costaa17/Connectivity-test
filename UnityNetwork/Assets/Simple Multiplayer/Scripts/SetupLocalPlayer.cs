using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour { 

    public Behaviour[] BehavioursToDisable;
    public Camera sceneCamera;

	void Start () {
        //Debug.Log("4local(set up): " + isLocalPlayer);
        //Debug.Log("4hasauthor(set up): " + hasAuthority);
    }

    // This is call when client spawn and is called before OnStartAuthority
    // hasAuthority should be false first
    public override void OnStartClient()
    {
        //Debug.Log("1local(set up): " + isLocalPlayer);
        //Debug.Log("1hasauthor(set up): " + hasAuthority);
        
        //if (!hasAuthority)
        //{
        
        //}

        Debug.Log("disable stuff");
        foreach (Behaviour be in BehavioursToDisable)
        {
            be.enabled = false;
        }
        if(Camera.main != null)
        {
            Camera.main.gameObject.SetActive(false);
        }
    }

    public override void OnStartAuthority()
    {
        //Debug.Log("2local(set up): " + isLocalPlayer);
        //Debug.Log("2hasauthor(set up): " + hasAuthority);

        Debug.Log("enable stuff");
        foreach (Behaviour be in BehavioursToDisable)
        {
            be.enabled = true;
        }
        this.gameObject.transform.Find("Body").GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.transform.Find("Name").gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update ()
    {
        
    }

    private void OnDisable()
    {
        if(Camera.main != null)
        {
            Camera.main.gameObject.SetActive(true);
        }
    }

}
