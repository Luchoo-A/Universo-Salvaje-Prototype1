using UnityEngine;

public class AsteroidStats : MonoBehaviour
{
    public enum AsteroidSize { Big, Small }

    public AsteroidSize size = AsteroidSize.Big;
    [Header("Vida")]
    public float bigHealth = 100f;
    public float smallHealth = 40f;
    [HideInInspector] public float currentHealth = 0f;


    [Header("Tamaño")]
    public Vector2 bigScaleRange = new Vector2(1.3f, 1.8f);
    public Vector2 smallScaleRange = new Vector2(0.6f, 0.9f);

    [Header("Velocidad")]
    public Vector2 bigSpeedRange = new Vector2(0.2f, 1f);
    public Vector2 smallSpeedRange = new Vector2(1.5f, 3f);

    [Header("Daño")]
    public int bigDamage = 30;
    public int smallDamage = 15;

    [Header("Explosión")]
    public float BigexplosionRadius = 25f;
    public float BigexplosionForce = 1000f;
    public float BigexplosionDamage = 25f;

    public float SmallexplosionRadius = 15f;
    public float SmallxplosionForce = 800f;
    public float SmallexplosionDamage = 10f;


    public void ApplyStats()
    {
        AsteroidMovement move = GetComponent<AsteroidMovement>();
        AsteroidEnemy enemy = GetComponent<AsteroidEnemy>();

        float scale = 1f;
        float speed = 1f;

        if (size == AsteroidSize.Big)
        {
            scale = Random.Range(bigScaleRange.x, bigScaleRange.y);
            speed = Random.Range(bigSpeedRange.x, bigSpeedRange.y);
            currentHealth = bigHealth;
            if (enemy)
            {
                enemy.damage = bigDamage;
                enemy.explosionRadius = BigexplosionRadius;
                enemy.explosionForce = BigexplosionForce;
                enemy.explosionDamage = BigexplosionDamage;
            } 
        }
        else
        {
            scale = Random.Range(smallScaleRange.x, smallScaleRange.y);
            speed = Random.Range(smallSpeedRange.x, smallSpeedRange.y);
            currentHealth = smallHealth;
            if (enemy)
            {
                enemy.damage = smallDamage;
                enemy.explosionRadius = SmallexplosionRadius;
                enemy.explosionForce = SmallxplosionForce;
                enemy.explosionDamage = SmallexplosionDamage;
            }
                
            
        }

        transform.localScale = new Vector3(scale, scale, 1f);
        if (move) move.SetRandomSpeed(speed);
    }
}

