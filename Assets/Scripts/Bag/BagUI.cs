using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;

public class BagUI : MonoBehaviour
{

    public GameObject itemPrefab;
    public Transform itemsContainer;
    public Transform abilitiesContainer;

    public GameObject submenuUI;
    public TextMeshProUGUI submenuText;
    public TextMeshProUGUI optionEquip; 
    public TextMeshProUGUI optionCancel;
    private int submenuIndex = 0; 
    private bool submenuOpen = false;
    private BagItemUI currentSelectedItem;

    private int nextAbilitySlot = 0;

    private List<BagItemUI> weaponItems = new ();
    private List<BagItemUI> abilityItems = new ();
    

    private int selectedIndex = 0;
    private bool weaponsSelected = true;

    private Keyboard keyboard;

    void OnEnable()
    {
        keyboard = Keyboard.current;
        submenuUI.SetActive(false);
        StartCoroutine(RefreshNextFrame());
    }

    IEnumerator RefreshNextFrame()
    {
        yield return null; // espera un frame

        if (Bag.Instance != null)
        {
            Refresh();
        }
    }


    public void Refresh()
    {
        ClearContainer(itemsContainer, weaponItems);
        ClearContainer(abilitiesContainer, abilityItems);

        SpawnItems(Bag.Instance.GetItems(), itemsContainer, weaponItems);
        SpawnItems(Bag.Instance.GetAbilities(), abilitiesContainer, abilityItems);

        selectedIndex = 0;
        weaponsSelected = true;
        UpdateSelection();

    }

    void ClearContainer(Transform container, List<BagItemUI> list)
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);
        list.Clear();
    }

    void SpawnItems(List<ItemSO> items, Transform container, List<BagItemUI> list)
    {
        foreach (ItemSO item in items)
        {
            GameObject go = Instantiate(itemPrefab, container);
            BagItemUI itemUI = go.GetComponent<BagItemUI>();
            itemUI.SetText(item.itemName);
            list.Add(itemUI);
        }
    }

    void Update()
    {
        if(keyboard == null) return;

        //Change selection (weapons/abilities)
        if(keyboard.tabKey.wasPressedThisFrame)
        {
            weaponsSelected = !weaponsSelected;
            selectedIndex = 0;
            UpdateSelection();
        }
        List<BagItemUI> currentList = weaponsSelected ? weaponItems : abilityItems;

        if(currentList.Count == 0) return;

        if(keyboard.sKey.wasPressedThisFrame)
        {
            selectedIndex++;
            if(selectedIndex >= currentList.Count) 
                selectedIndex = 0;
            UpdateSelection();
        }

        if(keyboard.wKey.wasPressedThisFrame)
        {
            selectedIndex--;
            if(selectedIndex < 0) 
                selectedIndex = currentList.Count - 1;
            UpdateSelection();
        }

        if(keyboard.spaceKey.wasPressedThisFrame && !submenuOpen)
        {
            if(currentList.Count > 0)
                MenuManager.Instance.SubMenuUsing = true;
                OpenSubmenu(currentList[selectedIndex]);
        }

        if(submenuOpen)
        {
            // Change submenu selection
            if(keyboard.upArrowKey.wasPressedThisFrame || keyboard.downArrowKey.wasPressedThisFrame)
            {
                submenuIndex = 1 - submenuIndex; 
                UpdateSubmenuSelection();
            }

            // Confirm
            if(keyboard.enterKey.wasPressedThisFrame)
            {
                if(submenuIndex == 0) // Equip
                {
                    EquipItem(currentSelectedItem);
                }
                CloseSubmenu();
            }

            // Cancel with Escape
            if(keyboard.escapeKey.wasPressedThisFrame)
            {
                CloseSubmenu();
            }
            }
    }

    void UpdateSelection()
    {
        UpdateList(weaponItems, weaponsSelected);
        UpdateList(abilityItems, !weaponsSelected);
    }

    void UpdateList(List<BagItemUI> list, bool active)
    {
        for (int i = 0; i < list.Count; i++)
        {
            bool selected = active && i == selectedIndex;
            list[i].SetSelected(selected);
        }
    }

    void OpenSubmenu(BagItemUI itemUI)
    {
        currentSelectedItem = itemUI;
        submenuUI.SetActive(true);
        submenuOpen = true;
        MenuManager.Instance.SubMenuUsing = true;
        submenuIndex = 0;

        string itemName = itemUI.text.text; 
        submenuText.text = $"Equipar {(weaponsSelected ? "Instrumento" : "Habilidad")}: {itemName}";

        UpdateSubmenuSelection();
    }

    public void CloseSubmenu()
    {
        submenuUI.SetActive(false);
        submenuOpen = false;
        MenuManager.Instance.SubMenuUsing = false;
        currentSelectedItem = null;
    }

    void EquipItem(BagItemUI itemUI)
    {
        if (weaponsSelected)
        {
            ItemSO weapon = Bag.Instance.GetItems().Find(i => i.itemName == itemUI.text.text);

            if (weapon != null)
                EquipmentManager.Instance.EquipWeapon(weapon);
        }
        else
        {
            ItemSO ability = Bag.Instance.GetAbilities().Find(i => i.itemName == itemUI.text.text);

            if (ability != null)
                EquipmentManager.Instance.EquipAbility(ability);
        }
    }
    void UpdateSubmenuSelection()
    {
        optionEquip.color = submenuIndex == 0 ? Color.yellow : Color.white;
        optionCancel.color = submenuIndex == 1 ? Color.yellow : Color.white;
    }
}
