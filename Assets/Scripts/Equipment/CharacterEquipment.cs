using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    public static CharacterEquipment Instance;

    public Element element;

    public ItemSO equippedWeapon;
    public ItemSO[] equippedAbilities = new ItemSO[2];

    private int nextAbilitySlot = 0;

    void Awake()
    {

        if (Instance == null)
        {
            if (equippedAbilities == null)
                equippedAbilities = new ItemSO[2];
            Instance = this;
        }
    }

    public void EquipWeapon(ItemSO weapon)
    {
        equippedWeapon = weapon;
        Debug.Log($"{gameObject.name} equipó arma {weapon.itemName}");
    }

    /*public void EquipAbility(ItemSO ability)
    {
        equippedAbilities[nextAbilitySlot] = ability;
        Debug.Log($"{gameObject.name} equipó habilidad {ability.itemName} en slot {nextAbilitySlot}");

        nextAbilitySlot++;
        if (nextAbilitySlot >= equippedAbilities.Length)
            nextAbilitySlot = 0;
    }*/


    public bool ToggleAbility(ItemSO ability)
    {
       
        for (int i = 0; i < equippedAbilities.Length; i++)
        {
            if (equippedAbilities[i] == ability) 
            {
                equippedAbilities[i] = null; 
                return false;
            }
        }

        
        int startSlot = nextAbilitySlot;
        do
        {
            if (equippedAbilities[nextAbilitySlot] == null)
            {
                equippedAbilities[nextAbilitySlot] = ability;
                nextAbilitySlot++;
                if (nextAbilitySlot >= equippedAbilities.Length) nextAbilitySlot = 0;
                return true;
            }

            nextAbilitySlot++;
            if (nextAbilitySlot >= equippedAbilities.Length) nextAbilitySlot = 0;

        } while (nextAbilitySlot != startSlot);


        return false;
    }

}
