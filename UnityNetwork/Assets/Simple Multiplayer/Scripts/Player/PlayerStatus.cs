using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class PlayerStatus : NetworkBehaviour {

    public float health;
    public bool isAlive = true;
    public bool isImmune;

    private float timeImmuneElapsed;

    public float Health { get { return health; } }
    public const float TIME_IMMUNE = 2f;

    public void Update()
    {
        if (!this.transform.GetComponent<NetworkIdentity>().hasAuthority)
        {
            return;
        }

        if (isImmune)
        {
            timeImmuneElapsed += Time.deltaTime;
        }

        if(timeImmuneElapsed >= TIME_IMMUNE)
        {
            isImmune = false;
            timeImmuneElapsed = 0;
        }

        if (!isAlive)
        {
            Reset();
            isAlive = true;
        }
    }

    //[Command]
    public void DealDamage(float health, bool isContinuous)
    {
        if (isAlive)
        {
            if (isContinuous)
            {
                this.health -= health;
            }
            else
            {
                if (!isImmune)
                {
                    this.health -= health;
                }
            }
            isImmune = true;

            if (this.health <= 0) isAlive = false;
        }
    }

    public void SetHealth(int health)
    {
        this.health = health;
    }

    public abstract void Reset();
}
