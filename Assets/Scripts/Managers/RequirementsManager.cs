using UnityEngine;
using TMPro;

public class RequirementsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            txt.gameObject.SetActive(true);
            CheckRequirements();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            txt.gameObject.SetActive(false);
        }
    }

    private void CheckRequirements()
    {
        if(WorldData.Instance.completedEvents.Contains("minion")) return;

        string res = "Queda por derrotar a:\n";

        if (!WorldData.Instance.completedEvents.Contains("paquirrín"))
            res += "- Paquirrín \n";

        if (!WorldData.Instance.completedEvents.Contains("cigala"))
            res += "- El Cigala \n";

        if (!WorldData.Instance.completedEvents.Contains("antonio"))
            res += "- Antonio Flores \n";

        if (!WorldData.Instance.completedEvents.Contains("fariV1"))
            res += "- El Fari \n";

        if (!WorldData.Instance.completedEvents.Contains("minion"))
            res += "- Secuaces \n";

        txt.text = res;
    }

    void Update()
    {
        bool finished =  WorldData.Instance.completedEvents.Contains("paquirrín")
                        && WorldData.Instance.completedEvents.Contains("cigala")
                        && WorldData.Instance.completedEvents.Contains("antonio")
                        && WorldData.Instance.completedEvents.Contains("fariV1")
                        && WorldData.Instance.completedEvents.Contains("minion");

        gameObject.SetActive(!finished);    
    }
}
