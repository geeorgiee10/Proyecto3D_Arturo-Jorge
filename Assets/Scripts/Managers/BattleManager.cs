using UnityEngine;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public Transform HeroTeam;
    public Transform EnemyTeam;
    public TurnManager turnManager;
    public GameObject battleCardPrefab;
    public RectTransform canvas;

    private Combatant[] heroTeam;
    private Combatant[] enemyTeam;

    private List<Combatant> activeHeroes = new List<Combatant>();
    private List<Combatant> activeEnemies = new List<Combatant>();

    float spacing = 150f;

    void Awake()
    {
        heroTeam = HeroTeam.GetComponentsInChildren<Combatant>(true);
        enemyTeam = EnemyTeam.GetComponentsInChildren<Combatant>(true);
        
        ApplyMask(BattleData.Instance.alliedMask, heroTeam, true);
        ApplyMask(BattleData.Instance.enemyMask, enemyTeam, false);

        FilterActiveCombatants();
    }

    void FilterActiveCombatants()
    {
        activeHeroes.Clear();
        activeEnemies.Clear();

        foreach (Combatant c in heroTeam)
        {
            if (c.gameObject.activeSelf)
                activeHeroes.Add(c);
        }

        foreach (Combatant c in enemyTeam)
        {
            if (c.gameObject.activeSelf)
                activeEnemies.Add(c);
        }
    }

    void Start()
    {
        int heroIndex = 0;
        foreach (Combatant c in activeHeroes)
        {
            turnManager.AddCombatant(c);

            GameObject card = Instantiate(battleCardPrefab, canvas);
            card.GetComponent<BattleCardUi>().combatant = c;
            RectTransform rt = card.GetComponent<RectTransform>();

            rt.anchorMin = new Vector2(1, 0);
            rt.anchorMax = new Vector2(1, 0);
            rt.pivot = new Vector2(1, 0);

            rt.anchoredPosition = new Vector2(-heroIndex * spacing, 0);
            heroIndex++;
        }

        int enemyIndex = 0;
        foreach (Combatant c in activeEnemies)
        {
            turnManager.AddCombatant(c);

            GameObject card = Instantiate(battleCardPrefab, canvas);
            BattleCardUi ui = card.GetComponent<BattleCardUi>();
            ui.combatant = c;

            ui.followWorldPosition = true;

            ui.worldOffset = new Vector3(0, 3.5f, 0);
            card.GetComponent<RectTransform>().localScale = Vector3.one * 0.5f;

            enemyIndex++;
        }

        SetupTeamPositions(activeHeroes, true);
        SetupTeamPositions(activeEnemies, false);

        turnManager.StartBattle();
    }

    void ApplyMask(int mask, Combatant[] slots, bool isHero)
    {
        int k = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            bool active = (mask & (1 << i)) != 0;
            Debug.Log(slots[i].gameObject);
            slots[i].gameObject.SetActive(active);

            if (active)
            {
                slots[i].name = isHero ?
                    BattleData.Instance.alliedData[k].name : 
                    BattleData.Instance.enemyData[k].name;
                slots[i].maxHealth = isHero ? 
                    BattleData.Instance.alliedData[k].maxHealth : 
                    BattleData.Instance.enemyData[k].maxHealth;
                slots[i].health = isHero ?
                    BattleData.Instance.alliedData[k].health :
                    BattleData.Instance.enemyData[k].health;
                slots[i].strength = isHero ?
                    BattleData.Instance.alliedData[k].strength :
                    BattleData.Instance.enemyData[k].strength;
                slots[i].speed = isHero ?
                    BattleData.Instance.alliedData[k].speed :
                    BattleData.Instance.enemyData[k].speed;
                slots[i].initiative = isHero ?
                    BattleData.Instance.alliedData[k].initiative :
                    BattleData.Instance.enemyData[k].initiative;
                slots[i].abilityPoints = isHero ?
                    BattleData.Instance.alliedData[k].abilityPoints :
                    BattleData.Instance.enemyData[k].abilityPoints;
                slots[i].weapon = isHero ?
                    BattleData.Instance.alliedData[k].weapon :
                    BattleData.Instance.enemyData[k].weapon;
                slots[i].abilities[0] = isHero ?
                    BattleData.Instance.alliedData[k].abilities[0] :
                    BattleData.Instance.enemyData[k].abilities[0];
                slots[i].abilities[1] = isHero ?
                    BattleData.Instance.alliedData[k].abilities[1] :
                    BattleData.Instance.enemyData[k].abilities[1];
                slots[i].element = isHero ?
                    BattleData.Instance.alliedData[k].element :
                    BattleData.Instance.enemyData[k].element;

                k++;
            }
        }
    }

    void SetupTeamPositions(List<Combatant> members, bool isHero)
    {
        int count = members.Count;

        if (count == 0)
            return;

        float planeWidth = -15f;
        float spacing = planeWidth / Mathf.Max(count, 1);
        float startOffset = -((count - 1) * spacing) / 2f;

        for (int i = 0; i < count; i++)
        {
            Transform t = members[i].transform;

            Vector3 localPos = t.localPosition;
            localPos.x = startOffset + i * spacing;
            localPos.z = isHero ? -6f : 6f;

            t.localPosition = localPos;
            t.forward = isHero ? Vector3.forward : Vector3.back;
        }
    }
}
