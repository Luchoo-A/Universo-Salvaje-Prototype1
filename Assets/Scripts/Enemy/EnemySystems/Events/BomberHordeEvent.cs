using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "BomberHordeEvent", menuName = "SpawnEvents/Bomber Horde")]
public class BomberHordeEvent : ScriptableObject, ISpawnEvent
{
    public string EventName => "🚨 Horda de Bombers";
    public float Duration => 2f;

    public IEnumerator Execute(WaveManager manager)
    {
        Debug.Log(EventName);
        for (int i = 0; i < 10; i++)
        {
            manager.spawner.SpawnSpecificEnemy("Bomber");
            yield return new WaitForSeconds(0.2f);
        }
    }
}
