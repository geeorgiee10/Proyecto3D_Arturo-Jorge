using System.Collections.Generic;
using UnityEngine;

public class PlayerParty : MonoBehaviour
{
    public static PlayerParty Instance;

    public int partyMask;
    public List<CombatantData> partyMembers;
    public CharacterBagDisplayUI[] displays;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        int i = 0;
        foreach(CombatantData cd in partyMembers)
        {
            displays[i].slot = i;
            i++;
        }
    }

    public List<CombatantData> GetCombatantData() => partyMembers;

    public void AddCharacter(CombatantData cd) => partyMembers.Add(cd);

    public void EquipWeapon(Weapon w){
        foreach(CombatantData cd in partyMembers)
            if(cd.element == w.element)
                cd.weapon = w;
    }

    public void EquipAbility(int slot, Ability a)
    {
        foreach(CombatantData cd in partyMembers)
            if(cd.element == a.element)
                cd.abilities[slot] = a;
    }
    public void UnequipAbility(Element element, int slot)
    {
        CombatantData cd = partyMembers.Find(c => c.element == element);

        if (cd != null && slot >= 0 && slot < cd.abilities.Length)
        {
            cd.abilities[slot] = null;
        }
    }
}
