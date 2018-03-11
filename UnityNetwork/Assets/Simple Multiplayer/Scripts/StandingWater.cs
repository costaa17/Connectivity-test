using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StandingWater : NetworkBehaviour {

    [SyncVar]
    private float health = 100;
    [SyncVar]
    private int eggsLay;

    private TextMesh healthText;

	void Start () {
        healthText = transform.Find("HealthText").GetComponent<TextMesh>();
        
    }

	void Update () {

        if (IsAlive())
        {
            CmdDestroy();
        }
        healthText.text = "";
        healthText.text = "water health: " + health + " eggs: " + eggsLay;
    }

    public bool IsAlive()
    {
        return health <= 0;
    }
    
    [Command]
    public void CmdClean(float val)
    {
        health -= val;
    }

    [Command]
    public void CmdDestroy()
    {
        NetworkServer.Destroy(this.gameObject);
    }

    [Command]
    public void CmdAddEgg()
    {
        eggsLay++;
    }
}
