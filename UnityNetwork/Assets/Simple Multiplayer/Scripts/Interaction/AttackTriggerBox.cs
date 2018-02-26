using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AttackTriggerBox : MonoBehaviour {

    public PlayerAttack playerAttack;
    public AttackButton attackButton;
    public PlayerMovement playerMovement;

    private Camera cam;
    private bool isAttack;
    private float timeElapsed;
    private float timeResetMax = 0.5f;
    public Collider attackCollider;

	void Start () {
        attackCollider = transform.GetComponent<BoxCollider>();
        attackCollider.enabled = false;
        attackButton = GameObject.Find("AttackButton").GetComponent<AttackButton>();
        cam = playerMovement.cam;
	}

	void Update () {
        if (!this.transform.parent.GetComponent<NetworkIdentity>().hasAuthority)
        {
            return;
        }
        //Debug.Log("time eleaspe: " + timeElapsed);
        //Debug.Log("is attack: " + isAttack);
        transform.position = cam.transform.position + cam.transform.forward * 2;
        //Debug.Log("Time: " + timeElapsed);
        if (isAttack)
        {
            timeElapsed += Time.deltaTime;
        }

        if (IsResetAttackBox())
        {
            timeElapsed = 0;
            isAttack = false;
            //Debug.Log("disableee");
            attackCollider.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            OnClick();
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (transform.parent.CompareTag("Human"))
        {
            playerAttack.Attack(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (transform.parent.CompareTag("Mosquito"))
        {
            playerAttack.Attack(other.gameObject);
        }
    }

    public void OnClick()
    {
        isAttack = true;
        //Debug.Log("enable");
        attackCollider.enabled = true;
    }

    private bool IsResetAttackBox()
    {
        if (transform.parent.gameObject.CompareTag("Human"))
        {
            return timeElapsed >= timeResetMax;
        }else if (transform.parent.gameObject.CompareTag("Mosquito"))
        {
            return !attackButton.IsButtonHold;
        }
        else
        {
            return false;
        }
    }
}
