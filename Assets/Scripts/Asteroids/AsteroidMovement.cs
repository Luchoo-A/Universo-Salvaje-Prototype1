using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AsteroidMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public bool randomDirection = true;
    public Vector2 fixedDirection = Vector2.down;

    [Header("Rotación")]
    public bool rotate = true;
    public float minRotationSpeed = 20f;
    public float maxRotationSpeed = 90f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        // Dirección
        Vector2 direction = randomDirection ? Random.insideUnitCircle.normalized : fixedDirection.normalized;
        rb.linearVelocity = direction * rb.linearVelocity.magnitude; // velocidad ya seteada

        // Rotación aleatoria
        if (rotate)
        {
            float torque = Random.Range(minRotationSpeed, maxRotationSpeed) * (Random.value > 0.5f ? 1 : -1);
            rb.angularVelocity = torque;
        }
        else
        {
            rb.angularVelocity = 0f;
        }
    }

    public void SetRandomSpeed(float speed)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        // Esto lo llama AsteroidStats después de determinar tamaño y velocidad
        Vector2 dir = randomDirection ? Random.insideUnitCircle.normalized : fixedDirection.normalized;
        rb.linearVelocity = dir * speed;
    }
}
