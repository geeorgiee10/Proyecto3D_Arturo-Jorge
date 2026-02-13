using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QTEManager : MonoBehaviour
{
    [Header("Beat Data")]
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

    private int beatPoints = 0;


    [Header("Validate Beat")]
    [SerializeField] private HitFeedbackUI feedbackPrefab;
    [SerializeField] private RectTransform feedbackParent;

    [Header("Sounds")]
    [SerializeField] private AudioSource[] missSounds;

    [SerializeField] private AudioSource goodSound;

    [SerializeField] private AudioSource perfect1;
    [SerializeField] private AudioSource perfect2;
    [SerializeField] private AudioSource perfect3;
    [SerializeField] private AudioSource perfect4;
    [SerializeField] private AudioSource perfect5;
    [SerializeField] private AudioSource perfect6;

    private int perfectCombo = 0;

    [Header("QTE Effects")]
    private bool grooveActive = false;
    private bool extasisActive = false;
    private bool microtoneActive = false;

    public void SetModifiers(bool groove, bool extasis, bool microtone)
    {
        grooveActive = groove;
        extasisActive = extasis;
        microtoneActive = microtone;
    }


    public void ShowBeats(QTEPattern pattern)
    {
        beatPoints = 0;
        beats.Clear();
        perfectCombo = 0;
        currentBeatIndex = 0;

        PerfectCount = 0;
        GoodCount = 0;
        MissCount = 0;

        float timelineSize = timeline.sizeDelta.x;
        beatPoints = pattern.sequence.Length + 1;

        float spacing = timelineSize / beatPoints;
        float lastPosition = spacing - (timelineSize / 2f);

        foreach (QTEInput qteI in pattern.sequence)
        {
            BeatUI beat = Instantiate(beatPrefab, timeline);
            beat.Setup(qteI, spacing, lastPosition, grooveActive);

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
        if (!AnyQTEKeyPressed())
            return;

        if (currentBeatIndex >= beats.Count)
            return;

        BeatUI beat = beats[currentBeatIndex];

        if (beat.resolved)
            return;

        float barX = progressBar.anchoredPosition.x;
        float distance = Mathf.Abs(barX - beat.XPosition);

        if (distance <= goodWindow)
        {
            if (IsValidKeyPressed(beat.ExpectedKey))
            {
                EvaluateHit(beat, distance);
            }
            else
            {
                RegisterMiss(beat);
            }
        }
        else
        {
            // RegisterMiss(beat);
            return;
        }
    }
    
    bool IsValidKeyPressed(QTEKey expectedKey)
    {
        if (grooveActive)
            return Input.GetKeyDown(KeyCode.A) ||
                Input.GetKeyDown(KeyCode.S) ||
                Input.GetKeyDown(KeyCode.D) ||
                Input.GetKeyDown(KeyCode.F);

        // Comportamiento normal
        return expectedKey switch
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
        QTEResult result;

        if (distance <= perfectWindow)
            result = QTEResult.Perfect;

        else if (distance <= goodWindow)
            result = QTEResult.Good;

        else
            result = QTEResult.Miss;

        if (result == QTEResult.Good)
            if (extasisActive && microtoneActive)
                result = QTEResult.Good;

            else if (extasisActive)
                result = QTEResult.Perfect;

            else if (microtoneActive)
                result = QTEResult.Miss;

        beat.MarkResolved(result);
        ShowFeedback(result, beat);

        switch (result)
        {
            case QTEResult.Perfect:
                PerfectCount++;
                perfectCombo++;
                PlayPerfectComboSound();
                break;

            case QTEResult.Good:
                GoodCount++;
                goodSound.Play();
                perfectCombo = 0;
                break;

            case QTEResult.Miss:
                MissCount++;
                missSounds[Random.Range(0, missSounds.Length)].Play();
                perfectCombo = 0;
                break;
        }
        currentBeatIndex++;
    }

    IEnumerator MoveProgressBar()
    {
        float left = -timeline.sizeDelta.x / 2f;
        float right = timeline.sizeDelta.x / 2f;

        progressBar.anchoredPosition = new Vector2(left, progressBar.anchoredPosition.y);
        float previousX = progressBar.anchoredPosition.x;

        while (progressBar.anchoredPosition.x < right)
        {
            progressBar.anchoredPosition += Vector2.right * ((6*progressBarSpeed)/beatPoints) * Time.deltaTime;
            float currentX = progressBar.anchoredPosition.x;

            CheckBeats(previousX, currentX);

            previousX = currentX;
            yield return null;
        }

        ClearBeats();
    }

    void CheckBeats(float previousX, float currentX)
    {
        if (currentBeatIndex >= beats.Count)
            return;

        BeatUI beat = beats[currentBeatIndex];

        if (beat.resolved)
            return;

        float beatX = beat.XPosition;

        if (previousX < beatX && currentX >= beatX && !beat.resolved)
        {
            RegisterMiss(beat);
        }
    }

    bool AnyQTEKeyPressed()
    {
        return Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.F);
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

    void ShowFeedback(QTEResult result, BeatUI beat)
    {
        HitFeedbackUI feedback = Instantiate(feedbackPrefab, feedbackParent);

        RectTransform feedbackRect = feedback.GetComponent<RectTransform>();
        RectTransform beatRect = beat.GetComponent<RectTransform>();

        
        Vector2 pos = beatRect.anchoredPosition;
        pos.y += 30f;

        feedbackRect.anchoredPosition = pos;

        feedback.Play(result, perfectCombo+1);
    }

    void PlayPerfectComboSound()
    {
        int comboIndex = Mathf.Clamp(perfectCombo, 1, 6);

        switch (comboIndex)
        {
            case 1: perfect1?.Play(); break;
            case 2: perfect2?.Play(); break;
            case 3: perfect3?.Play(); break;
            case 4: perfect4?.Play(); break;
            case 5: perfect5?.Play(); break;
            case 6: perfect6?.Play(); break;
        }
    }

    void RegisterMiss(BeatUI beat)
    {
        if (beat.resolved)
            return;

        beat.MarkResolved(QTEResult.Miss);
        ShowFeedback(QTEResult.Miss, beat);

        MissCount++;
        perfectCombo = 0;

        if (missSounds.Length > 0)
            missSounds[Random.Range(0, missSounds.Length)].Play();
            
        currentBeatIndex++;
    }
}

/*

    ToDo:

    CUANDO SE HACE UN PERFECTO PONER EL SONIDO DE COMBO DEL JET SET RADIO

*/
