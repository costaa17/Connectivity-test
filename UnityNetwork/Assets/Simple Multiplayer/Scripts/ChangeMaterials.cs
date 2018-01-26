using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterials : MonoBehaviour {

    public Material[] materials;

    private Renderer rend;

	void Start () {
        rend = this.gameObject.GetComponent<Renderer>();
        int rand = Random.Range(0, materials.Length);
        rend.sharedMaterial = materials[rand];
	}
	
}
