using TMPro;
using UnityEngine;

public class FinalManager : MonoBehaviour
{
    public static FinalManager Instance;

    public bool gameWin;

    public AudioSource loserMusic;

    public AudioSource winnerMusic;

    public TextMeshProUGUI textResult;

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
        if(gameWin == true)
        {
            textResult.text = "Enhorabuena\n Has Ganado";
            if(winnerMusic != null)
                winnerMusic.Play();   
        }
        else
        {
            textResult.text = "Lo Siento\n Has Perdido";
            if(loserMusic != null)
                loserMusic.Play();
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    
}
