using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    public static CharacterEquipment Instance;

    public Element element;

    public ItemSO equippedWeapon;

    public int combatantIndex;

    public Combatant combatant;

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
        
        CombatantData cd = PlayerParty.Instance.partyMembers[combatantIndex];

        // Buscar si ya está equipada
        for (int i = 0; i < cd.abilities.Length; i++)
        {
            if (cd.abilities[i] == ability.ability)
            {
                cd.abilities[i] = null;
                return false;
            }
        }

        // Buscar slot vacío
        for (int i = 0; i < cd.abilities.Length; i++)
        {
            if (cd.abilities[i] == null)
            {
                cd.abilities[i] = ability.ability;
                return true;
            }
        }

        return false; 
    }

    public bool IsAbilityEquipped(ItemSO ability)
    {
        if (ability == null || PlayerParty.Instance == null)
        return false;

        // Buscar el miembro de la party con este elemento
        CombatantData cd = PlayerParty.Instance.partyMembers
            .Find(c => c.element == element);

        if (cd == null || cd.abilities == null)
            return false;

        // Buscar la habilidad en sus slots
        foreach (var a in cd.abilities)
        {
            if (a == ability.ability)
                return true;
        }

        return false;
    }



}
