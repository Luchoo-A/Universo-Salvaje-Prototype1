using UnityEngine;

public class AsteroidHealth : MonoBehaviour
{
    private AsteroidStats stats;

    void Awake()
    {
        stats = GetComponent<AsteroidStats>();
    }

    public void TakeDamage(float amount)
    {
        stats.currentHealth -= amount;

        if (stats.currentHealth <= 0)
        {
            GetComponent<AsteroidEnemy>()?.Die();
        }
    }
}
