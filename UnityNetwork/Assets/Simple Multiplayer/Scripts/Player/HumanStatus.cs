using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanStatus : PlayerStatus {
    public override void Reset()
    {
        this.transform.position = new Vector3(0, 20, 0);
        SetHealth(100);
    }

    // Use this for initialization
    void Start () {
        SetHealth(100);
	}

    private void Update()
    {
        base.Update();
    }

}
