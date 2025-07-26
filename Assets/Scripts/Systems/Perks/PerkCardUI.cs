using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PerkCardUI : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Image iconImage;
    private PerkSO perkData;

    public void Setup(PerkSO data)
    {
        perkData = data;
        titleText.text = data.perkName;
        descriptionText.text = data.description;
        iconImage.sprite = data.icon;
    }

    // Este método se llama desde el botón del prefab
    public void OnPerkSelected()
    {
        // Aplicar perk al jugador
        PerkManager.ApplyPerk(perkData);

        PerkSelectionUI.Instance.MarkPerkAsChosen(perkData); // ✅ Ahora marca perks únicos como elegidos

        // Ocultar todas las cartas (o cerrar el UI)
        FindAnyObjectByType<PerkSelectionUI>().Hide();
    }
}
