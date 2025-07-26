// Tipos posibles de efectos
public enum StatusEffectType
{
    None,   // Sin efecto
    Burn,   // Quemadura (daño en el tiempo)
    Slow,   // Ralentiza movimiento
    Stun    // Aturde al objetivo
}

[System.Serializable]
public class StatusEffect
{
    public StatusEffectType type;  // Tipo de efecto
    public float duration;         // Duración del efecto
    public float intensity;        // Intensidad (ej: daño por segundo o porcentaje de ralentización)

    public StatusEffect(StatusEffectType type, float duration, float intensity = 1f)
    {
        this.type = type;
        this.duration = duration;
        this.intensity = intensity;
    }
}
