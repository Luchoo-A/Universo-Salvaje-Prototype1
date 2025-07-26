using System.Collections.Generic;
using UnityEngine;

public class PerkSelectionUI : MonoBehaviour
{
    public static PerkSelectionUI Instance { get; private set; }

    //Prefabs de Cartas por Rareza: 0 = Common, 1 = Rare, 2 = Epic
    public GameObject[] cardPrefabs;
    public Transform cardContainer;
    public PerkDatabase allPerks;

    private HashSet<PerkSO> alreadyChosenPerks = new HashSet<PerkSO>();

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
        Hide();
    }

    public void ShowRandomPerks(int count = 3)
    {
        gameObject.SetActive(true);

        foreach (Transform child in cardContainer)
            Destroy(child.gameObject);

        if (allPerks == null || allPerks.allPerks.Count == 0)
        {
            Debug.LogWarning("No hay perks disponibles en la base de datos.");
            return;
        }

        List<PerkSO> available = new List<PerkSO>(allPerks.allPerks);
        available.RemoveAll(p => p.isUnic && alreadyChosenPerks.Contains(p));

        List<PerkSO> selection = new List<PerkSO>();
        while (selection.Count < count && selection.Count < available.Count)
        {
            var candidate = GetRandomPerk(available, selection);
            if (candidate != null) selection.Add(candidate);
        }

        int[] xPositions = new int[] { -290, 0, 290 };

        for (int i = 0; i < selection.Count && i < xPositions.Length; i++)
        {
            GameObject prefab = GetPrefabByRarity(selection[i].rarity);
            GameObject card = Instantiate(prefab, cardContainer);
            card.GetComponent<PerkCardUI>().Setup(selection[i]);

            RectTransform rt = card.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(xPositions[i], 0f);
        }

        Time.timeScale = 0f;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MarkPerkAsChosen(PerkSO perk)
    {
        if (perk.isUnic)
            alreadyChosenPerks.Add(perk);
    }

    private PerkSO GetRandomPerk(List<PerkSO> pool, List<PerkSO> alreadySelected)
    {
        float totalWeight = 0f;

        foreach (var perk in pool)
        {
            if (!alreadySelected.Contains(perk))
                totalWeight += GetRarityWeight(perk.rarity);
        }

        float randomValue = Random.Range(0, totalWeight);
        float currentSum = 0f;

        foreach (var perk in pool)
        {
            if (alreadySelected.Contains(perk)) continue;

            currentSum += GetRarityWeight(perk.rarity);
            if (randomValue <= currentSum)
                return perk;
        }

        return null;
    }

    private float GetRarityWeight(PerkRarity rarity)
    {
        switch (rarity)
        {
            case PerkRarity.Common: return 60f;
            case PerkRarity.Rare: return 30f;
            case PerkRarity.Epic: return 10f;
            default: return 0f;
        }
    }
    private GameObject GetPrefabByRarity(PerkRarity rarity)
    {
        int index = (int)rarity;
        if (index >= 0 && index < cardPrefabs.Length && cardPrefabs[index] != null)
        {
            return cardPrefabs[index];
        }
        else
        {
            Debug.LogWarning($"No hay prefab asignado para la rareza {rarity}. Se usará el primero como fallback.");
            return cardPrefabs[0]; // fallback a Common
        }
    }
    
}
