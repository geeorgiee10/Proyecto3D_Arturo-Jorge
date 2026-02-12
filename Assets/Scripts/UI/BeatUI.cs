using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BeatUI : MonoBehaviour
{
    public Image background;
    public TMP_Text label;

    public QTEKey ExpectedKey { get; private set; }
    public bool resolved { get; private set; }
    

    public void MarkResolved(QTEResult result)
    {
        resolved = true;
        SetResult(result);
    }

    public void Setup(QTEInput input, float spacing, float lastPosition)
    {
        ExpectedKey = input.key;
        resolved = false;

        switch (input.key)
        {
            case QTEKey.A: label.text = "A"; break;
            case QTEKey.S: label.text = "S"; break;
            case QTEKey.D: label.text = "D"; break;
            case QTEKey.F: label.text = "F"; break;
        }

        switch (input.type)
        {
            case QTEType.None:
                gameObject.SetActive(false);
                return;
        }
    }

    public void SetResult(QTEResult result)
    {
        background.color = result switch
        {
            QTEResult.Perfect => Color.green,
            QTEResult.Good => Color.yellow,
            QTEResult.Miss => Color.red,
            _ => Color.white
        };
    }

    public float XPosition
    {
        get
        {
            return ((RectTransform)transform).anchoredPosition.x;
        }
    }
}
