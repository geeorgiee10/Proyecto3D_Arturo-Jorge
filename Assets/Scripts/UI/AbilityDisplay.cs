using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityDisplay : MonoBehaviour
{
    public Image titleBackground; //353535
    public Image keyBackground; //0C0C0C
    public Image costBackground; //00F1CA

    public TextMeshProUGUI txtTitle;
    public TextMeshProUGUI txtDescription;
    public TextMeshProUGUI txtKey;
    public TextMeshProUGUI txtCost;

    public bool canUse;

    void Update()
    {
        float alpha = canUse ? 1f : 0.3f;

        SetAlpha(titleBackground, alpha);
        SetAlpha(keyBackground, alpha);
        SetAlpha(costBackground, alpha);

        txtKey.color = canUse ? Color.white : Color.red;
        SetAlpha(txtTitle, alpha);
        SetAlpha(txtDescription, alpha);
        SetAlpha(txtKey, alpha);
        SetAlpha(txtCost, alpha);
    }

    void SetAlpha(Graphic graphic, float alpha)
    {
        Color c = graphic.color;
        c.a = alpha;
        graphic.color = c;
    }
}
