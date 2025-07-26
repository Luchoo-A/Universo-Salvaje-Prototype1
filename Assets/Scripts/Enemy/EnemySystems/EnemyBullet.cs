using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;
    public float damage = 5f;

    public float life = 3f;
    private Vector2 direction;

    public void Fire(Vector2 dir, int overrideDamage, float overrideSpeed)
    {
        direction = dir.normalized;

        // Si se pasa una velocidad personalizada, usarla
        if (overrideSpeed > 0f)
            speed = overrideSpeed;

        if (overrideDamage != damage)
            damage = overrideDamage;

        Invoke(nameof(Disable), life); // 🔥 Se desactiva en 5 segundos (ajustable)}
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>()?.TakeDamage((int)damage);
            gameObject.SetActive(false);
        }
    }

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
    
    void OnDisable()
    {
        CancelInvoke();
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}
