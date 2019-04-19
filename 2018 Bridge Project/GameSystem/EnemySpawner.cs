using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    float spawnTime;
    [SerializeField]
    Vector2[] spawnPos;
    ObjectPool objectPool;

    void OnEnable()
    {
        objectPool = GetComponent<ObjectPool>();
        StartCoroutine(update());
    }

    IEnumerator update()
    {
        yield return null;
        while (true)
        {
            objectPool.ObjectPoolPop(spawnPos[Random.Range(0, 6)]);
            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void SpawnTimeSetting(float value)
    {
        spawnTime = value;
    }
}
