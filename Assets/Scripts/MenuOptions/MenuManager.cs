using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public static MenuManager Instance;

    public GameObject menuPanel;
    public GameObject controlsPanel;
    public Transform optionsContainer;
    private List<TextMeshProUGUI> options = new List<TextMeshProUGUI>();
    private int selectedIndex = 0;

    private Keyboard keyboard;
    private bool menuActive = false;

    [Header("Animación")]
    public float slideSpeed = 800f;
    private Vector3 hiddenPos;
    private Vector3 visiblePos;

    public GameObject bagUI;

    public bool SubMenuUsing = false;

    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        bagUI.SetActive(false);
        controlsPanel.SetActive(false);

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
        if(keyboard.escapeKey.wasPressedThisFrame && !SubMenuUsing)
        {
            if(menuActive)
                StartCoroutine(HideMenu());
            else
                StartCoroutine(ShowMenu());
        }

        if(keyboard.oKey.wasPressedThisFrame)
        {
            controlsPanel.SetActive(false);
        }

        if(!menuActive) return;

        // Navegar opciones
        if(keyboard.wKey.wasPressedThisFrame)
        {
            selectedIndex--;
            if(selectedIndex < 0) selectedIndex = options.Count - 1;
            UpdateSelection();
        }
        if(keyboard.sKey.wasPressedThisFrame)
        {
            selectedIndex++;
            if(selectedIndex >= options.Count) selectedIndex = 0;
            UpdateSelection();
        }

        // Seleccionar opción
        if(!bagUI.activeSelf && keyboard.enterKey.wasPressedThisFrame && !SubMenuUsing)
        {
            SelectOption();
        }


        if (bagUI.activeSelf && keyboard.escapeKey.wasPressedThisFrame && !SubMenuUsing)
        {
            bagUI.SetActive(false);
            menuPanel.SetActive(true);
            return;
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

        // Lógica de cada opción
        switch(selectedIndex)
        {
            case 0:
                OpenBag();
                break;
            case 1:
                controlsPanel.SetActive(true);
                break;
            case 2:
                // Volver menu inicial
                TransitionManager.Instance.LoadSceneWithFade("SampleScene");
                break;
        }
    }



    void OpenBag()
    {
        PlayerMovement.Instance.canMove = false;
        bagUI.SetActive(true);
        //menuPanel.SetActive(false);
    }

    void CloseBag()
    {
        PlayerMovement.Instance.canMove = true;
        bagUI.SetActive(false);
        //menuPanel.SetActive(true);
    }

    IEnumerator ShowMenu()
    {
        PlayerMovement.Instance.canMove = false;
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
        PlayerMovement.Instance.canMove = true;
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
