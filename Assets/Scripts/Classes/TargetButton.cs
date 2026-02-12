using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private TextMeshProUGUI key;

    private Combatant target;
    private TurnManager turnManager;

    public void Init(Combatant target, TurnManager manager, string lbl, string key)
    {
        this.target = target;
        turnManager = manager;
        label.text = lbl;
        this.key.text = key;
    }

    public void Confirm()
    {
        turnManager.SelectTarget(target);
    }
}
