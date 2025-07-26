using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PerkDatabase", menuName = "Perks/Perk Database")]
public class PerkDatabase : ScriptableObject
{
    public List<PerkSO> allPerks;
}
