using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public EnemyStatsSO stats; // 🧠 ScriptableObject con todos los valores configurables

    protected Rigidbody2D rb;
    protected Transform player;

    [Header("Efectos")]
    public GameObject explosionEffect;

    [Header("Exp")]
    public GameObject xpOrbPrefab;
    [Header("Pool")]
    public int poolIndex;

    protected float currentHealth;
    public bool IsAlive => currentHealth > 0;

    // Referencia al spawner que creó este enemigo
    private EnemySpawner spawner;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        currentHealth = stats.maxHealth;
    }

    // Método para que el spawner asigne su referencia
    public void SetSpawner(EnemySpawner spawner)
    {
        this.spawner = spawner;
    }

    public virtual void TakeDamage(float amount, bool isCrit)
    {
        // 💢 Esquiva si tiene dodgeChance
        if (UnityEngine.Random.value < stats.dodgeChance)
        {
            Debug.Log("🌀 Enemigo esquivó el daño");
            return;
        }

        // 🛡️ Aplica reducción de daño por armadura y resistencia
        float reduced = Mathf.Max(amount - stats.armor, 0f);
        float finalDamage = reduced * (1f - stats.damageResistance);

        currentHealth -= finalDamage;

        GetComponent<FlashEffect>()?.Flash();
        DamagePopupSpawner.Instance.ShowDamage(transform.position, Mathf.RoundToInt(finalDamage), isCrit);

        if (currentHealth <= 0) Die();
    }

    public virtual void ResetEnemy()
    {
        currentHealth = stats.maxHealth;
        rb.linearVelocity = Vector2.zero;
        GetComponent<FlashEffect>()?.ResetFlash();
    }

    public virtual void Die()
    {
        rb.linearVelocity = Vector2.zero;

        // 🎇 Instanciar explosión
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // 💠 Soltar XP
        if (xpOrbPrefab != null)
        {
            GameObject orb = Instantiate(xpOrbPrefab, transform.position, Quaternion.identity);
            orb.GetComponent<XPOrb>().SetXP(stats.xpDrop);
        }

        // Drop de recompensa (items, monedas, etc.)
        if (UnityEngine.Random.value < stats.dropChance)
        {
            Debug.Log("Enemigo soltó un ítem");
        }

        // Notificar al spawner que este enemigo murió
        if (spawner != null)
        {
            spawner.NotifyEnemyDeath(gameObject.name);
        }

        EnemyPoolManager.Instance.ReturnEnemy(gameObject, poolIndex);
    }

    //Metodo para curar Enemigos
    public virtual void Heal(float amount)
    {
        if (!IsAlive) return;

        currentHealth = Mathf.Min(currentHealth + amount, stats.maxHealth);
        // Aquí podrías agregar un pequeño efecto visual
    }



    //Metodo de movimiento de cada Enemigo
    protected abstract void HandleMovement();

    void FixedUpdate() => HandleMovement();
}
