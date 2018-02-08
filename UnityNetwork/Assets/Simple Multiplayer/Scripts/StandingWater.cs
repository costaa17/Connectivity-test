using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingWater : MonoBehaviour {

    private float health = 100;
    private TextMesh healthText;
	void Start () {
        healthText = transform.Find("HealthText").GetComponent<TextMesh>();
        
    }

	void Update () {
        if (IsAlive())
        {
            DestroyObject(this.gameObject);
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

}
