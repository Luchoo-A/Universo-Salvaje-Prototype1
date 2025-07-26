using UnityEngine;


public class Blaster : Weapon

{
    public float attack; // daño base del arma (meta progresión)
    public float speed;  // velocidad base del proyectil (meta progresión)

    [SerializeField] private GameObject Bullet_Blaster;
    private int currentFirePoint = 0;

    protected override void Fire()
    {
        if (firePoints == null || firePoints.Length == 0)
        {
            Debug.LogWarning("Blaster: No hay firePoints asignados.");
            return;
        }

        Transform firePoint = firePoints[currentFirePoint];

        // Obtener una bala desde el pool
        GameObject bullet = PlayerBulletPool.Instance.GetBullet(Bullet_Blaster);
        if (bullet != null)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 fireDirection = (mouseWorldPos - firePoint.position).normalized;
            //ROTAR BALA AL MOUSE
            float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg - 90f;

            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
            bullet.SetActive(true);

            // 🔁 Aplica los stats del jugador al proyectil
            float finalSpeed   = speed + PlayerRuntimeStats.Instance.bulletSpeed;

            float baseDamage   = attack * (1f + PlayerRuntimeStats.Instance.bulletDamage / 100f);

            bool isCrit;
            float finalDamage  = CalculateDamageWithCrit(baseDamage, out isCrit);

            bullet.GetComponent<BlasterBullet>().Fire(fireDirection, finalSpeed, finalDamage,isCrit);
        }

        currentFirePoint = (currentFirePoint + 1) % firePoints.Length;
    }
}