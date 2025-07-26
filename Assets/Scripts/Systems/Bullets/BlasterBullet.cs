using UnityEngine;

public class BlasterBullet : BulletBase
{
    [SerializeField] private float originalAreaRadius = 2f;
    private float areaRadius;

    [SerializeField] private float maxScaleMultiplier = 2f; // Escala máxima antes de explotar
    private Vector3 originalScale;

    private float currentLife;

    private void Awake()
    {
        originalScale = transform.localScale;
        areaRadius = originalAreaRadius;
    }

    private void OnEnable()
    {
        currentLife = life;
        transform.localScale = originalScale; // Reinicia escala al disparar
        areaRadius = originalAreaRadius;      // Reinicia radio
    }

    protected override void Update()
    {
        base.Update();

        currentLife -= Time.deltaTime;

        float t = 1f - (currentLife / life);
        float scaleFactor = Mathf.Lerp(1f, maxScaleMultiplier, t);

        // Escalar visualmente la bala
        transform.localScale = originalScale * scaleFactor;

        // Escalar el radio de explosión proporcionalmente
        areaRadius = originalAreaRadius * scaleFactor;

        // Explota automáticamente al terminar el tiempo de vida
        if (currentLife <= 0f)
        {
            Explode();
            ReturnToPool();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Asteroid"))
        {
            Explode();
            ReturnToPool();
        }
    }

    private void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, areaRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                hit.GetComponent<Enemy>()?.TakeDamage(damage, isCrit);
                ApplyEffect(hit.gameObject);
            }
            else
            {
                if (hit.CompareTag("Asteroid"))
                {
                    hit.GetComponent<AsteroidHealth>()?.TakeDamage(damage);
                }
            }
        }

        Debug.Log("💥 Blaster explosion");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, originalAreaRadius);
    }
}
