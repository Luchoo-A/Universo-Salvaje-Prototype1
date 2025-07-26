using System.Collections;
using UnityEngine;

public class AutoCannon : Weapon
{
    public float attack;  // daño base (meta‑progresión)
    public float speed;   // velocidad base (meta‑progresión)

    [SerializeField] private GameObject Bullet_Cannon;

    protected override void Fire()
    {
        if (firePoints == null || firePoints.Length != 2)
        {
            Debug.LogWarning("AutoCannon: Necesita exactamente 2 firePoints.");
            return;
        }

        StartCoroutine(FireWithDelay());
    }

    private IEnumerator FireWithDelay()
    {
        for (int i = 0; i < 2; i++)
        {
            Transform firePoint = firePoints[i];

            GameObject bullet = PlayerBulletPool.Instance.GetBullet(Bullet_Cannon);
            if (bullet != null)
            {
                // Dirección hacia el mouse
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 fireDir = (mouseWorldPos - firePoint.position).normalized;
                float angle = Mathf.Atan2(fireDir.y, fireDir.x) * Mathf.Rad2Deg - 90f;

                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
                bullet.SetActive(true);

                // --- Cálculo de stats finales ---
                float finalSpeed   = speed + PlayerRuntimeStats.Instance.bulletSpeed;

                float baseDamage   = attack * (1f + PlayerRuntimeStats.Instance.bulletDamage / 100f);

                bool isCrit;
                float finalDamage  = CalculateDamageWithCrit(baseDamage, out isCrit);

                // Disparo
                bullet.GetComponent<CannonBullet>().Fire(fireDir, finalSpeed, finalDamage, isCrit);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
