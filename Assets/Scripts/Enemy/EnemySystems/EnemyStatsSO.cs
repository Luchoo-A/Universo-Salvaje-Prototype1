using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Enemies/Stats")]
public class EnemyStatsSO : ScriptableObject
{
    // ❤️ VITALIDAD Y DEFENSA

    [Header("Vida y Defensa")]
    public float maxHealth = 50f;               // Vida máxima del enemigo
    public float armor = 0f;                    // Reducción fija de daño por impacto
    public float damageResistance = 0f;         // Reducción porcentual del daño recibido (0.25 = 25%)
    public float dodgeChance = 0f;              // Probabilidad de esquivar un ataque (0-1)
    public bool critImmune = false;             // Si es inmune a golpes críticos


    // 🦶 MOVIMIENTO

    [Header("Movimiento")]
    public float moveSpeed = 2f;                // Velocidad base de desplazamiento
    public float acceleration = 5f;             // Qué tan rápido acelera
    public float rotationSpeed = 180f;          // Qué tan rápido gira hacia el jugador
    public float knockbackResistance = 0f;      // Reduce empujones (0 = sin resistencia, 1 = inmune)


    // ⚔️ ATAQUE

    [Header("Ataque")]
    public float contactDamage = 10f;           // Daño al colisionar con el jugador
    public float attackCooldown = 2f;           // Tiempo entre ataques (solo si ataca a distancia o cuerpo a cuerpo)
    public float attackRange = 1.5f;            // Alcance del ataque (para enemigos cuerpo a cuerpo o torretas)
    public float bulletDamage = 10f;            // Daño de la bala
    public float bulletSpeed = 8f;              //Momiviento de la bala

    [Header("Explosión (si aplica)")]
    
    public float explosionRadius = 2f;      // Radio del empuje de explosión
    public float explosionForce = 500f;     // Fuerza con la que empuja a otros objetos
    public float explosionDelay = 0.3f;     // Tiempo antes de morir tras explotar



    // 🧠 COMPORTAMIENTO / IA

    [Header("Comportamiento")]
    public float aggressionRange = 8f;          // Distancia a la que detecta al jugador
    public bool canShoot = false;               // Si el enemigo dispara
    public bool canCharge = false;              // Si puede embestir al jugador


    // 🧪 RESISTENCIAS A EFECTOS

    [Header("Resistencias a efectos de estado")]
    public float burnResistance = 0f;           // 0 = sin resistencia, 1 = inmune
    public float slowResistance = 0f;
    public float stunResistance = 0f;


    // 💀 RECOMPENSAS AL MORIR

    [Header("Recompensas")]
    public int xpDrop = 5;                      // Cuánta experiencia da al morir
    public int coinDrop = 1;                    // Cuántas monedas suelta
    public float dropChance = 0.1f;             // Probabilidad de soltar un ítem o mejora (0.1 = 10%)
}
