using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private Button btn;
    private Combatant target;
    private TurnManager turnManager;

    public void Init(Combatant target, TurnManager manager, string lbl)
    {
        this.target = target;
        turnManager = manager;
        label.text = lbl;
        btn.onClick.AddListener(() => OnClick());
    }

    public void OnClick()
    {
        turnManager.SelectTarget(target);
    }
}
