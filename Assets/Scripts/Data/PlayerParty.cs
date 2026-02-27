using System.Collections.Generic;
using UnityEngine;

public class PlayerParty : MonoBehaviour
{
    public static PlayerParty Instance;
    [SerializeField] private RescuedCharacterData resCharData;

    public int partyMask = 1;
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
    
    public void AddCharacter(CombatantData cd)
    {
        if (partyMembers.Contains(cd))
            return;

        partyMembers.Add(cd);
    }

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

    void Start()
    {
        partyMask = 1;
        foreach(string completedEvent in WorldData.Instance.completedEvents)
        {            
            if(completedEvent == "paquirrín")
            {
                AddCharacter(resCharData.rescuableCombatants[2]);
                partyMask += 8;
            }

            if(completedEvent == "cigala")
            {
                AddCharacter(resCharData.rescuableCombatants[1]);
                partyMask += 4;
            }

            if(completedEvent == "fariV1")
            {
                AddCharacter(resCharData.rescuableCombatants[0]);
                partyMask += 2;
            }
        }

        foreach(CombatantData cd in partyMembers)
        {
            Bag.Instance.AddItem(new ItemSO(cd.abilities[0].name, cd.abilities[0]));
            Bag.Instance.AddItem(new ItemSO(cd.abilities[1].name, cd.abilities[1]));
            Bag.Instance.AddItem(new ItemSO(cd.weapon.name, cd.weapon));
        }
        
        TalkIndicator.Instance.Hide();
    }

    public bool ToggleAbility(Ability ability)
    {
        CombatantData cd = partyMembers.Find(c => c.element == ability.element);

        if (cd == null)
            return false;

        for (int i = 0; i < cd.abilities.Length; i++)
            if (cd.abilities[i] == ability)
            {
                cd.abilities[i] = null;
                return true;
            }

        for (int i = 0; i < cd.abilities.Length; i++)
            if (cd.abilities[i] == null)
            {
                cd.abilities[i] = ability;
                return true;
            }

        return false;
    }
}
