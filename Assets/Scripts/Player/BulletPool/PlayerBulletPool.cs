using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletPool : MonoBehaviour
{
    public static PlayerBulletPool Instance;

    [System.Serializable]
    public class BulletPoolEntry
    {
        public GameObject bulletPrefab;
        public int initialPoolSize = 20;
    }

    public List<BulletPoolEntry> bulletTypes;

    private Dictionary<GameObject, Queue<GameObject>> bulletPools = new();

    void Awake()
    {
        Instance = this;
        foreach (var entry in bulletTypes)
        {
            Queue<GameObject> pool = new Queue<GameObject>();
            for (int i = 0; i < entry.initialPoolSize; i++)
            {
                GameObject bullet = Instantiate(entry.bulletPrefab);
                bullet.SetActive(false);
                pool.Enqueue(bullet);
            }
            bulletPools[entry.bulletPrefab] = pool;
        }
    }

    public GameObject GetBullet(GameObject prefab)
    {
        if (!bulletPools.ContainsKey(prefab))
        {
            // Si no existe un pool para este prefab, crearlo dinámicamente
            bulletPools[prefab] = new Queue<GameObject>();
        }

        Queue<GameObject> pool = bulletPools[prefab];

        if (pool.Count > 0)
        {
            GameObject bullet = pool.Dequeue();
            if (bullet != null)
                return bullet;
        }

        // Si no hay en el pool, crear uno nuevo
        GameObject newBullet = Instantiate(prefab);
        return newBullet;
    }

    public void ReturnBullet(GameObject bullet, GameObject prefab)
    {
        bullet.SetActive(false);
        if (!bulletPools.ContainsKey(prefab))
        {
            bulletPools[prefab] = new Queue<GameObject>();
        }
        bulletPools[prefab].Enqueue(bullet);
    }
}
