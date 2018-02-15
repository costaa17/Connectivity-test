using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnStuff : NetworkBehaviour
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
            CmdSpawn();
            timeElapsed = 0;
        }

    }

    [Command]
    private void CmdSpawn()
    {
        GameObject go = Instantiate(prefabs[Random.Range(0, prefabs.Count )], 
                                    new Vector3(Random.Range(-20, 20), 1.5f, Random.Range(-20, 20)),
                                    Quaternion.identity);
        prefabsSpawned.Add(go);
        NetworkServer.Spawn(go);
    }

}
