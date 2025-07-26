using UnityEngine;
using System.Collections;

public class PlayerShieldHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] shieldLevels;
    private int currentShieldLevel = 0;
    private Coroutine rechargeCoroutine;

    public void UpdateShieldVisual()
    {
        // Desactivar todos los escudos anteriores correctamente
        foreach (var shieldGO in shieldLevels)
        {
            if (shieldGO.activeSelf)
            {
                Shield shield = shieldGO.GetComponent<Shield>();
                shield?.Deactivate(); // Lógica interna (como isActive = false)
            }

            shieldGO.SetActive(false); // Visualmente
        }

        int level = PlayerRuntimeStats.Instance.shieldLevel;

        if (level > 0 && level <= shieldLevels.Length)
        {
            shieldLevels[level - 1].SetActive(true);
            currentShieldLevel = level;
        }
        else
        {
            currentShieldLevel = 0;
        }
    }

    public void ActivateShield()
    {
        UpdateShieldVisual();
        GetActiveShield()?.Activate();
    }

    public void DeactivateShield()
    {
        GetActiveShield()?.Deactivate();

        if (rechargeCoroutine != null)
            StopCoroutine(rechargeCoroutine);

        rechargeCoroutine = StartCoroutine(RechargeShieldCoroutine());
    }

    private IEnumerator RechargeShieldCoroutine()
    {
        Shield shield = GetActiveShield();
        if (shield == null) yield break;

        float waitTime = shield.TimeToActivate;
        Debug.Log($"🕒 Esperando {waitTime} seg para reactivar escudo...");
        yield return new WaitForSeconds(waitTime);

        shield.Activate();
    }

    public Shield GetActiveShield()
    {
        int level = PlayerRuntimeStats.Instance.shieldLevel;
        if (level > 0 && level <= shieldLevels.Length)
        {
            GameObject shieldGO = shieldLevels[level - 1];
            return shieldGO.GetComponent<Shield>();
        }
        return null;
    }

    public bool ShieldIsActive()
    {
        var shield = GetActiveShield();
        return shield != null && shield.IsActive;
    }
}
