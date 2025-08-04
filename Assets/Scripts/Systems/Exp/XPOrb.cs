using Unity.VisualScripting;
using UnityEngine;

public class XPOrb : MonoBehaviour
{
    public int xpAmount = 5;

    private Transform targetPlayer;
    private bool attracted = false;
    private float attractionSpeed = 80f;

    Vector3 startPos;
    public float speed;
    public float amplitude;

    void Start()
    {
        startPos = transform.position;
    
    }


    void Update()
    {
        if (!attracted)
        {
            // Flotación vertical mientras está quieto
            transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * speed) * amplitude;

            // Pulso visual (opcional)
            //float pulse = 1f + Mathf.Sin(Time.time * 3f) * 0.05f;
            //transform.localScale = new Vector3(pulse, pulse, 1f);
        }
        else if (targetPlayer != null)
        {
            // Movimiento hacia el jugador
            Vector3 direction = (targetPlayer.position - transform.position).normalized;
            transform.position += direction * attractionSpeed * Time.deltaTime;

            if (Vector2.Distance(transform.position, targetPlayer.position) < 6.5f)
            {
                PlayerXPManager.Instance.AddXP(xpAmount);
                Destroy(gameObject); // o ReturnToPool()
            }
        }
    }


    public void AttractToPlayer(Transform player)
    {
        targetPlayer = player;
        attracted = true;

        // Opcional: resetear el startPos para evitar efectos visuales bruscos
        startPos = transform.position;
    }


    public void SetXP(int amount)
    {
        xpAmount = amount;
    }

    
    
}
