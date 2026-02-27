using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class InitialMenu : MonoBehaviour
{

    public AudioSource audioSelectMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        if (audioSelectMenu != null)
            audioSelectMenu.Play();
        TransitionManager.Instance.LoadSceneWithFade("GameScene");
    }

    public void Quit()
    {
        if (audioSelectMenu != null)
            audioSelectMenu.Play();
        Application.Quit();
    }
}
