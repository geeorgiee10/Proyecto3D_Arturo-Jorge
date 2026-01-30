using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public Transform HeroTeam;
    public Transform EnemyTeam;
    public TurnManager turnManager;

    public TextMeshProUGUI txtHealths;

    void Start()
    {
        foreach (Combatant c in HeroTeam.GetComponentsInChildren<Combatant>())
        {
            turnManager.AddCombatant(c);
        }

        foreach (Combatant c in EnemyTeam.GetComponentsInChildren<Combatant>())
        {
            turnManager.AddCombatant(c);
        }

        turnManager.StartBattle();
    }

    void Update()
    {
        string healths = "";

        healths += "ALIADOS:\n";
        foreach (Combatant c in HeroTeam.GetComponentsInChildren<Combatant>())
        {
            healths += c.name + " - " + c.health+"/"+c.maxHealth+"\n";
        }

        healths += "\n\nENEMIGOS:\n";
        foreach (Combatant c in EnemyTeam.GetComponentsInChildren<Combatant>())
        {
            healths += c.name + " - " + c.health+"/"+c.maxHealth+"\n";
        }

        txtHealths.text = healths;
    }
}
