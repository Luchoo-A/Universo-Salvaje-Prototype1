using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    [SerializeField] private Weapon[] weapons; // Todas las armas disponibles (asignadas en inspector)
    private List<Weapon> activeWeapons = new List<Weapon>();

    void Start()
    {
        // Al iniciar, activamos solo las armas desbloqueadas en PlayerRuntimeStats
        UpdateWeaponsActivation();
    }

    public void HandleWeapons()
    {
        foreach (var weapon in activeWeapons)
        {
            weapon.UpdateWeapon();
        }
    }

    // Activa o desactiva armas según si están desbloqueadas
    public void UpdateWeaponsActivation()
    {
        activeWeapons.Clear();

        for (int i = 0; i < weapons.Length; i++)
        {
            if (PlayerRuntimeStats.Instance.weaponUnlocked[i])
            {
                weapons[i].gameObject.SetActive(true);
                activeWeapons.Add(weapons[i]);
            }
            else
            {
                weapons[i].gameObject.SetActive(false);
            }
        }
    }

    // Para desbloquear un arma desde perks o runtime
    public void UnlockWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length) return;

        if (!PlayerRuntimeStats.Instance.weaponUnlocked[index])
        {
            PlayerRuntimeStats.Instance.weaponUnlocked[index] = true;
            // Actualizamos visualmente y funcionalmente
            UpdateWeaponsActivation();
        }
    }

    // Si querés, podés agregar método para desactivar un arma también
    public void LockWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length) return;

        if (PlayerRuntimeStats.Instance.weaponUnlocked[index])
        {
            PlayerRuntimeStats.Instance.weaponUnlocked[index] = false;
            UpdateWeaponsActivation();
        }
    }
}
