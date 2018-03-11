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
    }

    //[Command]
    public void DealDamage(float health, bool isContinuous)
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
    }

    public void SetHealth(int health)
    {
        this.health = health;
    }
}
