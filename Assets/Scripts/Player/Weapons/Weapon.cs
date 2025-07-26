using UnityEngine;
public abstract class Weapon : MonoBehaviour
{
    public Transform[] firePoints; // Puntos de disparo, puede tener más de uno
    public float fireRate = 0.5f;  // Tasa base de fuego del arma (meta progresión)
    protected float fireCooldown;
    [SerializeField]private Animator animator;

    public virtual void UpdateWeapon()
    {
        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f)
        {
            Fire();

            float modifiedFireRate = fireRate / PlayerRuntimeStats.Instance.fireRateMultiplier;
            fireCooldown = modifiedFireRate;

            if (animator != null && !animator.GetBool("IsFiring"))
                animator.SetBool("IsFiring", true); // ▶️ Activar animación en loop
        }
        else
        {
            if (animator != null && animator.GetBool("IsFiring"))
                animator.SetBool("IsFiring", false); // ⏹️ Cortar si no está disparando
        }
    }

    protected float CalculateDamageWithCrit(float baseDamage, out bool isCritical)
    {
        float critChance = PlayerRuntimeStats.Instance.criticalChance;
        float critMultiplier = PlayerRuntimeStats.Instance.criticalMultiplier;

        isCritical = UnityEngine.Random.value < critChance;

        return isCritical ? baseDamage * critMultiplier : baseDamage;
    }



    protected abstract void Fire(); // Implementación específica en subclases
}
