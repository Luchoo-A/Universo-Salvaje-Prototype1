using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    public static EnemyPoolManager Instance;

    public List<GameObject> enemyPrefabs;
    public int initialPoolSize = 10;

    private Dictionary<int, Queue<GameObject>> enemyPools = new Dictionary<int, Queue<GameObject>>();
    private int activeEnemies = 0;
    private int unlockedEnemyCount = 1;

    void Awake()
    {
        Instance = this;
        FillAllPools();
    }

    void FillAllPools()
    {
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            Queue<GameObject> pool = new Queue<GameObject>();
            for (int j = 0; j < initialPoolSize; j++)
            {
                GameObject enemy = Instantiate(enemyPrefabs[i]);
                enemy.SetActive(false);
                pool.Enqueue(enemy);
            }
            enemyPools.Add(i, pool);
        }
    }

public GameObject GetEnemy(int index)
{
    if (!enemyPools.ContainsKey(index))
    {
        Debug.LogError("Enemy prefab index no válido: " + index);
        return null;
    }

    Queue<GameObject> pool = enemyPools[index];
    GameObject enemy;

    if (pool.Count > 0)
    {
        enemy = pool.Dequeue();
    }
    else
    {
        enemy = Instantiate(enemyPrefabs[index]);
    }

    activeEnemies++;
    enemy.SetActive(true);

    if (enemy.TryGetComponent<Enemy>(out var baseEnemy))
    {
        baseEnemy.ResetEnemy(); // <<------ RESETEA vida, velocidad, etc.
        baseEnemy.poolIndex = index;
    }

    return enemy;
}

    public void ReturnEnemy(GameObject enemy, int index)
    {
        if (!enemyPools.ContainsKey(index))
        {
            Debug.LogError("Enemy prefab index no válido al devolver enemigo.");
            Destroy(enemy);
            return;
        }

        enemy.SetActive(false);
        enemyPools[index].Enqueue(enemy);
        activeEnemies--;
    }

    public int TotalActiveEnemies()
    {
        return activeEnemies;
    }

    public int GetUnlockedEnemyCount()
    {
        return unlockedEnemyCount;
    }

    public void UnlockNextEnemyType()
    {
        if (unlockedEnemyCount < enemyPrefabs.Count)
        {
            unlockedEnemyCount++;
            Debug.Log("Nuevo enemigo desbloqueado. Total disponibles: " + unlockedEnemyCount);
        }
    }

    public int GetEnemyIndexByName(string enemyName)
    {
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            if (enemyPrefabs[i].name.Equals(enemyName))
            {
                return i;
            }
        }

        Debug.LogWarning("EnemyPoolManager: No se encontró ningún prefab con el nombre: " + enemyName);
        return -1;
    }


}
