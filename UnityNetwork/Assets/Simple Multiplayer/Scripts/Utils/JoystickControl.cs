using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickControl : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler{

    private Image background;
    private Image joystick;
    private Vector2 inputVector;

    public bool IsEnable { get; set; }
    public float Rotation { get; private set; }

    public float GetX
    {
        get { return inputVector.x; }
    }

    public float GetY
    {
        get { return inputVector.y; }
    }

    void Start()
    {
        background = this.transform.GetChild(0).GetComponent<Image>();
        joystick = this.transform.GetChild(1).GetComponent<Image>();
        IsEnable = false;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotation();

    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background.rectTransform,
                                                                    eventData.position,
                                                                    eventData.pressEventCamera,
                                                                    out position))
        { 
            position.x = (position.x / background.rectTransform.sizeDelta.x);
            position.y = (position.y / background.rectTransform.sizeDelta.y);

            inputVector = new Vector2(position.x * 2, position.y * 2);
            inputVector = inputVector.magnitude > 1 ? inputVector.normalized : inputVector;
            joystick.rectTransform.anchoredPosition = new Vector2(inputVector.x * background.rectTransform.sizeDelta.x / 3, 
                                                                  inputVector.y * background.rectTransform.sizeDelta.y / 3);
            //Debug.Log(inputVector);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.OnDrag(eventData);
        IsEnable = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystick.rectTransform.anchoredPosition = new Vector2(0, 0);
        inputVector = new Vector2(0, 0);
        IsEnable = false;
    }
    
    /**
     * Rotation in rad
     **/
    private void UpdateRotation()
    {
        Rotation = Mathf.Atan2(GetY,GetX) / 2;
        //Debug.Log("y: " + GetY + " x: " + GetX + " Rotation" + Rotation);
    }
}
