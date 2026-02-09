using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;

public class BagUI : MonoBehaviour
{

    public GameObject itemTextPrefab;
    public Transform itemsContainer;
    public Transform abilitiesContainer;

    private List<TextMeshProUGUI> weaponTexts = new ();
    private List<TextMeshProUGUI> abilityTexts = new ();

    private int selectedIndex = 0;
    private bool weaponsSelected = true;

    private Keyboard keyboard;

    void OnEnable()
    {
        keyboard = Keyboard.current;
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
        ClearContainer(itemsContainer, weaponTexts);
        ClearContainer(abilitiesContainer, abilityTexts);

        SpawnItems(Bag.Instance.GetItems(), itemsContainer, weaponTexts);
        SpawnItems(Bag.Instance.GetAbilities(), abilitiesContainer, abilityTexts);

        selectedIndex = 0;
        weaponsSelected = true;
        UpdateSelection();

    }

    void ClearContainer(Transform container, List<TextMeshProUGUI> list)
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);
        list.Clear();
    }

    void SpawnItems(List<ItemSO> items, Transform container, List<TextMeshProUGUI> list)
    {
        foreach (ItemSO item in items)
        {
            GameObject go = Instantiate(itemTextPrefab, container);
            TextMeshProUGUI tmp = go.GetComponent<TextMeshProUGUI>();
            tmp.text = item.itemName;
            list.Add(tmp);
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
        List<TextMeshProUGUI> currentList = weaponsSelected ? weaponTexts : abilityTexts;

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
    }

    void UpdateSelection()
    {
        Highlight(weaponTexts, weaponsSelected);
        Highlight(abilityTexts, !weaponsSelected);
    }

    void Highlight(List<TextMeshProUGUI> list, bool active)
    {
        for(int i = 0; i < list.Count; i++)
        {
            list[i].color = 
                (active && i == selectedIndex) ? Color.yellow : Color.white;
        }
    }
}
