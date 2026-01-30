using UnityEngine;

public abstract class StatusEffectData : ScriptableObject
{
    public int duration;

    public abstract StatusEffect CreateEffect();
}
