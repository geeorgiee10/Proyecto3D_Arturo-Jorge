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

    public void EquipAbility(int combatant, int slot, Ability a) => partyMembers[combatant].abilities[slot] = a;
    public void UnequipAbility(int combatant, int slot) => partyMembers[combatant].abilities[slot] = null;
}
