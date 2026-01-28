using UnityEngine;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    private List<PlayableCharacter> combatants;
    private int currentIndex;

    void Awake()
    {
        combatants = new List<PlayableCharacter>();
    }

    public void AddCombatant(PlayableCharacter pc)
    {
        combatants.Add(pc);
        Debug.Log("ME AÑADEN A: "+pc);
    }

    public void StartBattle()
    {
        List<int> die = Utils.GetRandomPermutation(combatants.Count);

        for(int i = 0; i < combatants.Count; i++)
        {
            combatants[i].AddInitiative(die[i]);
        }

        Debug.Log("Iniciativas: ");
        foreach(var combatant in combatants)
        {
            combatant.AddInitiative(combatant.GetSpeed());
            Debug.Log(combatant+" - "+combatant.GetInitiative());
        }
        StartRound();
    }

    public void StartRound()
    {
        combatants.Sort((a, b) =>
            b.GetInitiative().CompareTo(a.GetInitiative()));

        Debug.Log("ORDEN: ");
        foreach(var c in combatants)
        {
            Debug.Log(c);
        }
    }

    public void StartTurn()
    {
        if (combatants[currentIndex])
        {
            
        }
        // combatants[currentIndex].StartTurn();
    }

    public void EndTurn()
    {
        combatants[currentIndex].EndTurn();
        currentIndex = (currentIndex + 1) % combatants.Count;
    }
}
