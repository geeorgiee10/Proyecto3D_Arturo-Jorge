using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public string description;
    public CollectibleType type;
}