using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public Transform HeroTeam;
    public Transform EnemyTeam;
    public TurnManager turnManager;
    public GameObject battleCardPrefab;
    public RectTransform canvas;

    float spacing = 150f;

    void Start()
    {
        int heroIndex = 0;
        foreach (Combatant c in HeroTeam.GetComponentsInChildren<Combatant>())
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
        foreach (Combatant c in EnemyTeam.GetComponentsInChildren<Combatant>())
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

        SetupTeamPositions(HeroTeam, true);
        SetupTeamPositions(EnemyTeam, false);

        // foreach (Combatant c in HeroTeam.GetComponentsInChildren<Combatant>())
        //     turnManager.AddCombatant(c);

        // foreach (Combatant c in EnemyTeam.GetComponentsInChildren<Combatant>())
        //     turnManager.AddCombatant(c);

        turnManager.StartBattle();
    }

    void SetupTeamPositions(Transform team, bool isHero)
    {
        Combatant[] members = team.GetComponentsInChildren<Combatant>();
        int count = members.Length;

        if (count == 0)
            return;

        float planeWidth = 20f; // ancho total disponible
        float spacing = planeWidth / Mathf.Max(count, 1);

        float startOffset = -((count - 1) * spacing) / 2f;

        for (int i = 0; i < count; i++)
        {
            Transform t = members[i].transform;

            Vector3 localPos = t.localPosition;

            localPos.x = startOffset + i * spacing;
            localPos.z = isHero ? -6f : 6f;

            t.localPosition = localPos;

            // Opcional: girarlos automáticamente
            if (isHero)
                t.forward = Vector3.forward;
            else
                t.forward = Vector3.back;
        }
    }
}
