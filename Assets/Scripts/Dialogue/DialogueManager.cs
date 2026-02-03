using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{

    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;

    private string[] lines;
    private int index;
    private bool dialogueActive;
    private bool justStarted;

    
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
            NextLine();
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

        dialogueText.text = lines[index];

        PlayerMovement.Instance.canMove = false;
    }

    void NextLine()
    {
        index++;

        if (index < lines.Length)
        {
            dialogueText.text = lines[index];
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialogueActive = false;
        dialoguePanel.SetActive(false);

        PlayerMovement.Instance.canMove = true;
    }
}
