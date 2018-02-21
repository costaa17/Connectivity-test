using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HumanMovement : PlayerMovement {


    void Start () {
        base.Start();
    }

    //public override void OnStartAuthority()
    //{
    //    GameObject.Find("JumpButton").GetComponent<Button>().onClick.AddListener(Jump);
    //}

    void Update () {
        base.Update();
    }

    private float jumpForce = 250;

    public override void Jump()
    {
        Debug.Log("Jump force");
        if (IsGround)
        {
            this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce);
        }
    }

    public override void Movement()
    {
        float moveX, moveY;
        if (joystickLeft.IsEnable)
        {
            //int mul = (gyroControl.IsEnable) ? -1 : 1;
            moveX = joystickLeft.GetX;
            moveY = joystickLeft.GetY;
        }
        else
        {
            moveX = Input.GetAxis("Horizontal");
            moveY = Input.GetAxis("Vertical");
        }
        //Debug.Log("x movement : " + moveX);
        //Debug.Log("y movement : " + moveY);
        transform.Translate(moveX * Time.deltaTime * Speed, 0f, moveY * Time.deltaTime * Speed);
    }
}
