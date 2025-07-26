using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    [Header("Configuración de Oleadas")]
    public int maxEnemies = 20; // Límite total de enemigos activos en pantalla.

    [Tooltip("Límite de enemigos por tipo (según el nombre del prefab)")]
    public Dictionary<string, int> maxEnemiesByType = new Dictionary<string, int>
    {
        { "Bomber", 5 },
        { "Fighter", 5 },
        { "Scout", 4 }
    }; // Límite individual por tipo de enemigo (nombre limpio del prefab).

    [Tooltip("Tiempo entre spawns")]
    public float spawnInterval = 2f; // Tiempo entre cada intento de spawn.

 

    [Header("Pool de enemigos")]
    public List<GameObject> enemyPrefabs; // Lista de prefabs conectados desde el editor.

    private Dictionary<string, int> activeEnemiesByType = new Dictionary<string, int>(); // Conteo activo por tipo.

    private Camera cam;

    void Start()
    {
        cam = Camera.main; // Se guarda la referencia de la cámara principal para el cálculo del spawn fuera de pantalla.
    }


    // Intenta hacer spawn de un enemigo aleatorio si no se superaron los límites.
    private void TrySpawnEnemy()
    {
        if (GetTotalEnemiesInScene() >= maxEnemies)
            return; // Si ya hay demasiados enemigos, se cancela el spawn.

        int tries = 0;
        int maxTries = 10; // Máximo de intentos para encontrar un tipo válido.

        while (tries < maxTries)
        {
            int index = Random.Range(0, enemyPrefabs.Count); // Elige un prefab aleatorio.
            GameObject prefab = enemyPrefabs[index];
            string enemyName = CleanName(prefab.name); // Nombre sin "(Clone)"

            // Asegura que haya un contador para este tipo.
            if (!activeEnemiesByType.ContainsKey(enemyName))
                activeEnemiesByType[enemyName] = 0;

            // Verifica si puede hacer spawn de este tipo.
            if (activeEnemiesByType[enemyName] < maxEnemiesByType.GetValueOrDefault(enemyName, maxEnemies))
            {
                SpawnEnemy(index);
                break;
            }

            tries++;
        }
    }

    // Hace el spawn real de un enemigo del índice dado.
    private GameObject SpawnEnemy(int index)
    {
        GameObject prefab = enemyPrefabs[index];
        string enemyName = CleanName(prefab.name);

        // Obtiene enemigo desde el pool.
        GameObject enemy = EnemyPoolManager.Instance.GetEnemy(index);
        enemy.transform.position = GetSpawnPositionOutsideCamera(); // Coloca fuera de cámara.

        if (enemy.TryGetComponent(out Enemy baseEnemy))
        {
            baseEnemy.poolIndex = index;
            baseEnemy.SetSpawner(this); // El enemigo podrá avisar su muerte al spawner.
        }

        enemy.SetActive(true); // Se activa el enemigo.

        // Aumenta el contador de ese tipo.
        if (!activeEnemiesByType.ContainsKey(enemyName))
            activeEnemiesByType[enemyName] = 0;

        activeEnemiesByType[enemyName]++;

        return enemy;
    }

    // Notificación de que un enemigo murió.
    public void NotifyEnemyDeath(string enemyName)
    {
        enemyName = CleanName(enemyName);

        if (activeEnemiesByType.ContainsKey(enemyName))
        {
            activeEnemiesByType[enemyName]--;
            if (activeEnemiesByType[enemyName] < 0)
                activeEnemiesByType[enemyName] = 0;
        }
    }

    // Retorna el total de enemigos activos en pantalla.
    private int GetTotalEnemiesInScene()
    {
        int total = 0;
        foreach (var count in activeEnemiesByType.Values)
            total += count;
        return total;
    }

    // Calcula una posición de spawn fuera de la cámara (arriba, abajo, izquierda o derecha).
    private Vector3 GetSpawnPositionOutsideCamera()
    {
        Vector2 camPos = cam.transform.position;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        int side = Random.Range(0, 4); // 0 = arriba, 1 = abajo, 2 = izquierda, 3 = derecha.

        switch (side)
        {
            case 0: return new Vector2(Random.Range(camPos.x - width / 2, camPos.x + width / 2), camPos.y + height / 2 + 1f);
            case 1: return new Vector2(Random.Range(camPos.x - width / 2, camPos.x + width / 2), camPos.y - height / 2 - 1f);
            case 2: return new Vector2(camPos.x - width / 2 - 1f, Random.Range(camPos.y - height / 2, camPos.y + height / 2));
            case 3: return new Vector2(camPos.x + width / 2 + 1f, Random.Range(camPos.y - height / 2, camPos.y + height / 2));
        }

        return camPos;
    }

    // Limpia el nombre del prefab eliminando "(Clone)".
    private string CleanName(string original)
    {
        return original.Replace("(Clone)", "").Trim();
    }

    // Permite spawnear un enemigo específico (por nombre limpio) — usado para eventos especiales.
    public void SpawnSpecificEnemy(string enemyName)
    {
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            if (CleanName(enemyPrefabs[i].name) == enemyName)
            {
                SpawnEnemy(i);
                return;
            }
        }

        Debug.LogWarning($"❌ Enemy '{enemyName}' no encontrado en la lista de prefabs.");
    }

    // Spawnea un enemigo aleatorio (para minijefes, etc.).
    public GameObject SpawnRandomEnemy()
    {
        if (enemyPrefabs.Count == 0)
            return null;

        int index = Random.Range(0, enemyPrefabs.Count);
        return SpawnEnemy(index);
    }

    public Vector3 getRandomPositionOutSideCamera()
    {
        Vector3 Position = GetSpawnPositionOutsideCamera();

        return (Position);
    }
}
