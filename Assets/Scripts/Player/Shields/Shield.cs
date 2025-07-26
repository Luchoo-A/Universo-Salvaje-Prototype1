using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private ShieldStatsSO stats;

    private float shieldPoints;
    private float currentShieldPoints;
    private bool isActive = false;

    public float TimeToActivate => stats.timeToActive;
    public bool IsActive => isActive;

    void Start()
    {
        shieldPoints = stats.BaseShieldPoints;
        currentShieldPoints = shieldPoints;
        Activate();
    }

    public void TakeDamage(int amount)
    {
        if (!isActive) return;

        currentShieldPoints -= amount;
        Debug.Log($"🛡️ Escudo recibió daño: {amount}, restante: {currentShieldPoints}");

        if (currentShieldPoints <= 0)
        {
            Deactivate();
        }
    }

    public void Activate()
    {
        isActive = true;
        currentShieldPoints = shieldPoints;
        gameObject.SetActive(true); // El escudo es su propio visual
        Debug.Log("🛡️ Escudo activado");
    }

    public void Deactivate()
    {
        isActive = false;
        gameObject.SetActive(false); // Ocultar visualmente el escudo
        Debug.Log("🛡️ Escudo destruido");
    }
}
