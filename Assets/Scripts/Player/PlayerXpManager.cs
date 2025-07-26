using UnityEngine;

public class PlayerXPManager : MonoBehaviour
{
    public static PlayerXPManager Instance;
    public XPBarUI xpBarUI; // Asignalo desde el inspector

    [Header("XP Configuración")]
    public int currentXP = 0;
    public int level = 1;
    public int xpToNextLevel = 100;
    public float xpGrowthRate = 1.25f; // Cada nivel requiere más XP

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        xpBarUI.UpdateUI();
    }

    void Update()
{
    if (Input.GetKeyDown(KeyCode.X))
    {
        AddXP(10); // Aumenta 10 XP al presionar "X"
        Debug.Log("Gané 10 XP");
    }
}

    public void AddXP(int amount)
    {
        int finalAmount = Mathf.RoundToInt(amount * PlayerRuntimeStats.Instance.xpGainMultiplier);
        currentXP += finalAmount;

        if (currentXP >= xpToNextLevel)
            LevelUp();

        xpBarUI?.UpdateUI();
    }

void LevelUp()
{
    level++;
    currentXP -= xpToNextLevel;
    xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpGrowthRate);

    xpBarUI?.UpdateUI();
    PerkSelectionUI.Instance.ShowRandomPerks();
}
}
