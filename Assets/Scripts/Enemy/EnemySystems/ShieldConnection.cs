using UnityEngine;

public class ShieldConnection : MonoBehaviour
{
    private Transform origin;
    private Transform target;
    public EnemyShieldComponent targetShield { get; private set; }
    private LineRenderer lineRenderer;
    private float healPerSecond;

    public Transform TargetTransform => target;

    public void Initialize(Transform origin, Transform target, EnemyShieldComponent shield, float healPerSecond)
    {
        this.origin = origin;
        this.target = target;
        this.targetShield = shield;
        this.healPerSecond = healPerSecond;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

        if (shield != null)
            shield.ActivateShield();
    }

    public bool UpdateConnection()
    {
        if (origin == null || target == null || targetShield == null || !targetShield.gameObject.activeInHierarchy)
            return false;

        // Actualizar rayo visual
        lineRenderer.SetPosition(0, origin.position);
        lineRenderer.SetPosition(1, target.position);

        // Curar al objetivo mientras está conectado
        if (targetShield.TryGetComponent(out Enemy enemy) && enemy.IsAlive)
        {
            enemy.Heal(healPerSecond * Time.deltaTime);
        }

        return true;
    }
}
