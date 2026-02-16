using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;


public class PickUpItem : MonoBehaviour
{
    public ItemSO item;

    private Keyboard keyboard;
    private bool playerInRange;           
    private bool itemPickedUp; 


    void Start()
    {
        keyboard = Keyboard.current;
    }

    void Update()
    {
        if (!playerInRange || keyboard == null || itemPickedUp) return;

        if (keyboard.bKey.wasPressedThisFrame)
        {
            ObjectIndicator.Instance.Hide();
            Bag.Instance.AddItem(item);    

            PickUpText.Instance.showText(item.itemName);

            itemPickedUp = true;                 
            Destroy(gameObject);                 
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (!itemPickedUp)
                ObjectIndicator.Instance.Show();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            ObjectIndicator.Instance.Hide();
        }
    }
}
