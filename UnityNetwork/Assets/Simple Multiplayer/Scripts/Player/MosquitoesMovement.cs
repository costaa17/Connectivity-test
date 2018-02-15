using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MosquitoesMovement : PlayerMovement {

    private float speed = 5;
    private float mouseSensitivity = 5;
    private Transform head;
    private bool isGround;

    public enum MovementMode { Walking, Flying };
    public MovementMode movementMode = MovementMode.Walking;

    void Start () {
        base.Start();
    }

    public override void OnStartAuthority()
    {
        GameObject.Find("JumpButton").GetComponent<Button>().onClick.AddListener(Jump);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        if (go.CompareTag("Ground"))
        {
            isGround = true;
            this.gameObject.GetComponent<Rigidbody>().useGravity = true;
            movementMode = MovementMode.Walking;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject go = collision.gameObject;
        if (go.CompareTag("Ground"))
        {
            isGround = false;
            this.gameObject.GetComponent<Rigidbody>().useGravity = false;
            movementMode = MovementMode.Flying;
        }
    }

    void Update () {
        base.Update();
    }

    private float FilteredValue(float val)
    {
        if (val <= -0.01 || val >= 0.01) return val;
        else return 0;
    }

    private float jumpForce = 250;
    public override void Jump()
    {
        this.transform.position += Vector3.up * 2;
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
        if (isGround)
        {
            transform.Translate(moveX * Time.deltaTime * speed, 0f, moveY * Time.deltaTime * speed);
        }
        else
        {
            transform.position += (cam.transform.forward * moveY * speed * Time.deltaTime);
            transform.Translate(moveX * Time.deltaTime * speed, 0f, 0f);
        }

    }
}
