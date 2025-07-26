using UnityEngine;

public class PlayerRuntimeStats : MonoBehaviour
{
    // Singleton para que cualquier objeto acceda fácilmente a los stats del jugador
    public static PlayerRuntimeStats Instance { get; private set; }

    // Referencia a los stats base (desde el ScriptableObject)
    [SerializeField] private PlayerStatsSO baseStats;

    // Valores modificables durante la partida (afectados por perks)
    [Header("Movimiento")]
    public float movementSpeed;
    public float acceleration;
    public float maxSpeed;
    public float drag;

    [Header("Vida y Escudo")]
    public int maxHealth;
    public int currentHealth;
    public float shieldDuration;
    public float shieldCooldown;

    [Header("Ataque Base")]
    public float fireRateMultiplier; // 🔁 Este modificador se aplica a las armas
    public float bulletSpeed;
    public float bulletDamage;
    public float criticalChance;
    public float criticalMultiplier;
    public float areaEffectMultiplier;

    [Header("Ataque Avanzado")]
    public int projectilePierce;
    public int projectileBounce;
    public float projectileSpread;
    public float projectileLifetime;
    public float projectileSizeMultiplier;
    public float damageOverTimeMultiplier;

    [Header("Defensivos Avanzados")]
    public float armor;
    public float damageResistance;
    public float dodgeChance;

    [Header("Especiales / Utilidad")]
    public float xpGainMultiplier;
    public float pickupRange;
    public float cooldownReduction;
    public float perkLuck;

    [Header("Desbloqueos y Mejoras")]
    public bool hasShield = false;
    public int shieldLevel = 0;
    public Weapon[] weapons; // Array que puede contener referencias a todas las armas del jugador
    public bool[] weaponUnlocked; // Para saber qué armas están desbloqueadas




    private void Awake()
    {
        // Configurar el Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        LoadBaseStats(); // Carga los valores iniciales desde el SO
        
        //Desbloquear primer Arma Base
        weaponUnlocked = new bool[weapons.Length];
        weaponUnlocked[0] = true;
    }

    public void LoadBaseStats()
    {
        if (baseStats == null)
        {
            Debug.LogError("No se asignó un PlayerStatsSO");
            return;
        }

        // Copiar todos los valores del SO a las variables de runtime
        //MOVIMIENTO Y DEFENSA BÁSICA
        movementSpeed = baseStats.moveSpeed;
        acceleration = baseStats.acceleration;
        maxSpeed = baseStats.maxSpeed;
        drag = baseStats.drag;
        //Vida y Escudo
        maxHealth = baseStats.maxHealth;
        currentHealth = maxHealth;
        shieldDuration = baseStats.shieldDuration;
        shieldCooldown = baseStats.shieldCooldown;
        // Ataque y proyectiles basicos
        fireRateMultiplier = baseStats.fireRate;
        bulletSpeed = baseStats.bulletSpeed;
        bulletDamage = baseStats.bulletDamage;
        criticalChance = baseStats.criticalChance;
        criticalMultiplier = baseStats.criticalMultiplier;
        areaEffectMultiplier = baseStats.areaEffectMultiplier;
        //Ofencivos Avanzados
        projectilePierce = baseStats.projectilePierce;
        projectileBounce = baseStats.projectileBounce;
        projectileLifetime = baseStats.projectileLifetime;
        projectileSizeMultiplier = baseStats.projectileSizeMultiplier;
        damageOverTimeMultiplier = baseStats.damageOverTimeMultiplier;
        //Defensivos Adicionales
        armor = baseStats.armor;
        damageResistance = baseStats.damageResistance;
        dodgeChance = baseStats.dodgeChance;
        //Especiales
        xpGainMultiplier = baseStats.xpGainMultiplier;
        pickupRange = baseStats.pickupRange;
        cooldownReduction = baseStats.cooldownReduction;
        perkLuck = baseStats.perkLuck;

    }



public void UnlockWeapon(int index)
{
    if (index < 0 || index >= weapons.Length) return;

    if (!weaponUnlocked[index])
    {
        weaponUnlocked[index] = true;

        // Informar al handler visual que active el arma
        PlayerWeaponHandler weaponHandler = FindAnyObjectByType<PlayerWeaponHandler>();
        if (weaponHandler != null)
            weaponHandler.UpdateWeaponsActivation();

        Debug.Log($"🔫 Arma {index} desbloqueada.");
    }
}

public void UnlockShield()
{
    if (!hasShield)
    {
        hasShield = true;
        shieldLevel = 1;

        PlayerShieldHandler shieldHandler = FindAnyObjectByType<PlayerShieldHandler>();
        if (shieldHandler != null)
            shieldHandler.ActivateShield();

        Debug.Log("🛡️ Escudo desbloqueado");
    }
}

public void UpgradeShield(int amount = 1)
{
    if (hasShield)
    {
        shieldLevel += amount;

        PlayerShieldHandler shieldHandler = FindAnyObjectByType<PlayerShieldHandler>();
        if (shieldHandler != null)
        {
            shieldHandler.UpdateShieldVisual();
            shieldHandler.ActivateShield(); // <- Agregado
        }

        Debug.Log($"🛡️ Escudo mejorado a nivel {shieldLevel}");
    }
}


}
