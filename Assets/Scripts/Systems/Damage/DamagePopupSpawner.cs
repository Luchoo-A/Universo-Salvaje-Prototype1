using UnityEngine;

public class DamagePopupSpawner : MonoBehaviour
{
    public static DamagePopupSpawner Instance;

    public GameObject popupPrefab;
    public Canvas canvas;

    void Awake()
    {
        Instance = this;
    }

    public void ShowDamage(Vector3 worldPos, int damage, bool isCrit)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        GameObject popup = Instantiate(popupPrefab, screenPos, Quaternion.identity, canvas.transform);

        if (isCrit == false)
        {
            popup.GetComponent<DamagePopup>().Setup(damage, Color.white);
        }
        else
        {
            popup.GetComponent<DamagePopup>().Setup(damage,Color.red);
        }
        
        
    }
}
