using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class LayEgg : Interaction {

    void Start()
    {
        base.Start();
        SetHoldAllow(false);
        SetTag("Layeggable");
    }

    void Update()
    {
        base.Update(); 
    }

    public override void Functionality(GameObject go)
    {
        go.transform.GetComponent<StandingWater>().CmdAddEgg();
    }
}
