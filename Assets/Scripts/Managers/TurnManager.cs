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
    [SerializeField] private AbilityDisplay ability1;
    [SerializeField] private AbilityDisplay ability2;


    [Header("Choose Target Panel")]
    [SerializeField] private GameObject chooseTargetPanel;
    [SerializeField] private Transform targetContainer;
    [SerializeField] private GameObject targetButtonPrefab;


    [Header("QTE Panel")]
    [SerializeField] private GameObject qtePanel;


    [Header("Turn Data")]
    [SerializeField] private Ability selectedAbility;
    [SerializeField] private List<Combatant> selectedTargets;

    private enum Action
    {
        SelectingAbility,
        SelectingTarget,
        SelectingEnemyTarget,
        SelectingAllyTarget,
        Confirming,
        Attacking,
    }
    private Action currentAction = Action.SelectingAbility;

    private string[] keys = {"Q","W","E","R"};
    private KeyCode[] inputKeys = {
        KeyCode.Q,
        KeyCode.W,
        KeyCode.E,
        KeyCode.R
    };
    private Dictionary<KeyCode, bool> keyPressed = new Dictionary<KeyCode, bool>();

    void Awake()
    {
        chooseTargetPanel.SetActive(false);
        qtePanel.SetActive(false);
        combatants = new List<Combatant>();
        
        selectedTargets = new List<Combatant>();

        KeyCode[] keysToCheck = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.Escape };
        foreach (var key in keysToCheck)
            keyPressed[key] = false;
    }

    private bool GetKeyPressedOnce(KeyCode key)
    {
        bool isDown = Input.GetKey(key);

        if (isDown && !keyPressed[key])
        {
            keyPressed[key] = true;
            return true;
        }

        if (!isDown)
            keyPressed[key] = false;

        return false;
    }

    void Update()
    {
        if(currentAction == Action.SelectingAbility)
            HandleSelectAbility();
        if(currentAction == Action.SelectingEnemyTarget)
            HandleSelectEnemyTarget();
        if(currentAction == Action.SelectingAllyTarget)
            HandleSelectAllyTarget();
        if(currentAction == Action.Confirming)
            HandleConfirm();
    }

    void HandleSelectAbility()
    {
        Combatant combatant = combatants[currentIndex];

        if (GetKeyPressedOnce(KeyCode.Q))
            SelectAttack();

        else if (GetKeyPressedOnce(KeyCode.W) && ability1.canUse)
            SelectAbility(combatant.GetAbility1());

        else if (GetKeyPressedOnce(KeyCode.E) && ability2.canUse)
            SelectAbility(combatant.GetAbility2());
    }

    void HandleSelectEnemyTarget()
    {
        int i = 0;
            foreach(Combatant c in GetEnemies())
            {
                if (GetKeyPressedOnce(inputKeys[i]))
                    SelectTarget(c);

                i++;
            }

        if (GetKeyPressedOnce(KeyCode.Escape))
            CancelAbility();
    }

    void HandleSelectAllyTarget()
    {
        int i = 0;
            foreach(Combatant c in GetHeroes())
            {
                if (GetKeyPressedOnce(inputKeys[i]))
                    SelectTarget(c);

                i++;
            }

        if (GetKeyPressedOnce(KeyCode.Escape))
            CancelAbility();
    }

    void HandleConfirm()
    {
        if (GetKeyPressedOnce(KeyCode.Q))
            SelectTarget(null);

        if (GetKeyPressedOnce(KeyCode.Escape))
            CancelAbility();
    }

    public void AddCombatant(Combatant pc)
    {
        combatants.Add(pc);
    }

    public void StartBattle()
    {
        List<int> die = Utils.GetRandomPermutation(combatants.Count);

        for(int i = 0; i < combatants.Count; i++)
        {
            combatants[i].AddInitiative(die[i]);
            combatants[i].abilityPoints = 3;
        }

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
            ability1.canUse = combatant.abilityPoints >= combatant.abilities[0].cost && !combatant.HasEffect(Effect.Silence);
            ability2.canUse = combatant.abilityPoints >= combatant.abilities[1].cost && !combatant.HasEffect(Effect.Silence);


            ability1.txtTitle.text = combatant.abilities[0].name;
            ability1.txtDescription.text = combatant.abilities[0].GetFormattedDescription();
            ability1.txtKey.text = "W";
            ability1.txtCost.text = ""+combatant.abilities[0].cost;

            ability2.txtTitle.text = combatant.abilities[1].name;
            ability2.txtDescription.text = combatant.abilities[1].GetFormattedDescription();
            ability2.txtKey.text = "E";
            ability2.txtCost.text = ""+combatant.abilities[1].cost;
        }
        else
        {

            selectActionPanel.SetActive(false);
            chooseTargetPanel.SetActive(false);
            qtePanel.SetActive(true);

            txtTurnTitle.text = "Turno de "+combatant.name;
            selectedAbility = ChooseRandomAbility(combatant);

            if(selectedAbility.target == AttackTarget.Ally)
                selectedTargets.Add(GetRandomEnemy());

            else if(selectedAbility.target == AttackTarget.AllyTeam)
                foreach(Combatant c in GetEnemies())
                    selectedTargets.Add(c);

            else if(selectedAbility.target == AttackTarget.Enemy)
                selectedTargets.Add(GetRandomHero());

            else if(selectedAbility.target == AttackTarget.EnemyTeam)
                foreach(Combatant c in GetHeroes())
                    selectedTargets.Add(c);

            StartCoroutine(ShowAbilityAttack());
        }
    }

    void CancelAbility()
    {
        selectedAbility = null;
        selectActionPanel.SetActive(true);
        chooseTargetPanel.SetActive(false);
        qtePanel.SetActive(false);

        currentAction = Action.SelectingAbility;
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
            if(target.dead) return;

            GameObject btnObj = Instantiate(targetButtonPrefab, targetContainer);
            TargetButton btn = btnObj.GetComponent<TargetButton>();
            btn.Init(target, this, target.name, keys[i]);
            
            RectTransform rt = btnObj.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, startY + i * spacing);
            i++;
        }
    }

    public void ShowConfirmation()
    {
        currentAction = Action.Confirming;
        chooseTargetPanel.SetActive(true);

        foreach (Transform child in targetContainer)
            Destroy(child.gameObject);
        
        GameObject btnObj = Instantiate(targetButtonPrefab, targetContainer);
        TargetButton btn = btnObj.GetComponent<TargetButton>();

        btn.Init(null, this, "Confirmar", "Q");
    }

    private void SelectAttack()
    {
        selectActionPanel.SetActive(false);
        chooseTargetPanel.SetActive(true);
        qtePanel.SetActive(false);
        
        currentAction = Action.SelectingEnemyTarget;
        ShowTargetSelection(GetEnemies());
    }

    private void SelectAbility(Ability a)
    {
        selectedAbility = a;
        selectActionPanel.SetActive(false);
        chooseTargetPanel.SetActive(true);
        qtePanel.SetActive(false);
        
        if (a.target == AttackTarget.EnemyTeam || a.target == AttackTarget.AllyTeam)
        {
            ShowConfirmation();
            currentAction = Action.Confirming;
        }
        else if (a.target == AttackTarget.Ally)
        {
            ShowTargetSelection(GetHeroes());
            currentAction = Action.SelectingAllyTarget;
        }
        else
        {
            ShowTargetSelection(GetEnemies());
            currentAction = Action.SelectingEnemyTarget;
        }
    }

    public void SelectTarget(Combatant c)
    {
        if(selectedAbility != null)
            combatants[currentIndex].abilityPoints -= selectedAbility.cost;
        selectedTargets.Add(c);
        if(selectedTargets[0] == null)
        {
            selectedTargets = new List<Combatant>();
            foreach(
                Combatant comb in 
                    selectedAbility.target == AttackTarget.AllyTeam ? GetHeroes() :
                    selectedAbility.target == AttackTarget.EnemyTeam ? GetEnemies() :
                    GetEnemies()
                )
                selectedTargets.Add(comb);
        }
        
        selectActionPanel.SetActive(false);
        chooseTargetPanel.SetActive(false);
        if(selectedAbility == null)
            StartCoroutine(ShowBasicAttack());
        else{
            qtePanel.SetActive(true);
            StartCoroutine(ShowAbilityAttack());
        }
    }


    IEnumerator ShowBasicAttack()
    {
        currentAction = Action.Attacking;
        Combatant combatant = combatants[currentIndex];
        
        string actionText = combatant.name + " hace un ataque " +
                            ElementChart.GetText(combatant.element, selectedTargets[0].element)+" a " +
                            selectedTargets[0].name;
        txtTurnTitle.text = actionText;

        yield return new WaitForSeconds(2f);
        selectedTargets[0].GetHit(combatant);
        
        selectedAbility = null;
        selectedTargets = new List<Combatant>();
        combatant.abilityPoints += 2;

        yield return new WaitForSeconds(.5f);
        if (!CheckBattleEnd())
            EndTurn();
    }

    IEnumerator ShowAbilityAttack()
    {
        currentAction = Action.Attacking;
        Combatant combatant = combatants[currentIndex];

        string actionText = combatant.name + " usa " +
                            selectedAbility.name + " contra " +
                            (selectedTargets.Count < 2 ? selectedTargets[0].name : "todos los adversarios");
        txtTurnTitle.text = actionText;

        float multiplier = 1f;

        if (!combatant.isEnemy)
        {
            qteManager.SetModifiers(
                combatant.HasEffect(Effect.Groove),
                combatant.HasEffect(Effect.Extasis),
                combatant.HasEffect(Effect.Microtone)
            );
            qteManager.ShowBeats(selectedAbility.qtePattern);
            yield return new WaitForSeconds(0.5f);

            Coroutine progressRoutine = qteManager.StartMoveProgressBar();
            yield return progressRoutine;

            multiplier += qteManager.PerfectCount * .1f;
            multiplier -= qteManager.MissCount * .1f;

            combatant.abilityPoints += qteManager.PerfectCount / 2;

            if(
                qteManager.MissCount == 0 &&
                qteManager.GoodCount == 0 &&
                qteManager.PerfectCount != 0
            )
            {
                combatant.abilityPoints++;
                multiplier += .2f;
            }

            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            qtePanel.SetActive(false);
            yield return new WaitForSeconds(2.5f);
        }

        foreach(Combatant c in selectedTargets)
            selectedAbility.Apply(combatant, c, multiplier);

        selectedAbility = null;
        selectedTargets = new List<Combatant>();

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
        currentAction = Action.SelectingAbility;
        if(currentIndex != combatants.Count - 1)
            StartTurn();
        else
            StartRound();

    }

    

    private List<Combatant> GetEnemies() => combatants.FindAll(c => c.GetTeam() == Team.Enemy && !c.dead);
    private List<Combatant> GetHeroes() => combatants.FindAll(c => c.GetTeam() == Team.Hero && !c.dead);

    private Ability ChooseRandomAbility(Combatant enemy) => enemy.GetAbilities()[Random.Range(0, enemy.GetAbilities().Length)];

    private Combatant GetRandomHero() => GetHeroes()[Random.Range(0, GetHeroes().Count)];
    private Combatant GetRandomEnemy() => GetEnemies()[Random.Range(0, GetEnemies().Count)];
}
