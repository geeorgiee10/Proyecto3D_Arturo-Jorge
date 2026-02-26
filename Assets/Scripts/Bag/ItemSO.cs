using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Ability ability;
    public Weapon weapon;

    public ItemSO(string n, Ability a)
    {
        itemName = n;
        ability = a;
        weapon = null;
    }
    public ItemSO(string n, Weapon w)
    {
        itemName = n;
        ability = null;
        weapon = w;
    }
}
