using Unity.VisualScripting;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    // Dirección de movimiento de la bala
    private Vector2 direction;

    // Velocidad de la bala
    [SerializeField] protected float speed;

    // Daño que inflige
    [SerializeField]protected float damage;

    // Tiempo de vida en segundos antes de desaparecer automáticamente
    [SerializeField]protected float life = 3f;

    // Bandera para saber si es critico
    [SerializeField] protected bool isCrit;

    // Referencia al prefab original (para devolverlo al pool)
    protected GameObject originalPrefab;

    // Método llamado al disparar la bala
    public virtual void Fire(Vector2 dir, float speed, float damage,bool isCrit)
    {
        this.direction = dir.normalized;
        this.speed = speed;
        this.damage = damage;
        this.isCrit = isCrit;

        // Programar autodesactivación
        Invoke(nameof(Disable), life);
    }

    // Asignar el prefab original al instanciar del pool
    public void SetOriginPrefab(GameObject prefab)
    {
        originalPrefab = prefab;
    }

    // Movimiento constante de la bala
    protected virtual void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    // Si sale de pantalla, volver al pool
    protected virtual void OnBecameInvisible() => ReturnToPool();

    // Cancela invocaciones programadas si se desactiva manualmente
    protected virtual void OnDisable() => CancelInvoke();

    // Método de desactivación automática (por tiempo)
    protected virtual void Disable() => ReturnToPool();

    // Devuelve la bala al pool
    protected virtual void ReturnToPool()
    {
        if (originalPrefab != null)
            PlayerBulletPool.Instance.ReturnBullet(gameObject, originalPrefab);
        else
            gameObject.SetActive(false);
    }

    // Método virtual para aplicar efectos de estado al objetivo
    protected virtual void ApplyEffect(GameObject target) {}
}
