using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class PlayerAttack : NetworkBehaviour {


    public abstract void Attack(GameObject other);
}
