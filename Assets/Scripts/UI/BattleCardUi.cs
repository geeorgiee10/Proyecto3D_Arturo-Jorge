using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleCardUi : MonoBehaviour
{
    public Combatant combatant;
    [SerializeField] Image imgHealth;
    [SerializeField] TextMeshProUGUI txtHealth;
    [SerializeField] TextMeshProUGUI txtName;

    void Update()
    {
        txtName.text = combatant.name;
        txtHealth.text = combatant.health+"/"+combatant.maxHealth;

        float h = combatant.health;
        float mh = combatant.maxHealth;

        imgHealth.fillAmount = h / mh;

        string effects = "";
        foreach(StatusEffect se in combatant.activeEffects)
        {
            switch (se.effect)
            {
                case Effect.OutOfTune: effects += "D"; break;
                case Effect.OutOfTempo: effects += "FT"; break;
                case Effect.Microtone: effects += "M"; break;
                case Effect.Silence: effects += "S"; break;
                case Effect.Tuned: effects += "A"; break;
                case Effect.PerfectTempo: effects += "TP"; break;
                case Effect.Extasis: effects += "E"; break;
                case Effect.Groove: effects += "G"; break;
            }
        }
    }
}
