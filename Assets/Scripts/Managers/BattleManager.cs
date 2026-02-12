using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public Transform HeroTeam;   // referencia solo para los combatants
    public Transform EnemyTeam;  // referencia solo para los combatants
    public TurnManager turnManager;
    public GameObject battleCardPrefab; // prefab de la card
    public RectTransform canvas;        // canvas de la batalla

    float spacing = 150f; // separación entre cards

    void Start()
    {
        // Héroes: esquina inferior derecha -> izquierda
        int heroIndex = 0;
        foreach (Combatant c in HeroTeam.GetComponentsInChildren<Combatant>())
        {
            turnManager.AddCombatant(c);

            GameObject card = Instantiate(battleCardPrefab, canvas);
            card.GetComponent<BattleCardUi>().combatant = c;
            RectTransform rt = card.GetComponent<RectTransform>();

            // Anchors en esquina inferior derecha
            rt.anchorMin = new Vector2(1, 0);
            rt.anchorMax = new Vector2(1, 0);
            rt.pivot = new Vector2(1, 0);

            // Posición relativa desde la esquina inferior derecha
            rt.anchoredPosition = new Vector2(-heroIndex * spacing, 0); // 10 píxeles del borde inferior
            heroIndex++;
        }

        // Enemigos: esquina superior izquierda -> derecha
        int enemyIndex = 0;
        foreach (Combatant c in EnemyTeam.GetComponentsInChildren<Combatant>())
        {
            turnManager.AddCombatant(c);

            GameObject card = Instantiate(battleCardPrefab, canvas);
            card.GetComponent<BattleCardUi>().combatant = c;
            RectTransform rt = card.GetComponent<RectTransform>();

            // Anchors en esquina superior izquierda
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(0, 1);
            rt.pivot = new Vector2(0, 1);

            // Posición relativa desde la esquina superior izquierda
            rt.anchoredPosition = new Vector2(enemyIndex * spacing, 0); // 10 píxeles del borde superior
            enemyIndex++;
        }

        turnManager.StartBattle();
    }
}
