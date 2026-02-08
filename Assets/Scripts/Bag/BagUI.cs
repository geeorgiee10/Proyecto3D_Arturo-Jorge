using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;

public class BagUI : MonoBehaviour
{

    public GameObject itemTextPrefab;
    public Transform itemsContainer;

    void OnEnable()
    {
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
        foreach (Transform child in itemsContainer)
            Destroy(child.gameObject);

        List<ItemSO> items = Bag.Instance.GetItems();
        Debug.Log("Items en Bag.Instance: " + items.Count);

        Dictionary<ItemSO, int> counts = new Dictionary<ItemSO, int>();

        foreach (ItemSO item in items)
        {
            if (counts.ContainsKey(item))
                counts[item]++;
            else
                counts[item] = 1;
        }

        foreach (var pair in counts)
        {
            GameObject go = Instantiate(itemTextPrefab, itemsContainer);
            Debug.Log("Instanciado: " + pair.Key.itemName);
            TextMeshProUGUI text = go.GetComponent<TextMeshProUGUI>();

            if (pair.Value > 1)
                text.text = pair.Key.itemName + " x" + pair.Value;
            else
                text.text = pair.Key.itemName;
        }
    }
}
