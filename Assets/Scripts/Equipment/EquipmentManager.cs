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
        EquipInternal(weapon, true);
    }

    public void EquipAbility(ItemSO ability)
    {
        EquipInternal(ability, false);
    }

    void EquipInternal(ItemSO item, bool isWeapon)
    {
        foreach (var character in characters)
        {
            if (character.element == item.element)
            {
                if (isWeapon)
                    character.EquipWeapon(item);
                else
                    character.EquipAbility(item);
                return;
            }
        }

        Debug.LogWarning("No hay personaje con elemento " + item.element);
    }
}
