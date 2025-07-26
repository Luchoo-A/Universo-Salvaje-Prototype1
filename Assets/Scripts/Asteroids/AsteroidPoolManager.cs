using System.Collections.Generic;
using UnityEngine;

public class AsteroidPoolManager : MonoBehaviour
{
    public static AsteroidPoolManager Instance;

    public GameObject asteroidPrefab;
    public int initialPoolSize = 15;

    private Queue<GameObject> asteroidPool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
        FillPool();
    }

    void FillPool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject asteroid = Instantiate(asteroidPrefab);
            asteroid.SetActive(false);
            asteroidPool.Enqueue(asteroid);
        }
    }

    public GameObject GetAsteroid()
    {
        if (asteroidPool.Count > 0)
        {
            GameObject asteroid = asteroidPool.Dequeue();
            asteroid.SetActive(true);
            return asteroid;
        }

        GameObject newAsteroid = Instantiate(asteroidPrefab);
        return newAsteroid;
    }

    public void ReturnAsteroid(GameObject asteroid)
    {
        asteroid.SetActive(false);
        asteroidPool.Enqueue(asteroid);
    }
}
