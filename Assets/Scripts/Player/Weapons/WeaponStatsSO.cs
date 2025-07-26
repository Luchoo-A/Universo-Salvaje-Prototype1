using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponStats", menuName = "Weapons/Stats")]
public class WeaponStatsSO : ScriptableObject
{
    [Header("Identificación")]
    public string weaponName = "Nueva Arma";   // Nombre del arma (para mostrar en UI, upgrades, etc.)

    // 🔫 Atributos base del arma
    [Header("Atributos base")]
    public float baseFireRate = 1f;            // Tiempo entre disparos (menor = dispara más rápido)
    public float baseDamage = 10f;             // Daño base por disparo
    public float baseBulletSpeed = 8f;         // Velocidad de las balas disparadas
    public float baseProjectileLifetime = 3f;  // Tiempo que la bala dura activa

    // 📦 Comportamiento de proyectiles
    [Header("Proyectil")]
    public int pierceCount = 0;                // Cuántos enemigos puede atravesar
    public int bounceCount = 0;                // Cuántos rebotes puede hacer contra paredes
    public float spreadAngle = 0f;             // Ángulo de dispersión entre balas (para armas tipo escopeta)
    public float projectileSizeMultiplier = 1f;// Escala visual del proyectil
    public bool canCrit = true;                // Si esta arma puede aplicar golpes críticos

    // 💥 Especiales
    [Header("Especiales")]
    public bool hasAreaEffect = false;         // Si aplica daño en área
    public float areaRadius = 0f;              // Radio del daño en área (si aplica)
    public bool canApplyStatusEffect = false;  // Si puede aplicar efectos de estado
    public StatusEffectType statusEffect = StatusEffectType.None; // Tipo de efecto
    public float statusDuration = 0f;          // Duración del efecto de estado
    public float statusIntensity = 0f;         // Intensidad del efecto (ej: daño por segundo o slow %)
}
