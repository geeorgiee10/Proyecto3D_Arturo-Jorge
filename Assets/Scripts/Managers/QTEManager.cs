using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QTEManager : MonoBehaviour
{
    [SerializeField] private RectTransform timeline;
    [SerializeField] private BeatUI beatPrefab;
    [SerializeField] private RectTransform progressBar;
    [SerializeField] private float progressBarSpeed = 200f;

    [SerializeField] private float perfectWindow = 10f;
    [SerializeField] private float goodWindow = 25f;

    private List<BeatUI> beats = new();
    private int currentBeatIndex = 0;

    public int PerfectCount { get; private set; }
    public int GoodCount { get; private set; }
    public int MissCount { get; private set; }

    public void ShowBeats(QTEPattern pattern)
    {
        beats.Clear();
        currentBeatIndex = 0;

        PerfectCount = 0;
        GoodCount = 0;
        MissCount = 0;

        float timelineSize = timeline.sizeDelta.x;
        int beatPoints = pattern.sequence.Length + 1;

        float spacing = timelineSize / beatPoints;
        float lastPosition = spacing - (timelineSize / 2f);

        foreach (QTEInput qteI in pattern.sequence)
        {
            BeatUI beat = Instantiate(beatPrefab, timeline);
            beat.Setup(qteI, spacing, lastPosition);

            RectTransform rt = beat.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(lastPosition, 0f);

            beats.Add(beat);
            lastPosition += spacing;
        }
    }

    public Coroutine StartMoveProgressBar()
    {
        return StartCoroutine(MoveProgressBar());
    }

    void Update()
    {
        float barX = progressBar.anchoredPosition.x;

        foreach (var beat in beats)
        {
            if (!beat.gameObject.activeSelf || beat.resolved)
                continue;

            float distance = Mathf.Abs(barX - beat.XPosition);
            if (distance > goodWindow)
                continue;

            if (IsCorrectKeyPressed(beat.ExpectedKey))
            {
                EvaluateHit(beat, distance);
                break;
            }
        }
    }

    bool IsCorrectKeyPressed(QTEKey key)
    {
        return key switch
        {
            QTEKey.A => Input.GetKeyDown(KeyCode.A),
            QTEKey.S => Input.GetKeyDown(KeyCode.S),
            QTEKey.D => Input.GetKeyDown(KeyCode.D),
            QTEKey.F => Input.GetKeyDown(KeyCode.F),
            _ => false
        };
    }

    void EvaluateHit(BeatUI beat, float distance)
    {
        if (distance <= perfectWindow)
        {
            beat.MarkResolved(QTEResult.Perfect);
            PerfectCount++;
        }
        else if (distance <= goodWindow)
        {
            beat.MarkResolved(QTEResult.Good);
            GoodCount++;
        }
        else
        {
            beat.MarkResolved(QTEResult.Miss);
            MissCount++;
        }
    }

    IEnumerator MoveProgressBar()
    {
        float left = -timeline.sizeDelta.x / 2f;
        float right = timeline.sizeDelta.x / 2f;

        progressBar.anchoredPosition = new Vector2(left, progressBar.anchoredPosition.y);
        float previousX = progressBar.anchoredPosition.x;

        while (progressBar.anchoredPosition.x < right)
        {
            progressBar.anchoredPosition += Vector2.right * progressBarSpeed * Time.deltaTime;
            float currentX = progressBar.anchoredPosition.x;

            CheckBeats(previousX, currentX);

            previousX = currentX;
            yield return null;
        }

        ClearBeats();
    }

    void CheckBeats(float previousX, float currentX)
    {
        foreach (var beat in beats)
        {
            if (!beat.gameObject.activeSelf || beat.resolved)
                continue;

            float beatX = beat.XPosition;
            if (previousX < beatX && currentX >= beatX)
            {
                beat.MarkResolved(QTEResult.Miss);
                MissCount++;
            }
        }
    }

    void ClearBeats()
    {
        foreach (var beat in beats)
        {
            if (beat != null)
                Destroy(beat.gameObject);
        }

        beats.Clear();
        currentBeatIndex = 0;
    }
}
