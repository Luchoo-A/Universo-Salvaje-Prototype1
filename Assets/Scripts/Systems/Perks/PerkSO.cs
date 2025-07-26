using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPerk", menuName = "Perks/Perk")]
public class PerkSO : ScriptableObject
{
    public string perkName;
    [TextArea] public string description;
    public Sprite icon;
    public PerkRarity rarity;
    public bool isUnic = false;
    public List<PerkModifier> modifiers;
}

[System.Serializable]
public class PerkModifier
{
    public PlayerStatType targetStat;
    public float amount; // Valor fijo o porcentaje, según el stat
}

public enum PlayerStatType
{
    MoveSpeed,
    FireRate,
    BulletDamage,
    CritChance,
    MaxHealth,
    PickupRange,
    Unlock_Shield,
    Unlock_Weapon,
    Shield_Level
}

public enum PerkRarity
{
    Common,
    Rare,
    Epic
}
