using UnityEngine;

public class EnemyFighter : Enemy
{
    public Transform firePoint;
    private float shootTimer = 0f;

    protected override void HandleMovement()
    {
        if (player == null) return;

        // Se lanza directamente hacia el jugador sin suavizado
        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * stats.moveSpeed;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.Euler(0, 0, angle),
            stats.rotationSpeed * Time.fixedDeltaTime
        );
    }

    void Update()
    {
        if (!stats.canShoot || player == null) return;

        shootTimer += Time.deltaTime;
        if (shootTimer >= stats.attackCooldown)
        {
            Fire();
            shootTimer = 0f;
        }
    }

    void Fire()
    {
        GameObject bullet = EnemyBulletPool.Instance.GetBullet();
        if (bullet == null) return;

        Vector2 fireDirection = (player.position - firePoint.position).normalized;
        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg - 90f;

        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        bullet.SetActive(true);

        bullet.GetComponent<EnemyBullet>().Fire(fireDirection, (int)stats.bulletDamage, stats.bulletSpeed);
    }
}
