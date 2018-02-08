using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStuff : MonoBehaviour
{

    public List<GameObject> prefabs;
    public const float TIME_SPAWN = 5f;
    private List<GameObject> prefabsSpawned;

    private float timeElapsed;

    void Start()
    {
        prefabsSpawned = new List<GameObject>();
    }


    void Update()
    {
        timeElapsed += Time.deltaTime;
        if(timeElapsed >= TIME_SPAWN)
        {
            Spawn();
            timeElapsed = 0;
        }

    }

    private void Spawn()
    {
        GameObject go = Instantiate(prefabs[Random.Range(0, prefabs.Count )], 
                                    new Vector3(Random.Range(-20, 20), 1.5f, Random.Range(-20, 20)),
                                    Quaternion.identity);
        prefabsSpawned.Add(go);
    }

}
