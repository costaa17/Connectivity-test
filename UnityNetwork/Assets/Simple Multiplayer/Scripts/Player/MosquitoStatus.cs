using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoStatus : PlayerStatus {

	// Use this for initialization
	void Start () {
        SetHealth(3);
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}

    public override void Reset()
    {
        this.transform.position = new Vector3(0, 10, 0);
        SetHealth(3);
    }
}
