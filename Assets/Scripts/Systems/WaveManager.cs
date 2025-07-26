using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public EnemySpawner spawner;
    public List<EnemySpawnDataSO> allEnemies;

    [Header("Duración de partida")]
    public float matchDuration = 900f;
    private float matchTimer = 0f;
    private bool matchEnded = false;

    [Header("Spawning")]
    public float spawnInterval = 2f;
    private float spawnTimer = 0f;
    private List<EnemySpawnDataSO> unlockedEnemies = new List<EnemySpawnDataSO>();

    [Header("Eventos especiales")]
    public List<ScriptableObject> spawnEventsSO; // Asignás los eventos desde el Inspector
    private List<ISpawnEvent> spawnEvents = new();
    public float eventStartTime = 60f;
    public float minTimeBetweenEvents = 30f;
    public float maxTimeBetweenEvents = 60f;
    private float nextEventTime;

    [Header("Jefe Final")]
    public GameObject bossPrefab;
    public float bossSpawnTime = 780f;

    void Start()
    {
        foreach (var e in spawnEventsSO)
        {
            if (e is ISpawnEvent se)
                spawnEvents.Add(se);
        }

        ScheduleNextEvent();
    }

    void Update()
    {
        if (matchEnded) return;

        matchTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        if (matchTimer >= matchDuration)
        {
            EndMatch();
            return;
        }

        UpdateUnlockedEnemies();

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnRandomEnemyByWeight();
        }

        if (matchTimer >= eventStartTime && matchTimer >= nextEventTime)
        {
            TriggerRandomEvent();
            ScheduleNextEvent();
        }

        if (matchTimer >= bossSpawnTime)
        {
            SpawnBoss();
        }
    }

    void UpdateUnlockedEnemies()
    {
        foreach (var enemy in allEnemies)
        {
            if (!unlockedEnemies.Contains(enemy) && matchTimer >= enemy.unlockTime)
            {
                unlockedEnemies.Add(enemy);
                Debug.Log($"🔓 Nuevo enemigo disponible: {enemy.enemyName}");
            }
        }
    }

    void SpawnRandomEnemyByWeight()
    {
        if (unlockedEnemies.Count == 0) return;

        int totalWeight = 0;
        foreach (var enemy in unlockedEnemies)
            totalWeight += enemy.spawnWeight;

        int randomValue = Random.Range(0, totalWeight);
        int currentSum = 0;

        foreach (var enemy in unlockedEnemies)
        {
            currentSum += enemy.spawnWeight;
            if (randomValue < currentSum)
            {
                spawner.SpawnSpecificEnemy(enemy.enemyName);
                return;
            }
        }
    }

    void TriggerRandomEvent()
    {
        if (spawnEvents.Count == 0) return;

        var randomEvent = spawnEvents[Random.Range(0, spawnEvents.Count)];
        Debug.Log($"🔁 Evento activado: {randomEvent.EventName}");
        StartCoroutine(randomEvent.Execute(this));
    }

    void ScheduleNextEvent()
    {
        nextEventTime = matchTimer + Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
    }

    void SpawnBoss()
    {
        if (bossPrefab == null || matchEnded) return;
        matchEnded = true;

        Debug.Log("👑 ¡El jefe ha llegado!");
        Instantiate(bossPrefab, spawner.getRandomPositionOutSideCamera(), Quaternion.identity);
    }

    void EndMatch()
    {
        matchEnded = true;
        Debug.Log("🏁 Fin de la partida.");
        spawner.enabled = false;
    }
}
