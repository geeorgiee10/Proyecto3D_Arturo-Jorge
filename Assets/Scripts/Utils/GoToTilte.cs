using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DelayedSceneLoader : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadAfterDelay());
    }

    IEnumerator LoadAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("SampleScene");
    }
}