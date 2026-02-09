using UnityEngine;
using System.Collections.Generic;

public class Combatant : MonoBehaviour
{
    public string name;
    public int maxHealth;
    public int health;
    public int strength;
    public int chant;
    public int speed;
    public int initiative;
    public int abilityPoints;

    public bool isEnemy;
    public bool dead = false;

    public Weapon weapon;
    public Element element;

    public Team team;
    public Ability[] abilities;

    private Dictionary<StatusEffect, int> effects = new Dictionary<StatusEffect, int>();

    public void AddEffect(StatusEffect effect)
    {
        if (effects.ContainsKey(effect))
        {
            Debug.Log("effects[effect]: "+effects[effect]);
            Debug.Log("effect.remainingTurns: "+effect.remainingTurns);
            effects[effect] = Mathf.Max(effects[effect], effect.remainingTurns);
        }
        else
        {
            effects.Add(effect, effect.remainingTurns);
            
            var keys = new List<StatusEffect>(effects.Keys);
            foreach (var e in keys)
                Debug.Log("("+name+")"+e+": "+effects[e]);
        }
    }

    public void RemoveEffect(StatusEffect effect)
    {
        if (effects.ContainsKey(effect))
            effects.Remove(effect);
    }

    public void EndTurn()
    {
        var keys = new List<StatusEffect>(effects.Keys);
        foreach (var e in keys)
        {
            effects[e]--;
            if (effects[e] <= 0)
                effects.Remove(e);
        }
    }


    // public int GetEffectTurns(StatusEffect effect)
    // {
    //     if (effects.Keys.ContainsKey(effect))
    //         return effects[effect];
    //     return 0;
    // }

    public bool HasEffect(Effect effect)
    {
        bool has = false;
        var keys = new List<StatusEffect>(effects.Keys);
        foreach (var e in keys)
            if(e.effect == effect) has = true;

        return has;
    }
    
    public void GetHit(Combatant attacker)
    {
        health -= Mathf.RoundToInt(
            (attacker.strength + attacker.weapon.damage) *
            (attacker.HasEffect(Effect.Tuned) ? 1.3f : attacker.HasEffect(Effect.OutOfTune) ? 0.7f : 1f) *
            ElementChart.GetMultiplier(attacker.element, element) *
            (HasEffect(Effect.PerfectTempo) ? 0.7f : HasEffect(Effect.OutOfTempo) ? 1.3f : 1f)
        );
        
        if (health <= 0)
            dead = true;
    }

    public void AddHealth(int s) => health += s;
    public void AddStrength(int s) => strength += s; 
    public void AddChant(int s) => chant += s; 
    public void AddSpeed(int s) => speed += s;
    public void AddInitiative(int s){
        initiative += s;
        if(initiative >= 100) ResetInitiative();
    }
    public void ResetInitiative() => initiative = 0;

    public int GetMaxHealth() => maxHealth;
    public int GetHealth() => health;
    public int GetStrength() => strength;
    public int GetChant() => chant;
    public int GetSpeed() => speed;
    public int GetInitiative() => initiative;
    public Team GetTeam() => team;
    public Ability[] GetAbilities() => abilities;
    public Ability GetAbility1() => abilities[0];
    public Ability GetAbility2() => abilities[1];

    // Obtener todos los efectos activos
    public Dictionary<StatusEffect, int> GetEffects() => new Dictionary<StatusEffect, int>(effects);
}
