using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;

    [SerializeField]private float maxVelocity = 5f * 1.8f;

    private Vector3 localMovement;

    private float punchCooldown = 2.5f;
    private float lastPunchTime = -999f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
        if(animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        Vector3 velocity = characterController.velocity;
        localMovement = transform.InverseTransformDirection(velocity);
        float x = localMovement.x;
        float y = localMovement.z;

        if(maxVelocity > 0)
        {
            x /= maxVelocity;
            y /= maxVelocity;
        }

        animator.SetFloat("x", x);
        animator.SetFloat("y", y);
        animator.SetBool("Grounded", characterController.isGrounded);
    }

    private void OnPunch(InputValue value)
    {
        if(Time.time - lastPunchTime < punchCooldown)
            return;
    
        lastPunchTime = Time.time;
        animator.SetTrigger("Punch");
        
    }
}
