using UnityEngine;

[CreateAssetMenu(fileName = "NewEffect", menuName = "RPGFari/Status_Effects/Silenced")]
public class SilencedEffectData : StatusEffectData
{
    public override StatusEffect CreateEffect()
    {
        return new SilencedEffect(duration);
    }
}
