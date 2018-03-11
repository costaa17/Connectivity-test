using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MosquitoesMovement : PlayerMovement {

    private Rigidbody rb;

    public enum MovementMode { Walking, Flying };
    public MovementMode movementMode = MovementMode.Walking;

    void Start () {
        base.Start();
        rb = this.gameObject.GetComponent<Rigidbody>();
        this.gameObject.GetComponent<Rigidbody>().useGravity = false;

    }

    public override void OnStartAuthority()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        if (go.CompareTag("Ground"))
        {
            IsGround = true;
            //this.gameObject.GetComponent<Rigidbody>().useGravity = true;
            movementMode = MovementMode.Walking;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject go = collision.gameObject;
        if (go.CompareTag("Ground"))
        {
            IsGround = false;
            //this.gameObject.GetComponent<Rigidbody>().useGravity = false;
            movementMode = MovementMode.Flying;
        }
    }

    void Update () {
        base.Update();
        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }
        //Debug.Log("velocity: " + rb.velocity);
    }

    private float FilteredValue(float val)
    {
        if (val <= -0.01 || val >= 0.01) return val;
        else return 0;
    }

    private float jumpForce = 2f;

    public override void Jump()
    {
        rb.velocity = (Vector3.up*jumpForce);
    }

    public override void Movement()
    {
        //Getting control inputs
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

        if (IsGround)
        {
            //Walking 
            Speed = 2;
            transform.Translate(moveX * Time.deltaTime * Speed, 0f, moveY * Time.deltaTime * Speed);
            //rb.velocity = new Vector3(moveX * Time.deltaTime * Speed, 0f, moveY * Time.deltaTime * Speed);

#if UNITY_ANDROID
            if (!FJButton.IsButtonHold)
            {
                rb.velocity = Vector3.zero;
            }
#endif
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            if (Input.GetKeyUp(KeyCode.Space))
            {
                rb.velocity = Vector3.zero;
            }
#endif

        }
        else
        {
            //Flying
            Speed = 100;
            //transform.position += (cam.transform.forward * moveY * Speed * Time.deltaTime);
            //moveX != 0 ||
            if (moveY != 0)
            {
                rb.velocity = (cam.transform.forward * moveY * Speed * Time.deltaTime);
            }
            //rb.velocity = new Vector3(moveX * Time.deltaTime * Speed, 0f, 0f);
            rb.velocity += (Vector3.down * .05f);
            //rb.velocity -= (rb.velocity * .05f * Speed * Time.deltaTime);
        }


    }
}
