using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniBossEvent", menuName = "SpawnEvents/Mini Boss")]
public class MiniBossEvent : ScriptableObject, ISpawnEvent
{
    public string EventName => "⚠ MiniBoss detectado";
    public float Duration => 1f;

    public IEnumerator Execute(WaveManager manager)
    {
        Debug.Log(EventName);

        GameObject boss = manager.spawner.SpawnRandomEnemy();
        if (boss == null)
        {
            Debug.LogWarning("❌ No se pudo spawnear el miniboss.");
            yield break;
        }

        Enemy enemyScript = boss.GetComponent<Enemy>();
        if (enemyScript == null)
        {
            Debug.LogWarning("❌ El miniboss no tiene componente Enemy.");
            yield break;
        }

        // 🧬 Clonar y modificar stats
        EnemyStatsSO boostedStats = Instantiate(enemyScript.stats);
        boostedStats.maxHealth *= 3f;
        boostedStats.moveSpeed *= 0.8f;
        if (!boostedStats.critImmune) boostedStats.critImmune = true;

        // 🔫 Si tiene comportamiento de ataque (ej: Fighter)
        var shooter = boss.GetComponent<EnemyFighter>();
        if (shooter != null)
        {
            boostedStats.contactDamage *= 1f;
            boostedStats.attackCooldown *= 0.8f;
        }

        enemyScript.stats = boostedStats;
        boss.transform.localScale *= 1.5f;

        // 🟥 Efecto visual (parpadeo rojo usando FlashEffect)
        FlashEffect flash = boss.GetComponent<FlashEffect>();
        if (flash != null)
        {
            manager.StartCoroutine(FlashMiniBossTint(flash,enemyScript));
        }
        else
        {
            Debug.LogWarning("❌ MiniBoss no tiene FlashEffect para el flash visual.");
        }

        yield return null;
    }

private IEnumerator FlashMiniBossTint(FlashEffect flash, Enemy enemy)
{
    float speed = 2f;

    while (enemy != null && enemy.IsAlive) // 👈 Mientras esté vivo
    {
        float lerp = Mathf.PingPong(Time.time * speed, 1f);
        Color tintColor = Color.Lerp(Color.white, Color.red, lerp);
        flash.SetTint(tintColor);

        yield return null;
    }

    if (flash != null)
        flash.ResetTint();
}

}
