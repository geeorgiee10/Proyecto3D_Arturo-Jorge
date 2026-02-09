using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "RPGFari/Ability")]
public class Ability : ScriptableObject
{
    public string name;
    public int damage;
    public AttackType type;
    public AttackTarget target;
    public Element element;
    public StatusEffect statusEffect;
    public QTEPattern qtePattern;
    public int cost;

    
    public void Apply(Combatant attacker, Combatant target, float multiplier)
    {
        int value = damage;

        // Multiplicador elemental
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
                // target.health = 0;
                target.dead = true;
            }
        }
        else if (damage < 0)
        {
            target.health += Mathf.RoundToInt(value * finalMultiplier);

            if (target.health > target.maxHealth)
                target.health = target.maxHealth;
        }

        if(statusEffect != null) target.AddEffect(statusEffect);
    }
}
