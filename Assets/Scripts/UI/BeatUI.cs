using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BeatUI : MonoBehaviour
{
    public Image background;
    public Image grooveLogo;
    public TMP_Text label;

    public QTEKey ExpectedKey { get; private set; }
    public bool resolved { get; private set; }
    

    public void MarkResolved(QTEResult result)
    {
        resolved = true;
        SetResult(result);
    }

    public void Setup(QTEInput input, float spacing, float lastPosition, bool groove)
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
        
        if (groove)
        {
            grooveLogo.color = new Color(0, 0, 0, 1); 
            label.color = new Color(0, 0, 0, 0); 
        }
        else
        {
            grooveLogo.color = new Color(0, 0, 0, 0); 
            label.color = new Color(0, 0, 0, 1); 
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
            QTEResult.Perfect => new Color(0f, 1f, 1f),
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
