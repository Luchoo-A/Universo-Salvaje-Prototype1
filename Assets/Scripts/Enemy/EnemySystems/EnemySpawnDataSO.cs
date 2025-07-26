using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnData", menuName = "Scriptables/Enemy Spawn Data")]
public class EnemySpawnDataSO : ScriptableObject
{
    public string enemyName;
    public GameObject enemyPrefab;
    public float unlockTime; // Tiempo en segundos en que este enemigo puede empezar a aparecer
    public int spawnWeight;  // Probabilidad relativa
}
