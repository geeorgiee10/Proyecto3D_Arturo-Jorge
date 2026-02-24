using System.Collections.Generic;

[System.Serializable]
public class CombatantData
{
    public int slotIndex;

    public string name;
    public int maxHealth;
    public int health;
    public int strength;
    public int speed;
    public int initiative;
    public int abilityPoints;

    public Weapon weapon;
    public Element element;
    public Team team;

    public Ability[] abilities;
}