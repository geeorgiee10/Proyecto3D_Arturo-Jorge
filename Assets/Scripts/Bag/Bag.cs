using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public static Bag Instance;

    private List<ItemSO> weapons = new List<ItemSO>();

    private List<ItemSO> abilities = new List<ItemSO>();


    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(ItemSO item)
    {
        if (item.weapon == null && item.ability == null)
        {
            Debug.LogWarning("El item " + item.itemName + " no tiene ni arma ni habilidad asignada.");
            return;
        }

        if (item.ability != null)
        {
            bool alreadyExists = abilities.Exists(i => i.itemName == item.itemName);

            if (!alreadyExists)
                abilities.Add(item);
        }

        if (item.weapon != null)
        {
            bool alreadyExists = weapons.Exists(i => i.itemName == item.itemName);

            if (!alreadyExists)
                weapons.Add(item);
        }
    }

    public bool HasItem(ItemSO item)
    {
        return weapons.Contains(item);
    }

    public void RemoveItem(ItemSO item)
    {
        if (weapons.Contains(item))
        {
            weapons.Remove(item);
            Debug.Log("Objeto eliminado: " + item.itemName);
        }
    }

    public List<ItemSO> GetWeapons()
    {
        return weapons;
    }

    public List<ItemSO> GetAbilities()
    {
        return abilities;
    }

}
