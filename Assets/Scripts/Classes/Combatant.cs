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

    public bool isEnemy;
    public bool dead = false;

    public Weapon weapon;
    public Element element;

    public bool silenced;
    public Team team;
    public Ability[] abilities;
    

    public List<StatusEffect> activeEffects = new();

    public void AddEffect(StatusEffect effect)
    {
        activeEffects.Add(effect);
    }

    public void EndTurn()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].remainingTurns--;

            if (activeEffects[i].remainingTurns <= 0)
            {
                activeEffects.RemoveAt(i);
            }
        }
    }

    public void GetHit(Combatant attacker)
    {
        health -= Mathf.RoundToInt((attacker.strength + attacker.weapon.damage) * ElementChart.GetMultiplier(attacker.element, element));
        
        if (health <= 0)
        {
            // health = 0;
            dead = true;
        }
    }

    public bool HasEffect<T>() where T : StatusEffect
    {
        foreach (var effect in activeEffects)
            if (effect is T)
                return true;
        return false;
    }

    public void Silence(bool s) => silenced = s;

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
}
