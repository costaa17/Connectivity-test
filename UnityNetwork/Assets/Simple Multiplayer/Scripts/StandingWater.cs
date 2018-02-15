using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StandingWater : NetworkBehaviour {

    private float health = 100;
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
        healthText.text = "water health: " + health;
    }

    public bool IsAlive()
    {
        return health <= 0;
    }
    
    public void Clean(float val)
    {
        health -= val;
    }

    [Command]
    public void CmdDestroy()
    {
        NetworkServer.Destroy(this.gameObject);
    }

}
