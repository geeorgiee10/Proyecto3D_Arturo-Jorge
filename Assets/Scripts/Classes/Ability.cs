using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewAbility", menuName = "RPGFari/Ability")]
public class Ability : ScriptableObject
{
    public string name;
    public string description;
    public int damage;
    public AttackType type;
    public AttackTarget target;
    public Element element;
    public StatusEffect statusEffect;
    public QTEPattern qtePattern;
    public int cost;
    private Dictionary<Effect, string> effectColors = new Dictionary<Effect, string>()
    {
        { Effect.OutOfTempo, "#dfa945" },
        { Effect.OutOfTune, "#9b569b" },
        { Effect.Microtone, "#672090" },
        { Effect.Silence, "#808080" },
        { Effect.PerfectTempo, "#2056ba" },
        { Effect.Tuned, "#64a964" },
        { Effect.Extasis, "#cc2d6f" },
        { Effect.Groove, "#2eb1b1" },
        //HAY QUE PONER QUE SI NO HAY EFECTO; QUE NO DÉ ERROR
    };
    
    public void Apply(Combatant attacker, Combatant target, float multiplier)
    {
        int value = damage;

        float elementMultiplier =
            ElementChart.GetMultiplier(element, target.element);

        float finalMultiplier = multiplier * elementMultiplier;

        if (damage > 0)
        {
            value += attacker.strength;

            if (attacker.weapon != null)
                value += attacker.weapon.damage;

            target.health -= Mathf.RoundToInt(value * finalMultiplier);

            if (target.health <= 0)
            {
                target.health = 0;
                target.dead = true;
            }
        }
        else if (damage < 0)
        {
            target.health += Mathf.RoundToInt(value * finalMultiplier);

            if (target.health > target.maxHealth)
                target.health = target.maxHealth;
        }

        if(statusEffect != null){
            target.AddEffect(statusEffect);
        }
    }

    #region Texto

        public string GetColorEffect()
        {
            return $"<color={effectColors[statusEffect.effect]}><b>{GetEffectName()}</b></color>";
        }
        public string GetColorElement()
        {
            return $"<color={GetElementColor()}><b>{GetElementName()}</b></color>";
        }

        public string GetEffectName()
        {
            return statusEffect.effect switch
            {
                Effect.Extasis => "Éxtasis",
                Effect.Groove => "Groove",
                Effect.Microtone => "Microtono",
                Effect.OutOfTempo => "Fuera de Tempo",
                Effect.OutOfTune => "Desafinación",
                Effect.PerfectTempo => "Tempo Perfecto",
                Effect.Silence => "Silencio",
                Effect.Tuned => "Afinación",
                _ => "?"
            };
        }

        public string GetElementName()
        {
            return element switch
            {
                Element.Harmony => "Armonía",
                Element.Melody => "Melodía",
                Element.Rythm => "Ritmo",
                Element.Timbre => "Timbre",
                _ => "?"
            };
        }

        string GetElementColor()
        {
            return element switch
            {
                Element.Harmony => "#5C6BC0",
                Element.Melody => "#B86BFF",
                Element.Rythm => "#FF7043",
                Element.Timbre => "#4DB6AC",
                _ => "#FFFFFF"
            };
        }

        public string GetFormattedDescription()
        {
            return description.Replace("{EFFECT}", GetColorEffect()).Replace("{ELEMENT}", GetColorElement());
        }

    #endregion

}
