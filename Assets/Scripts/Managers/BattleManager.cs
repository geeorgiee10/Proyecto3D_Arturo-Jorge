using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public Transform HeroTeam;
    public Transform EnemyTeam;
    public TurnManager turnManager;

    void Start()
    {
        // Pasar todos los héroes
        foreach (PlayableCharacter c in HeroTeam.GetComponentsInChildren<PlayableCharacter>())
        {
            turnManager.AddCombatant(c);
        }

        // Pasar todos los enemigos
        foreach (PlayableCharacter c in EnemyTeam.GetComponentsInChildren<PlayableCharacter>())
        {
            turnManager.AddCombatant(c);
        }

        turnManager.StartBattle();
    }
}
