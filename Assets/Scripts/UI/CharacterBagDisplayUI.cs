using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CharacterBagDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private TextMeshProUGUI txtElement;
    [SerializeField] private TextMeshProUGUI txtWeapon;
    [SerializeField] private TextMeshProUGUI txtAbility1;
    [SerializeField] private TextMeshProUGUI txtAbility2;

    public int slot = -1;




    void Update()
    {
        if(slot == -1){
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);

        CombatantData cd = PlayerParty.Instance.GetCombatantData()[slot];
        txtName.text = ""+cd.name;
        switch (cd.element)
        {
            case Element.Harmony: txtElement.text = "Armonía"; break;
            case Element.Melody: txtElement.text = "Melodía"; break;
            case Element.Rythm: txtElement.text = "Ritmo"; break;
            case Element.Timbre: txtElement.text = "Timbre"; break;
            default: txtElement.text = "???"; break;
        }
        txtWeapon.text = ""+cd.weapon.name;
        txtAbility1.text = ""+cd.abilities[0].name;
        txtAbility2.text = ""+cd.abilities[1].name;
    }
}