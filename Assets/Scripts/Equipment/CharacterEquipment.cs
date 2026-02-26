using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    public static CharacterEquipment Instance;

    public Element element;

    public ItemSO equippedWeapon;

    public int combatantIndex;


    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void EquipWeapon(ItemSO weapon)
    {
        equippedWeapon = weapon;
    }


    public bool ToggleAbility(ItemSO ability)
    {
        if (combatantIndex < 0 || combatantIndex >= PlayerParty.Instance.partyMembers.Count)
        {
            return false;
        }

        // Buscar si ya está equipada
        for (int i = 0; i < PlayerParty.Instance.partyMembers[combatantIndex].abilities.Length; i++)
        {
            if (PlayerParty.Instance.partyMembers[combatantIndex].abilities[i] == ability.ability)
            {
                PlayerParty.Instance.partyMembers[combatantIndex].abilities[i] = null;
                return false;
            }
        }

        // Buscar slot vacío
        for (int i = 0; i < PlayerParty.Instance.partyMembers[combatantIndex].abilities.Length; i++)
        {
            if (PlayerParty.Instance.partyMembers[combatantIndex].abilities[i] == null)
            {
                PlayerParty.Instance.partyMembers[combatantIndex].abilities[i] = ability.ability;
                return true;
            }
        }

        return false; 
    }

    public bool IsAbilityEquipped(ItemSO ability)
    {
        if (ability == null || PlayerParty.Instance == null)
            return false;

        if (combatantIndex < 0 || combatantIndex >= PlayerParty.Instance.partyMembers.Count)
            return false;

        CombatantData cd = PlayerParty.Instance.partyMembers[combatantIndex];

        if (cd.abilities == null)
            return false;

        foreach (var a in cd.abilities)
        {
            if (a == ability.ability)
                return true;
        }

        return false;
    }



}
