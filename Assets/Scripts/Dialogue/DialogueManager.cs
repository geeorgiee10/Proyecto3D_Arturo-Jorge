using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{

    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;

    
    [Header("Texto")]
        public float typingSpeed = 0.04f;

    private string[] lines;
    private int index;
    private bool dialogueActive;
    private bool justStarted;

    private Coroutine typingCoroutine;
    private bool isTyping;

    
    private Keyboard keyboard;
    void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        keyboard = Keyboard.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dialogueActive || keyboard == null) return;

        if (justStarted)
        {
            if (!keyboard.eKey.isPressed)
                justStarted = false;

            return;
        }

        if (keyboard.eKey.wasPressedThisFrame)
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = lines[index];
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    public void StartDialogue(DialogueSO dialogue)
    {
        dialogueActive = true;
        justStarted = true;

        dialoguePanel.SetActive(true);

        npcNameText.text = dialogue.npcName;
        lines = dialogue.lines;
        index = 0;

        StartTypingLine(lines[index]);

        PlayerMovement.Instance.canMove = false;
    }

    void NextLine()
    {
        index++;

        if (index < lines.Length)
        {
            StartTypingLine(lines[index]);
        }
        else
        {
            EndDialogue();
        }
    }

    void StartTypingLine(string line)
    {
        if(typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        dialogueActive = false;
        dialoguePanel.SetActive(false);

        PlayerMovement.Instance.canMove = true;
    }
}
