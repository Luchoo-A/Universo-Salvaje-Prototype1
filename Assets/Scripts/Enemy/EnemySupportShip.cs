using System.Collections.Generic;
using UnityEngine;

public class EnemySupportShip : Enemy
{
    [Header("Soporte")]
    public float supportRange = 6f;
    public float shieldCooldown = 5f;
    public int maxConnections = 3;
    public GameObject shieldConnectionPrefab;

    [Header("Curación en Área")]
    public float healRadius = 5f;
    public float healAmount = 2f;
    public float healInterval = 1.5f;

    [Header("Distancia de Movimiento")]
    public float safeDistanceFromPlayer = 8f;
    public float stopDistanceFromPlayer = 5f;

    private float lastShieldTime;
    private float lastHealTime;

    private List<ShieldConnection> activeConnections = new();

    protected override void Start()
    {
        base.Start();
        lastShieldTime = Time.time;
        lastHealTime = Time.time;
    }

    protected override void HandleMovement()
    {
        if (player == null) return;

        // Si tiene conexión activa, seguir al objetivo
        if (activeConnections.Count > 0 && activeConnections[0].TargetTransform != null)
        {
            MoveTowards(activeConnections[0].TargetTransform.position, 0.75f);
            RotateTowards(player.position);
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < stopDistanceFromPlayer)
        {
            // Muy cerca, alejarse
            MoveTowards(transform.position + (transform.position - player.position), 1f);
        }
        else if (distanceToPlayer > safeDistanceFromPlayer)
        {
            // Muy lejos, acercarse
            MoveTowards(player.position, 0.5f);
        }
        else
        {
            // En zona segura, buscar aliados
            GameObject ally = FindAllyWithoutShield();
            if (ally != null)
            {
                MoveTowards(ally.transform.position, 0.5f);
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }

        RotateTowards(player.position);
    }

    void MoveTowards(Vector2 target, float speedMultiplier)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * stats.moveSpeed * speedMultiplier;
    }

    void RotateTowards(Vector2 targetPos)
    {
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, stats.rotationSpeed * Time.fixedDeltaTime);
    }

    void Update()
    {
        if (!IsAlive) return;

        if (Time.time - lastShieldTime >= shieldCooldown)
        {
            TryConnectAllies();
            lastShieldTime = Time.time;
        }

        if (Time.time - lastHealTime >= healInterval)
        {
            HealNearbyAllies();
            lastHealTime = Time.time;
        }

        // Actualizar conexiones
        for (int i = activeConnections.Count - 1; i >= 0; i--)
        {
            ShieldConnection conn = activeConnections[i];
            if (conn == null || conn.TargetTransform == null || !conn.UpdateConnection() ||
                Vector2.Distance(transform.position, conn.TargetTransform.position) > supportRange)
            {
                conn?.targetShield?.DeactivateShield(); // desactiva escudo si se pierde conexión
                Destroy(conn?.gameObject);
                activeConnections.RemoveAt(i);
            }
        }
    }

    void TryConnectAllies()
    {
        if (activeConnections.Count >= maxConnections) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, supportRange, LayerMask.GetMask("Enemy"));

        foreach (Collider2D col in hits)
        {
            if (col.gameObject == gameObject) continue;

            var shield = col.GetComponent<EnemyShieldComponent>();
            if (shield == null || shield.IsShieldActive) continue;

            // Evitar conectar a un objetivo ya conectado
            bool alreadyConnected = activeConnections.Exists(conn => conn.TargetTransform == col.transform);
            if (alreadyConnected) continue;

            shield.ActivateShield();

            GameObject connObj = Instantiate(shieldConnectionPrefab, transform.position, Quaternion.identity);
            ShieldConnection connection = connObj.GetComponent<ShieldConnection>();
            connection.Initialize(transform, col.transform, shield, healAmount);

            activeConnections.Add(connection);

            if (activeConnections.Count >= maxConnections) break;
        }
    }

    GameObject FindAllyWithoutShield()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, supportRange, LayerMask.GetMask("Enemy"));
        foreach (Collider2D col in hits)
        {
            if (col.gameObject == gameObject) continue;

            var shield = col.GetComponent<EnemyShieldComponent>();
            bool alreadyConnected = activeConnections.Exists(conn => conn.TargetTransform == col.transform);

            if (shield != null && !shield.IsShieldActive && !alreadyConnected)
                return col.gameObject;
        }
        return null;
    }

    void HealNearbyAllies()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, healRadius, LayerMask.GetMask("Enemy"));
        foreach (Collider2D col in hits)
        {
            if (col.gameObject == gameObject) continue;

            var ally = col.GetComponent<Enemy>();
            if (ally != null && ally.IsAlive)
            {
                ally.Heal(healAmount);
            }
        }
    }

    public override void ResetEnemy()
    {
        base.ResetEnemy();
        lastShieldTime = Time.time;
        lastHealTime = Time.time;

        foreach (var conn in activeConnections)
        {
            if (conn != null)
            {
                conn.targetShield?.DeactivateShield();
                Destroy(conn.gameObject);
            }
        }
        activeConnections.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, supportRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, healRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopDistanceFromPlayer);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, safeDistanceFromPlayer);
    }
}
