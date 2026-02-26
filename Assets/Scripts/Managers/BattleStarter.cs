using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleStarter : MonoBehaviour
{
    [Header("Enemy Setup")]
    [SerializeField] private int enemyMask;
    [SerializeField] private string enemyId;

    [SerializeField] private List<CombatantData> enemyCombatants;

    public void StartBattle()
    {
        PreparePlayerData();
        PrepareEnemyData();

        BattleData.Instance.enemyId = enemyId;

        WorldData.Instance.playerPosition = PlayerMovement.Instance.transform.position;
        WorldData.Instance.hasSavedPosition = true;
        SceneManager.LoadScene("CombatScene");
    }

    void PreparePlayerData()
    {
        BattleData.Instance.alliedMask = PlayerParty.Instance.partyMask;

        BattleData.Instance.alliedData = PlayerParty.Instance.GetCombatantData();
    }

    void PrepareEnemyData()
    {
        BattleData.Instance.enemyMask = enemyMask;
        BattleData.Instance.enemyData = enemyCombatants;
    }

    void Update()
    {
        gameObject.SetActive(!WorldData.Instance.IsEnemyDefeated(enemyId));
    }
}


/*

AUSTIN      1
PÁRSIFAL    2
LULU        4
SERGEY      8

ANTONIO     1
FARI        2
PAQUIRRÍN   4
CIGALA      8
gMINION     16
mMINION     32
pMINION     64
bMINION     128

*/