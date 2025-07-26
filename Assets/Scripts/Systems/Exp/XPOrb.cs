using Unity.VisualScripting;
using UnityEngine;

public class XPOrb : MonoBehaviour
{
    public int xpAmount = 5;

    private Transform targetPlayer;
    private bool attracted = false;
    private float attractionSpeed = 80f;


    void Update()
    {
        if (attracted && targetPlayer != null)
        {
            // Movimiento hacia el jugador
            Vector3 direction = (targetPlayer.position - transform.position).normalized;
            transform.position += direction * attractionSpeed * Time.deltaTime;

            // Si está lo suficientemente cerca, se recoge
            if (Vector2.Distance(transform.position, targetPlayer.position) < 6.5f)
            {
                PlayerXPManager.Instance.AddXP(xpAmount);
                Destroy(gameObject); // O ReturnToPool
            }
        }

    }

    public void AttractToPlayer(Transform player)
    {
        targetPlayer = player;
        attracted = true;
    }

    public void SetXP(int amount)
    {
        xpAmount = amount;
    }

    
    
}
