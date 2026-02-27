using UnityEngine;
using TMPro;
using System.Collections;

public class HitFeedbackUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float floatSpeed = 80f;
    [SerializeField] private float duration = 0.7f;

    private RectTransform rect;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void Play(QTEResult result, int perfectCount)
    {
        switch (result)
        {
            case QTEResult.Perfect:
                if(perfectCount == 1)
                    text.text = "PERFECT!";
                else
                    text.text = "x"+perfectCount+"PERFECT!";
                text.color = new Color(0f, 1f, 1f);
                rect.localScale = Vector3.one * 0.5f;
                StartCoroutine(PerfectAnimation());
                break;

            case QTEResult.Good:
                text.text = "GOOD";
                text.color = Color.yellow;
                rect.localScale = Vector3.one;
                StartCoroutine(StandardAnimation());
                break;

            case QTEResult.Miss:
                text.text = "MISS...";
                text.color = Color.red;
                rect.localScale = Vector3.one * 0.8f;
                StartCoroutine(MissAnimation());
                break;
        }
    }

    IEnumerator PerfectAnimation()
    {
        float t = 0f;

        // POP inicial exagerado
        while (t < 0.15f)
        {
            t += Time.deltaTime;
            rect.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one * 1.8f, t / 0.15f);
            yield return null;
        }

        rect.localScale = Vector3.one * 1.4f;

        yield return StartCoroutine(FloatAndFade());
    }

    IEnumerator StandardAnimation()
    {
        yield return StartCoroutine(FloatAndFade());
    }

    IEnumerator MissAnimation()
    {
        float shakeTime = 0.2f;
        float t = 0f;
        Vector2 originalPos = rect.anchoredPosition;

        // pequeño shake triste
        while (t < shakeTime)
        {
            t += Time.deltaTime;
            rect.anchoredPosition = originalPos + Random.insideUnitCircle * 5f;
            yield return null;
        }

        rect.anchoredPosition = originalPos;

        yield return StartCoroutine(FloatAndFade());
    }

    IEnumerator FloatAndFade()
    {
        float time = 0f;
        Vector2 startPos = rect.anchoredPosition;

        while (time < duration)
        {
            time += Time.deltaTime;

            rect.anchoredPosition = startPos + Vector2.up * floatSpeed * time;
            canvasGroup.alpha = 1 - (time / duration);

            yield return null;
        }

        Destroy(gameObject);
    }
}
