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

AUSTIN      1       G
PÁRSIFAL    2       M
LULU        4       P
SERGEY      8       B

ANTONIO     1       G
FARI        2       M
PAQUIRRÍN   4       P
CIGALA      8       B

gMINION     16      G
mMINION     32      M
pMINION     64      P
bMINION     128     G

*/