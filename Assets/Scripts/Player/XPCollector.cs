using UnityEngine;

public class XPCollector : MonoBehaviour
{
    public LayerMask xpLayer;
    public float RangeCollector=1;

    void Start()
    {
        RangeCollector = PlayerRuntimeStats.Instance.pickupRange;
    }
    void FixedUpdate()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, RangeCollector,xpLayer);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("XPOrb"))
            {
                XPOrb orb = hit.GetComponent<XPOrb>();
                if (orb != null)
                {
                    orb.AttractToPlayer(transform);
                }
                Debug.Log("Detectado orb: " + hit.name);
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, RangeCollector);
    }
}
