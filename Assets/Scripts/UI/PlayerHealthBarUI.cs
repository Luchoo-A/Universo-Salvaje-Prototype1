using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    public Image healthFill;
    public PlayerController player;

    void Update()
    {
        if (player == null || healthFill == null) return;

        float fillAmount = (float)player.GetCurrentHealth() / player.setMaxHealth();
        healthFill.fillAmount = Mathf.Clamp01(fillAmount);
    }
}

