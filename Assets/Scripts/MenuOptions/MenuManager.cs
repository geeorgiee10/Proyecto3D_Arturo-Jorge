using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{

    public GameObject menuPanel;
    public Transform optionsContainer;
    private List<TextMeshProUGUI> options = new List<TextMeshProUGUI>();
    private int selectedIndex = 0;

    private Keyboard keyboard;
    private bool menuActive = false;

    [Header("Animación")]
    public float slideSpeed = 800f;
    private Vector3 hiddenPos;
    private Vector3 visiblePos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        keyboard = Keyboard.current;

       
        foreach (Transform t in optionsContainer)
        {
            TextMeshProUGUI tmp = t.GetComponent<TextMeshProUGUI>();
            if(tmp != null)
                options.Add(tmp);
        }

    
        visiblePos = menuPanel.transform.localPosition;
        hiddenPos = visiblePos + Vector3.right * (menuPanel.GetComponent<RectTransform>().rect.width + 100);
        menuPanel.transform.localPosition = hiddenPos;

        UpdateSelection();
    }

    // Update is called once per frame
    void Update()
    {
         if(keyboard == null) return;

        // Abrir/cerrar menú
        if(keyboard.escapeKey.wasPressedThisFrame)
        {
            if(menuActive)
                StartCoroutine(HideMenu());
            else
                StartCoroutine(ShowMenu());
        }

        if(!menuActive) return;

        // Navegar opciones
        /*if(keyboard.tabKey.wasPressedThisFrame)
        {
            selectedIndex--;
            if(selectedIndex < 0) selectedIndex = options.Count - 1;
            UpdateSelection();
        }*/
        if(keyboard.tabKey.wasPressedThisFrame)
        {
            selectedIndex++;
            if(selectedIndex >= options.Count) selectedIndex = 0;
            UpdateSelection();
        }

        // Seleccionar opción
        if(keyboard.enterKey.wasPressedThisFrame)
        {
            SelectOption();
        }
    }

    void UpdateSelection()
    {
        for(int i=0; i<options.Count; i++)
        {
            if(i == selectedIndex)
                options[i].color = Color.yellow;
            else
                options[i].color = Color.white;
        }
    }

    void SelectOption()
    {
        string choice = options[selectedIndex].text;
        Debug.Log("Seleccionaste: " + choice);

        // Lógica de cada opción
        switch(choice)
        {
            case "Bag":
                // Abrir mochila
                break;
            case "Controls":
                // Mostrar controles
                break;
            case "Exit":
                // Volver menu inicial
                break;
        }
    }

    IEnumerator ShowMenu()
    {
        menuActive = true;
        menuPanel.SetActive(true);
        while(Vector3.Distance(menuPanel.transform.localPosition, visiblePos) > 0.1f)
        {
            menuPanel.transform.localPosition = Vector3.MoveTowards(menuPanel.transform.localPosition, visiblePos, slideSpeed * Time.deltaTime);
            yield return null;
        }
        menuPanel.transform.localPosition = visiblePos;
    }

    IEnumerator HideMenu()
    {
        while(Vector3.Distance(menuPanel.transform.localPosition, hiddenPos) > 0.1f)
        {
            menuPanel.transform.localPosition = Vector3.MoveTowards(menuPanel.transform.localPosition, hiddenPos, slideSpeed * Time.deltaTime);
            yield return null;
        }
        menuPanel.transform.localPosition = hiddenPos;
        menuPanel.SetActive(false);
        menuActive = false;
    }
}
