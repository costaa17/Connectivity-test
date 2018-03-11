using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanStatus : PlayerStatus {

    // Use this for initialization
    void Start () {
        SetHealth(100);
	}

    private void Update()
    {
        base.Update();
    }

}
