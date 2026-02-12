using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class InitialMenu : MonoBehaviour
{



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
        Debug.Log("Iniciar Juego");
        TransitionManager.Instance.LoadSceneWithFade("GameScene");
    }

    public void Quit()
    {
        Debug.Log("Salir Juego");
        Application.Quit();
    }
}
