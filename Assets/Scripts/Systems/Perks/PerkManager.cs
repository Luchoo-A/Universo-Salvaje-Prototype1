using UnityEngine;

public class PerkManager : MonoBehaviour
{
    public static PerkManager Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

public static void ApplyPerk(PerkSO perk)
    {
        var stats = PlayerRuntimeStats.Instance;

        foreach (var mod in perk.modifiers)
        {
            switch (mod.targetStat)
            {
                case PlayerStatType.MoveSpeed:
                    stats.movementSpeed += mod.amount;
                    break;

                case PlayerStatType.FireRate:
                    stats.fireRateMultiplier =
                    stats.fireRateMultiplier = stats.fireRateMultiplier * (1f + mod.amount / 100f);
                    break;

                case PlayerStatType.BulletDamage:
                    stats.bulletDamage += mod.amount;
                    break;

                case PlayerStatType.CritChance:
                    stats.criticalChance += mod.amount;
                    break;

                case PlayerStatType.MaxHealth:
                    stats.maxHealth += (int)mod.amount;
                    break;
                
                case PlayerStatType.PickupRange:
                    stats.pickupRange += (int)mod.amount;
                    break;

                case PlayerStatType.Unlock_Shield:
                    PlayerRuntimeStats.Instance.UnlockShield();
                    break;

                case PlayerStatType.Shield_Level:
                    PlayerRuntimeStats.Instance.UpgradeShield((int)mod.amount);
                    break;

                case PlayerStatType.Unlock_Weapon:
                    PlayerRuntimeStats.Instance.UnlockWeapon((int)mod.amount);
                    break;
    
        
            }
        }

        Debug.Log($"🔧 Perk aplicado: {perk.perkName}");
    }
}
