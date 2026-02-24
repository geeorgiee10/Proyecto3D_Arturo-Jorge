using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleStarter : MonoBehaviour
{
    [Header("Enemy Setup")]
    [SerializeField] private int enemyMask;

    [SerializeField] private List<CombatantData> enemyCombatants;

    public void StartBattle()
    {
        PreparePlayerData();
        PrepareEnemyData();

        SceneManager.LoadScene("CombatScene");
    }

    void PreparePlayerData()
    {
        BattleData.Instance.alliedMask = PlayerParty.Instance.partyMask;

        BattleData.Instance.alliedData = 
            PlayerParty.Instance.GetCombatantData();
    }

    void PrepareEnemyData()
    {
        BattleData.Instance.enemyMask = enemyMask;
        BattleData.Instance.enemyData = enemyCombatants;
    }
}
