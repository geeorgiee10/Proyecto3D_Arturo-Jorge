using UnityEngine;
using System.Collections.Generic;

public class PlayableCharacter : MonoBehaviour
{
    public int health;
    public int strength;
    public int chant;
    public int speed;
    public int initiative;

    public Weapon weapon;
    public Element element;

    public bool silenced;
    public Team team;
    public Ability abiñities;
    

    private List<StatusEffect> activeEffects = new();

    public void AddEffect(StatusEffect effect, int turns)
    {
        effect.Apply(this, turns);
        activeEffects.Add(effect);
    }

    public void StartTurn()
    {
        foreach (var effect in activeEffects)
            effect.OnTurnStart();
    }

    public void EndTurn()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].OnTurnEnd();
            if (activeEffects[i].IsExpired)
                activeEffects.RemoveAt(i);
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
    public void AddInitiative(int s) => initiative += s;
    public void ResetInitiative() => initiative = 0;

    public int GetHealth() => health;
    public int GetStrength() => strength;
    public int GetChant() => chant;
    public int GetSpeed() => speed;
    public int GetInitiative() => initiative;
}
