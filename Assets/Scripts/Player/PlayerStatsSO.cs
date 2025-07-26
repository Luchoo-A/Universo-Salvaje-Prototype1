using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Player/Stats")]
public class PlayerStatsSO : ScriptableObject
{
    // 🎮 MOVIMIENTO Y DEFENSA BÁSICA

    [Header("Movimiento")]
    public float moveSpeed = 5f;          // Velocidad base del jugador
    public float acceleration = 10f;      // Qué tan rápido alcanza la velocidad máxima
    public float maxSpeed = 5f;           // Límite superior de velocidad
    public float drag = 2f;               // Resistencia al movimiento cuando no se está aplicando fuerza

    [Header("Vida y Escudo")]
    public int maxHealth = 100;           // Vida máxima del jugador
    public float shieldDuration = 3f;     // Duración del escudo activado
    public float shieldCooldown = 10f;    // Tiempo que tarda en volver a estar disponible el escudo


    // 🔫 ATAQUE Y PROYECTILES BÁSICOS

    [Header("Ataque Base")]
    public float fireRate = 0.5f;         // Cadencia de disparo base
    public float bulletDamage = 15f;      // Daño base de las balas
    public float bulletSpeed = 6f;        // Velocidad base del proyectil
    public float criticalChance = 0f;     // Probabilidad de golpe crítico (0-1)
    public float criticalMultiplier = 2f; // Daño Multiplicado en Criticos (1-10)
    public float areaEffectMultiplier = 1f; // Aumenta el tamaño o daño de efectos de área como explosiones


    // 📦 STATS OFENSIVOS AVANZADOS

    [Header("Ofensivos Avanzados")]
    public int projectilePierce = 0;           // Cuántos enemigos puede atravesar una bala
    public int projectileBounce = 0;           // Cuántos rebotes puede hacer contra paredes u objetos
    public float projectileSpread = 0f;        // Ángulo de dispersión del disparo (ideal para escopetas)
    public float projectileLifetime = 3f;      // Tiempo de vida del proyectil antes de autodestruirse
    public float projectileSizeMultiplier = 1f;// Aumenta el tamaño visual de la bala
    public float damageOverTimeMultiplier = 1f;// Multiplica el daño causado por efectos como quemadura


    // 🛡️ STATS DEFENSIVOS ADICIONALES

    [Header("Defensivos Avanzados")]
    public float armor = 0f;                   // Reducción de daño fijo por impacto
    public float damageResistance = 0f;        // Reducción porcentual del daño recibido (0.25 = 25% menos)
    public float dodgeChance = 0f;             // Probabilidad de evitar completamente un golpe (0-1)


    // 🧪 STATS ESPECIALES / UTILIDAD

    [Header("Especiales / Utilidad")]
    public float xpGainMultiplier = 1f;        // Aumenta la experiencia ganada
    public float pickupRange = 1f;             // Aumenta el radio en el que recoge monedas o mejoras
    public float cooldownReduction = 0f;       // Reducción de tiempos de espera en habilidades especiales
    public float perkLuck = 0f;                // Aumenta la probabilidad de perks raros
}
