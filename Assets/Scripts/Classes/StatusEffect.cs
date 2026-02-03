using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewEffect", menuName = "RPGFari/Status_Effects")]
public class StatusEffect : ScriptableObject
{
    public int remainingTurns;
    public Effect effect;
    public Sprite icon;
}


// public abstract class StatusEffect
// {
//     protected Combatant owner;
//     public int remainingTurns;
//     public Effect effect;

//     public bool IsExpired => remainingTurns <= 0;

//     public void Apply(Combatant target, int turns)
//     {
//         owner = target;
//         remainingTurns = turns;
//         OnApply();
//     }

//     public void OnTurnStart()
//     {
//         OnTurnStartEffect();
//     }

//     public void OnTurnEnd()
//     {
//         remainingTurns--;
//         if (IsExpired)
//             OnExpire();
//     }

//     protected virtual void OnApply() { }
//     protected virtual void OnTurnStartEffect() { }
//     protected virtual void OnExpire() { }
// }
