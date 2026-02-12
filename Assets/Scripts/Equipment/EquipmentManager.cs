using UnityEngine;
using System.Collections.Generic;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    private CharacterEquipment[] characters;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            characters = FindObjectsOfType<CharacterEquipment>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EquipWeapon(ItemSO weapon)
    {
        EquipInternal(weapon);
    }

    public void EquipAbility(ItemSO ability)
    {
        EquipInternal(ability);
    }

    void EquipInternal( ItemSO item)
    {
        foreach (var character in characters)
        {
            if(item.weapon != null)
                if(item.weapon.element == character.element)
                    character.EquipWeapon(item);
            if(item.ability != null)
                if(item.ability.element == character.element)
                    character.ToggleAbility(item);
        }

    }
}
