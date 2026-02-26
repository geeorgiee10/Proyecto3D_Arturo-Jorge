using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator animator;

    public enum CharacterType
    {
        Hero,
        Villain
    }

    private Vector3 pos;

    public CharacterType characterType;

    private bool Win;

    void Start()
    {
        bool gameWin = FinalManager.Instance.gameWin;

        if (characterType == CharacterType.Hero)
            Win = gameWin;
        else
            Win = !gameWin;

        animator.SetBool("isWin", Win);

        if (Win)
        {
            PlayWinAnimation();
        }

        pos = transform.position;
    }

    void Update()
    {
        //PlayWinAnimation();
        transform.position = pos;
    }

    public void PlayWinAnimation()
    {
        int numRandom = Random.Range(1, 5);
        animator.SetInteger("Random", numRandom);
    }
}