using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;      // La imagen con Fill


    public void UpdateUI()
    {
        if (PlayerXPManager.Instance == null) return;

        var xp = PlayerXPManager.Instance;
        
        // Calcula el fill (0 a 1)
        float progress = (float)xp.currentXP / xp.xpToNextLevel;
        fillImage.fillAmount = progress;

        
    }
}
