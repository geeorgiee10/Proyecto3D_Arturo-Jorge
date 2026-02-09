using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;



public class PlayerEquipment : MonoBehaviour
{
    public static PlayerEquipment Instance;

    public ItemSO equippedWeapon;   
    public ItemSO[] equippedAbilities; 

    void Awake()
    {

         if (Instance == null)
        {
            Instance = this;
            if (equippedAbilities == null || equippedAbilities.Length == 0)
                equippedAbilities = new ItemSO[2];
        }
        else
        {
            Destroy(gameObject);
        }
    } 

    public void EquipWeapon(ItemSO weapon)
    {
        equippedWeapon = weapon;
        Debug.Log("Arma equipada: " + weapon.itemName);
    }


    public void EquipAbility(ItemSO ability, int slot)
    {
        if(slot < 0 || slot >= equippedAbilities.Length)
        {
            Debug.LogWarning("Slot inválido: " + slot);
            return;
        }
        equippedAbilities[slot] = ability;
        Debug.Log("Habilidad equipada en slot " + slot + ": " + ability.itemName);
    }
}
