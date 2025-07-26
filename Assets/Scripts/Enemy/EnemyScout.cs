using UnityEngine;

public class EnemyScout : Enemy
{
    [Header("Disparo")]
    public Transform firePoint;
    private float fireCooldown = 0f;

    [Header("Comportamiento Scout")]
    public float safeDistance = 5f;            // Se aleja si está más cerca que esto
    public float maxDistance = 7f;             // Se acerca si está más lejos que esto
    public float avoidRadius = 1.5f;           // Radio para evitar otros Scouts
    public float orbitVariation = 1f;          // Aleatoriedad al orbitar

    private Vector2 orbitOffset;               // Desfase para que no orbiten todos igual

    // ----------------------------

    protected override void Start()
    {
        base.Start();
        orbitOffset = Random.insideUnitCircle * orbitVariation;
    }

    protected override void HandleMovement()
    {
        if (player == null) return;

        Vector2 toPlayer = player.position - transform.position;
        float distance = toPlayer.magnitude;
        Vector2 direction;

        if (distance < safeDistance)
        {
            // Se aleja del jugador
            direction = (transform.position - player.position).normalized;
        }
        else if (distance > maxDistance)
        {
            // Se acerca al jugador
            direction = toPlayer.normalized;
        }
        else
        {
            // Orbita al jugador con variación
            Vector2 tangent = new Vector2(-toPlayer.y, toPlayer.x).normalized;
            direction = (tangent + orbitOffset).normalized;
            TryShoot(); // Solo dispara mientras orbita
        }

        // Evita superposición con otros scouts
        direction += CalculateAvoidance();
        direction.Normalize();

        // Movimiento
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, direction * stats.moveSpeed, stats.acceleration * Time.fixedDeltaTime);

        // Rotación hacia el jugador
        float angle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), stats.rotationSpeed * Time.fixedDeltaTime);
    }

    // ----------------------------

    private void TryShoot()
    {
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            Fire();
            fireCooldown = stats.attackCooldown;
        }
    }

    private void Fire()
    {
        GameObject bullet = EnemyBulletPool.Instance.GetBullet();
        if (bullet == null) return;

        Vector2 fireDir = (player.position - firePoint.position).normalized;
        float angle = Mathf.Atan2(fireDir.y, fireDir.x) * Mathf.Rad2Deg - 90f;

        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        bullet.SetActive(true);

        bullet.GetComponent<EnemyBullet>().Fire(fireDir, (int)stats.bulletDamage, stats.bulletSpeed);
    }

    // ----------------------------

    private Vector2 CalculateAvoidance()
    {
        Vector2 avoidance = Vector2.zero;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, avoidRadius);

        foreach (var hit in hits)
        {
            if (hit != null && hit.gameObject != gameObject && hit.TryGetComponent(out EnemyScout otherScout))
            {
                Vector2 away = (Vector2)(transform.position - otherScout.transform.position);
                float dist = away.magnitude;
                if (dist > 0)
                    avoidance += away.normalized / dist;
            }
        }

        return avoidance;
    }

    // ----------------------------

    public override void ResetEnemy()
    {
        base.ResetEnemy();
        fireCooldown = 0f;
        orbitOffset = Random.insideUnitCircle * orbitVariation;
    }

    // ----------------------------

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, safeDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, avoidRadius);
    }
}
