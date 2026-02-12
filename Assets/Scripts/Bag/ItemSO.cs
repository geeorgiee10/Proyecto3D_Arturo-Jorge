using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Ability ability;
    public Weapon weapon;
}
