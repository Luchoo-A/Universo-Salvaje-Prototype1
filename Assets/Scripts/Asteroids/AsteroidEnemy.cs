using UnityEngine;

public class AsteroidEnemy : MonoBehaviour
{
    [Header("Explosión")]
    public float explosionRadius = 2f;
    public float explosionForce = 300f;
    public float explosionDamage = 15f;

    public int damage = 20;
    public int splitCount = 2;


    private Animator animator;
    private bool isDying = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        isDying = false;
        GetComponent<AsteroidStats>()?.ApplyStats();
        animator.Play("Idle");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>()?.TakeDamage(damage);
            Die();
        }
    }

    public void Die()
    {
        if (isDying) return;
        isDying = true;

        if (animator != null)
        {
            animator.Play("Destroy");
        }
        else
        {
            FinishDeath();
        }
    }

    // Este método se llama al final de la animación (Animation Event)
    public void OnExplosionFinished()
    {
        FinishDeath();
    }

    private void FinishDeath()
{
    // ⚡ Explosión en área
    var hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
    foreach (var h in hits)
    {
        if (h.gameObject.CompareTag("Player"))
        {
            h.GetComponent<PlayerController>()?.TakeDamage((int)explosionDamage);
        }
        if (h.gameObject.CompareTag("Enemy"))
        {
            h.GetComponent<Enemy>()?.TakeDamage(explosionDamage,false);
        }


        Rigidbody2D hitRb = h.GetComponent<Rigidbody2D>();
        if (hitRb != null && hitRb != GetComponent<Rigidbody2D>())
        {
            Vector2 forceDir = hitRb.position - (Vector2)transform.position;
            hitRb.AddForce(forceDir.normalized * explosionForce);
        }
    }

    // 💥 Split si es grande
    var stats = GetComponent<AsteroidStats>();
    if (stats != null && stats.size == AsteroidStats.AsteroidSize.Big)
    {
        for (int i = 0; i < splitCount; i++)
        {
            GameObject newAsteroid = AsteroidPoolManager.Instance.GetAsteroid();
            newAsteroid.transform.position = transform.position;

            AsteroidStats newStats = newAsteroid.GetComponent<AsteroidStats>();
            if (newStats != null)
            {
                newStats.size = AsteroidStats.AsteroidSize.Small;
                newStats.ApplyStats();
            }
        }
    }

    // ♻️ Volver al pool
    AsteroidPoolManager.Instance.ReturnAsteroid(gameObject);
}

void OnDrawGizmosSelected()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, explosionRadius);
}


}

