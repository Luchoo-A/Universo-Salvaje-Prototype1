using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerRuntimeStats stats;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("Componentes")]
    private PlayerWeaponHandler weaponHandler;
    private PlayerShieldHandler shieldHandler;

    [SerializeField] private Transform enginesParent;
    private Animator currentEngineAnimator;

    private float currentHealth;
    private float maxHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = PlayerRuntimeStats.Instance;

        weaponHandler = GetComponent<PlayerWeaponHandler>();
        shieldHandler = GetComponent<PlayerShieldHandler>();

        currentHealth = stats.maxHealth;
        maxHealth = stats.maxHealth;
        rb.linearDamping = stats.drag;

        foreach (Transform engine in enginesParent)
        {
            if (engine.gameObject.activeSelf)
            {
                currentEngineAnimator = engine.GetComponentInChildren<Animator>();
                break;
            }
        }
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        if (currentEngineAnimator != null)
        {
            bool isMoving = moveInput.magnitude > 0.1f;
            currentEngineAnimator.SetBool("IsMoving", isMoving);
        }

        RotateTowardsMouse();

        // Lógica del arma se delega
        weaponHandler?.HandleWeapons();
    }

    void FixedUpdate()
    {
        rb.AddForce(moveInput * stats.acceleration);
        if (rb.linearVelocity.magnitude > stats.maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * stats.maxSpeed;
    }

    private void RotateTowardsMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }
    public void TakeDamage(int amount)
    {
        Shield activeShield = shieldHandler.GetActiveShield();
        if (activeShield != null && activeShield.IsActive)
        {
            activeShield.TakeDamage(amount);

            if (!activeShield.IsActive)
            {
                // Si se rompió justo ahora, empezamos la recarga
                shieldHandler.DeactivateShield();
            }

            return;
        }

        // Si no hay escudo o ya está roto
        currentHealth -= amount;
        Debug.Log($"Player recibió daño. HP actual: {currentHealth}");
        GetComponent<FlashEffect>()?.Flash();

        if (currentHealth <= 0)
        {
            Die();
        }
    }




    public void Die()
    {
        Debug.Log("Jugador murió");
        gameObject.SetActive(false);
        Time.timeScale = 0f;
    }

    public float GetCurrentHealth() => currentHealth;
    public float setMaxHealth() => maxHealth;
}
