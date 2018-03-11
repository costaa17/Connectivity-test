using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusDisplay : MonoBehaviour {

    public PlayerStatus playerStatus;

    private Text healthText;

	void Start () {
        healthText = this.transform.Find("HealthText").GetComponent<Text>();
        healthText.text = "Health: 0";
	}
	
	// Update is called once per frame
	void Update () {
        DisplayHealth();
	}

    private void DisplayHealth()
    {
        if(playerStatus != null)
        {
            healthText.text = "Health: " + playerStatus.Health;
        }
    }
}
