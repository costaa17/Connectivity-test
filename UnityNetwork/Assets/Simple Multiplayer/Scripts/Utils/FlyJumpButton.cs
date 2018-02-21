using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FlyJumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public PlayerMovement playerMovement;
    private bool isButtonHold;

    public bool IsButtonHold { get { return isButtonHold; } }

    void Start () {
	    	
	}
	
	// Update is called once per frame
	void Update () {
        if (playerMovement != null && playerMovement.GetType() == typeof(MosquitoesMovement) && isButtonHold)
        {   
            playerMovement.Jump();
        }else if (playerMovement != null && playerMovement.GetType() == typeof(HumanMovement))
        {
            // PC control (need better structure)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerMovement.Jump();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonHold = true;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        isButtonHold = false;
    }

    // Called when a new authorized player create
    public void SetPlayerMovemment(PlayerMovement playerMovement)
    {
        this.playerMovement = playerMovement;
    }

}
