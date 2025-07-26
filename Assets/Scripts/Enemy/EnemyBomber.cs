using System.Collections;
using UnityEngine;

public class EnemyBomber : Enemy
{
    [Header("Explosión")]
    private float explosionRadius => stats.explosionRadius;
    private float explosionForce => stats.explosionForce;
    private float explosionDelay => stats.explosionDelay;

    [Header("Carga")]
    public float chargeDistance = 2f;
    public float chargeForce = 10f;
    public float chargeDuration = 0.4f;
    public float chargeCooldown = 1f;
    private float chargeTimer = 0f;

    public float radiousforExplosion = 2f;




    private bool exploded = false;
    private bool charging = false;
    private bool selfDestructing = false;

    protected override void HandleMovement()
    {
        if (player == null || exploded) return;

        Vector2 toPlayer = player.position - transform.position;
        float distance = toPlayer.magnitude;
        Vector2 dir = toPlayer.normalized;
        chargeTimer = Mathf.Max(0, chargeTimer - Time.fixedDeltaTime);

        // 🚀 Carga si está cerca

        if (!charging && distance <= chargeDistance && chargeTimer == 0)
        {
            StartCoroutine(ChargeTowardsPlayer(dir));
        }



        // Movimiento normal si no está cargando
        if (!charging)
        {
            rb.linearVelocity = dir * stats.moveSpeed;
        }

        // 💣 Activa autodestrucción si entra al rango de explosión
        if (!selfDestructing && distance <= radiousforExplosion)
        {
            StartCoroutine(ExplosionSequence());
        }

        // 🔁 Rotación hacia el jugador
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.Euler(0, 0, angle),
            stats.rotationSpeed * Time.fixedDeltaTime
        );
    }

    IEnumerator ChargeTowardsPlayer(Vector2 dir)
    {
        charging = true;
        rb.linearVelocity = dir * chargeForce;
        yield return new WaitForSeconds(chargeDuration);
        charging = false;
        chargeTimer = chargeCooldown;
    }

    IEnumerator ExplosionSequence()
    {
        selfDestructing = true;
        rb.linearVelocity = Vector2.zero;

        // Parpadeo previo
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            float t = 0f;
            float blinkSpeed = 20f;

            while (t < explosionDelay)
            {
                float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);
                Color c = sr.color;
                c.a = Mathf.Clamp01(alpha);
                sr.color = c;

                t += Time.deltaTime;
                yield return null;
            }

            // Restaurar color
            Color restore = sr.color;
            restore.a = 1f;
            sr.color = restore;
        }
        else
        {
            yield return new WaitForSeconds(explosionDelay);
        }

        // Explosión en área
        var hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var h in hits)
        {
            Rigidbody2D hitRb = h.GetComponent<Rigidbody2D>();
            if (hitRb != null && hitRb != rb)
            {
                Vector2 forceDir = hitRb.position - rb.position;
                hitRb.AddForce(forceDir.normalized * explosionForce);
            }

            if (h.CompareTag("Player"))
            {
                h.GetComponent<PlayerController>()?.TakeDamage((int)stats.contactDamage);
            }
        }

        Die();
    }

    public override void ResetEnemy()
    {
        base.ResetEnemy();
        exploded = false;
        charging = false;
        selfDestructing = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, chargeDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radiousforExplosion);
    }
}
