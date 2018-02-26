using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private AttackTriggerBox attackTriggerBox;
    private bool isButtonHold;

    public bool IsButtonHold { get { return isButtonHold; } private set { isButtonHold = value; } }

    public void Update()
    {
        //Debug.Log("is butotn hold: " + isButtonHold);
         if (attackTriggerBox != null && attackTriggerBox.transform.parent.CompareTag("Mosquito") && isButtonHold)
        {
            //Debug.Log("im attacking ");
            attackTriggerBox.OnClick();
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

    public void SetAttackTriggerBox(AttackTriggerBox atb)
    {
        attackTriggerBox = atb;
    }

}
