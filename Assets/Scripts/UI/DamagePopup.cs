using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float floatSpeed = 40f;
    public float duration = 1f;

    private float timer;

    public void Setup(int damage, Color color)
    {
        text.text = damage.ToString();
        text.color = color;
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        timer += Time.deltaTime;

        if (timer >= duration)
            Destroy(gameObject);
    }
}
