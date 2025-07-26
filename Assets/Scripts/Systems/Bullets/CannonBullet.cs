using UnityEngine;

public class CannonBullet : BulletBase
{
    // Al colisionar con un enemigo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Aplica daño directo
            collision.gameObject.GetComponent<Enemy>()?.TakeDamage(damage,isCrit);

            // Aplica efecto si corresponde (se define en subclases o por sistema externo)
            ApplyEffect(collision.gameObject);

            // Vuelve al pool
            ReturnToPool();
        }
        else if (collision.gameObject.CompareTag("Asteroid"))
        {
            collision.gameObject.GetComponent<AsteroidHealth>()?.TakeDamage(damage);
            ReturnToPool();
        }
    }
}