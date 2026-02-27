using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BagItemUI : MonoBehaviour
{
    public Image background;
    public TextMeshProUGUI text;

    public void SetText(string value)
    {
        text.text = value;
    }

    public void SetSelected(bool selected)
    {
        background.color = selected
            ? new Color(0f, 0f, 0f, 0.6f) 
            : Color.clear;
    }
}
