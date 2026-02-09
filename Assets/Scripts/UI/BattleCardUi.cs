using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BattleCardUi : MonoBehaviour
{
    public Combatant combatant;
    [SerializeField] Image imgHealth;
    [SerializeField] TextMeshProUGUI txtHealth;
    [SerializeField] TextMeshProUGUI txtName;
    [SerializeField] Image[] imgIcons;


    private Dictionary<Effect, string> colors = new Dictionary<Effect, string>()
    {
        { Effect.OutOfTempo, "#dfa945" },
        { Effect.OutOfTune, "#9b569b" },
        { Effect.Microtone, "#470070" },
        { Effect.Silence, "#808080" },
        { Effect.PerfectTempo, "#2056ba" },
        { Effect.Tuned, "#64a964" },
        { Effect.Extasis, "#cc2d6f" },
        { Effect.Groove, "#2eb1b1" },
    };

    private float currentFill = 1f;
    public float lerpSpeed = 3f;


    void Update()
    {
        txtName.text = combatant.name;
        txtHealth.text = combatant.health + "/" + combatant.maxHealth;

        float targetFill = (float)combatant.health / combatant.maxHealth;

        currentFill = Mathf.Lerp(currentFill, targetFill, Time.deltaTime * lerpSpeed);
        imgHealth.fillAmount = currentFill;

        imgHealth.color = Color.Lerp(new Color(0.5f, 0f, 0f), Color.red, currentFill);

        foreach (Image i in imgIcons)
        {
            i.sprite = null;
            i.color = new Color(1, 1, 1, 0);
        }

        int ite = 0;
        foreach (var kvp in combatant.GetEffects())
        {
            if (ite >= imgIcons.Length) break;

            StatusEffect effect = kvp.Key;
            int turns = kvp.Value;

            imgIcons[ite].sprite = effect.icon;

            if (colors.ContainsKey(effect.effect))
            {
                Color c;
                if (ColorUtility.TryParseHtmlString(colors[effect.effect], out c))
                    imgIcons[ite].color = c;
                else
                    imgIcons[ite].color = Color.white;
            }
            else
            {
                imgIcons[ite].color = Color.white;
            }

            ite++;
        }
    }
}
