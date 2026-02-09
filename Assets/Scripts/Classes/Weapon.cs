using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "RPGFari/Weapon")]
public class Weapon : ScriptableObject
{
    public string name;
    public int damage;
    public Element element;
}
