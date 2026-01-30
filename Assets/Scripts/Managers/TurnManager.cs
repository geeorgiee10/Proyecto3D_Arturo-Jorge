using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    private List<Combatant> combatants;
    private int currentIndex;

    [Header("Start Turn Panel")]
    [SerializeField] private QTEManager qteManager;

    [Header("Start Turn Panel")]
    [SerializeField] private GameObject selectActionPanel;
    [SerializeField] private TextMeshProUGUI txtTurnTitle;
    [SerializeField] private Button btnBasicAttack;
    [SerializeField] private Button btnAbility1;
    [SerializeField] private TextMeshProUGUI txtAbility1;
    [SerializeField] private Button btnAbility2;
    [SerializeField] private TextMeshProUGUI txtAbility2;


    [Header("Choose Target Panel")]
    [SerializeField] private GameObject chooseTargetPanel;
    [SerializeField] private Transform targetContainer;
    [SerializeField] private GameObject targetButtonPrefab;
    [SerializeField] private Button btnCancelAbility;


    [Header("QTE Panel")]
    [SerializeField] private GameObject qtePanel;


    [Header("Turn Data")]
    [SerializeField] private Ability selectedAbility;
    [SerializeField] private Combatant selectedTarget;

    void Awake()
    {
        chooseTargetPanel.SetActive(false);
        qtePanel.SetActive(false);
        combatants = new List<Combatant>();
    }

    public void AddCombatant(Combatant pc)
    {
        combatants.Add(pc);
    }

    public void StartBattle()
    {
        List<int> die = Utils.GetRandomPermutation(combatants.Count);

        for(int i = 0; i < combatants.Count; i++)
            combatants[i].AddInitiative(die[i]);

        StartRound();
    }

    public void StartRound()
    {
        foreach(var combatant in combatants)
            combatant.AddInitiative(combatant.GetSpeed());

        combatants.Sort((a, b) =>
            b.GetInitiative().CompareTo(a.GetInitiative()));

        StartTurn();
    }

    public void StartTurn()
    {
        Combatant combatant = combatants[currentIndex];

        if (combatant.dead)
        {
            EndTurn();
            return;
        }

        if (combatant.GetTeam() == Team.Hero)
        {

            selectActionPanel.SetActive(true);
            chooseTargetPanel.SetActive(false);
            qtePanel.SetActive(false);
            
            txtTurnTitle.text = "Turno de "+combatant.name;
            txtAbility1.text = combatant.GetAbility1().name;
            txtAbility2.text = combatant.GetAbility2().name;

            btnAbility1.onClick.AddListener(() => SelectAttack());
            btnAbility1.onClick.AddListener(() => SelectAbility(combatant.GetAbility1()));
            btnAbility2.onClick.AddListener(() => SelectAbility(combatant.GetAbility2()));
        }
        else
        {

            selectActionPanel.SetActive(false);
            chooseTargetPanel.SetActive(false);
            qtePanel.SetActive(true);

            txtTurnTitle.text = "Turno de "+combatant.name;
            selectedAbility = ChooseRandomAbility(combatant);
            selectedTarget = GetRandomHero();

            StartCoroutine(ShowAttack());
        }
    }

    void CancelAbility()
    {
        selectedAbility = null;
        selectActionPanel.SetActive(true);
        chooseTargetPanel.SetActive(false);
        qtePanel.SetActive(false);
    }

    public void ShowTargetSelection(List<Combatant> targets)
    {
        chooseTargetPanel.SetActive(true);

        foreach (Transform child in targetContainer)
            Destroy(child.gameObject);

        float startY = 0f;
        float spacing = -70f;
        int i = 0;

        foreach (Combatant target in targets)
        {
            if(target == combatants[currentIndex]) return;
            if(target.dead) return;

            GameObject btnObj = Instantiate(targetButtonPrefab, targetContainer);
            TargetButton btn = btnObj.GetComponent<TargetButton>();
            btn.Init(target, this, target.name);
            
            RectTransform rt = btnObj.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, startY + i * spacing);
            i++;
        }
    }

    public void ShowConfirmation()
    {
        chooseTargetPanel.SetActive(true);

        foreach (Transform child in targetContainer)
            Destroy(child.gameObject);

        
        GameObject btnObj = Instantiate(targetButtonPrefab, targetContainer);
        TargetButton btn = btnObj.GetComponent<TargetButton>();
        btn.Init(null, this, "Confirmar");
    }

    private void SelectAttack()
    {
        btnCancelAbility.onClick.RemoveAllListeners();
        
        selectActionPanel.SetActive(false);
        chooseTargetPanel.SetActive(true);
        qtePanel.SetActive(false);

        btnCancelAbility.onClick.AddListener(() => CancelAbility());
        ShowTargetSelection(GetEnemies());
    }

    private void SelectAbility(Ability a)
    {
        btnCancelAbility.onClick.RemoveAllListeners();

        selectedAbility = a;
        selectActionPanel.SetActive(false);
        chooseTargetPanel.SetActive(true);
        qtePanel.SetActive(false);
        btnCancelAbility.onClick.AddListener(() => CancelAbility());
        if (a.target == AttackTarget.EnemyTeam || a.target == AttackTarget.AllyTeam || a.target == AttackTarget.Self)
            ShowConfirmation();
        else if (a.target == AttackTarget.Ally)
            ShowTargetSelection(GetHeroes());
        else
            ShowTargetSelection(GetEnemies());
    }

    private List<Combatant> GetEnemies() => combatants.FindAll(c => c.GetTeam() == Team.Enemy);
    private List<Combatant> GetHeroes() => combatants.FindAll(c => c.GetTeam() == Team.Hero);

    public void SelectTarget(Combatant c)
    {
        selectedTarget = c;
        selectActionPanel.SetActive(false);
        chooseTargetPanel.SetActive(false);
        qtePanel.SetActive(true);
        if(selectedAbility == null)
            StartCoroutine(ShowBasicAttack());
        else
            StartCoroutine(ShowAttack());
    }


    IEnumerator ShowBasicAttack()
    {
        Combatant combatant = combatants[currentIndex];
        
        string actionText = combatant.name + " hace un ataque " +
                            ElementChart.GetText(combatant.element, selectedTarget.element)+" a " +
                            selectedTarget.name;
        txtTurnTitle.text = actionText;

        yield return new WaitForSeconds(2f);
        selectedTarget.GetHit(combatant);
    }

    IEnumerator ShowAttack()
    {
        Combatant combatant = combatants[currentIndex];

        string actionText = combatant.name + " usa " +
                            selectedAbility.name + " contra " +
                            (selectedTarget != null ? selectedTarget.name : "todos los adversarios");
        txtTurnTitle.text = actionText;

        float multiplier = 1f;

        // 🧠 SOLO el jugador usa QTE
        if (!combatant.isEnemy)
        {
            qteManager.ShowBeats(selectedAbility.qtePattern);
            yield return new WaitForSeconds(0.5f);

            Coroutine progressRoutine = qteManager.StartMoveProgressBar();
            yield return progressRoutine;

            multiplier += qteManager.PerfectCount * 0.1f;
            multiplier -= qteManager.MissCount * 0.1f;

            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            qtePanel.SetActive(false);
            yield return new WaitForSeconds(2.5f);
        }

        // Enemigos y jugador aplican daño aquí
        selectedAbility.Apply(combatant, selectedTarget, multiplier);

        selectedAbility = null;
        selectedTarget = null;

        if (!CheckBattleEnd())
            EndTurn();
    }

    public bool CheckBattleEnd()
    {
        bool allEnemiesDead = true;
        foreach (Combatant c in combatants)
        {
            if (c.team == Team.Enemy && !c.dead)
            {
                allEnemiesDead = false;
                return false;
            }
        }

        if (allEnemiesDead)
        {
            txtTurnTitle.text = "Ganan los héroes";
            return true;
        }

        bool allHeroesDead = true;
        foreach (Combatant c in combatants)
        {
            if (c.team == Team.Hero && !c.dead)
            {
                allHeroesDead = false;
                return false;
            }
        }

        if (allHeroesDead)
        {
            txtTurnTitle.text = "Ganan los malos";
            return true;
        }

        return false;
    }

    public void EndTurn()
    {
        combatants[currentIndex].EndTurn();
        currentIndex = (currentIndex + 1) % combatants.Count;
        if(currentIndex != combatants.Count - 1)
            StartTurn();
        else
            StartRound();

    }

    private Ability ChooseRandomAbility(Combatant enemy) => enemy.GetAbilities()[Random.Range(0, enemy.GetAbilities().Length)];

    private Combatant GetRandomHero() => GetHeroes()[Random.Range(0, GetHeroes().Count)];
}
