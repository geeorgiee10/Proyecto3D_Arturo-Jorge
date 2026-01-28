using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "RPGFari/Weapon")]
public class Weapon : ScriptableObject
{
    private string name;
    private int damage;
    private Element element;
}
