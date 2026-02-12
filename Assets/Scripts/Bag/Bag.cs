using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public static Bag Instance;

    private List<ItemSO> items = new List<ItemSO>();

    private List<ItemSO> abilities = new List<ItemSO>();


    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(ItemSO item)
    {
        if(item.weapon == null && item.ability == null)
        {
            Debug.LogWarning("El item " + item.itemName + " no tiene ni arma ni habilidad asignada.");
            return;
        }

        if (item.ability != null)
            abilities.Add(item);

        if (item.weapon != null)
            items.Add(item);

    
    }

    public bool HasItem(ItemSO item)
    {
        return items.Contains(item);
    }

    public void RemoveItem(ItemSO item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log("Objeto eliminado: " + item.itemName);
        }
    }

    public List<ItemSO> GetItems()
    {
        return items;
    }

    public List<ItemSO> GetAbilities()
    {
        return abilities;
    }

}
