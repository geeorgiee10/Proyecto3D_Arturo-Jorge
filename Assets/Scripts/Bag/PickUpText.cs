using System.Collections;
using TMPro;
using UnityEngine;

public class PickUpText : MonoBehaviour
{
    public static PickUpText Instance;

    public TextMeshProUGUI pickUpText;
    public float floatSpeed = 50f;
    public float duration = 1.5f;

    private Vector3 originalPosition;

    void Awake()
    {
        Instance = this;
        //originalPosition = pickUpText.rectTransform.localPosition;
    }

    public void showText(string nameItem)
    {

        Debug.Log("Mostrando texto: " + nameItem);
        pickUpText.gameObject.SetActive(true);
        StopAllCoroutines();
        pickUpText.text = "Has recogido: " + nameItem;
        //pickUpText.rectTransform.localPosition = originalPosition;
        pickUpText.gameObject.SetActive(true);

        StartCoroutine(FloatUp());
    }

    IEnumerator FloatUp()
    {
        float time = 0f;

        while(time < duration)
        {
            time += Time.deltaTime;
            pickUpText.rectTransform.localPosition += Vector3.up * floatSpeed * Time.deltaTime;
            yield return null;
        }

        pickUpText.gameObject.SetActive(false);
    }
}
