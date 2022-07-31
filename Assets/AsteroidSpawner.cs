using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public int asteroidCount = 200;
    public float spawnRange = 200;
    public float spawnHeightRange = 100;
    private GameObject newAsteroid;

    void Start()
    {
        const int initialSeed = 1234;

        Random.InitState(initialSeed);
        for (int i = -asteroidCount; i < asteroidCount; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-spawnRange, spawnRange), Random.Range(-spawnHeightRange, spawnHeightRange), Random.Range(-spawnRange, spawnRange));
            newAsteroid = Instantiate(asteroidPrefab, randomPos, Quaternion.identity);
            newAsteroid.transform.localScale = Vector3.one * Random.Range(0.5f, 3.0f);
        }
    }
}
