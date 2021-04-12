using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public float spawnYMax = 10;
    public float spawnYMin = -10;
    public float spawnTimeMin;
    public float spawnTimeMax = 2f;

    public Transform[] toSpawns;
    void Start()
    {
        Invoke(nameof(SpawnEntity), Random.Range(spawnTimeMin, spawnTimeMax));
    }

    void SpawnEntity()
    {
        int random = Random.Range(0, toSpawns.Length);
        Instantiate(toSpawns[random], new Vector3(transform.position.x, Random.Range(spawnYMin, spawnYMax), 0), transform.rotation);
        Invoke(nameof(SpawnEntity), Random.Range(spawnTimeMin, spawnTimeMax));
    }
    
    
}
