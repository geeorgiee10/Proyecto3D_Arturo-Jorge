using UnityEngine;
using UnityEngine.InputSystem;


public class NPCDialogue : MonoBehaviour
{
    public DialogueSO dialogue;
    private bool playerInRange;
    private bool dialogueStarted;

    private Keyboard keyboard;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        keyboard = Keyboard.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerInRange || keyboard == null) return;

        if (!dialogueStarted && keyboard.eKey.wasPressedThisFrame)
        {
            TalkIndicator.Instance.Hide();
            DialogueManager.Instance.StartDialogue(dialogue);
            dialogueStarted = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;

            if (!DialogueManager.Instance.dialoguePanel.activeSelf)
                TalkIndicator.Instance.Show();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogueStarted = false; 

            TalkIndicator.Instance.Hide();
        }
    }
}