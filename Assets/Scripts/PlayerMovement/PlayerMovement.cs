using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement Instance;

    [Header("Movimiento")]
        public float moveSpeed = 40f;
        public float sprintSpeed = 1.8f;
        private float currentSpeed; 
    
    [Header("Salto / Gravedad")]
        public float jumpHeight = 3f;
        public float gravity = -9.81f;

        private CharacterController characterController;
        

        [SerializeField] private Vector2 moveInput;
        public float verticalVelocity;
        public bool jumpRequested = false;

        [SerializeField] private AudioSource audioSourceJump;
        [SerializeField] private AudioSource audioSourceSteps;
        [SerializeField] private int minSpeedSound = 1;

        [SerializeField] private Animator animator;

        [SerializeField]    
            //private HacerseHijoMano hacerseHijoMano;

        
        private bool runPress;
        public bool canMove = true;


        

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;

        characterController = GetComponent<CharacterController>();
        currentSpeed = moveSpeed;
    }

    private void OnMove(InputValue value)
    {
        if (!canMove) 
        {
            moveInput = Vector2.zero;
            return;
        }
        moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (!canMove) return;

        if(value.isPressed)
        {
            jumpRequested = true;
        }
    }

    private void OnPunch(InputValue value)
    {
        
    }

    /*private void OnThrowStick(InputValue value)
    {
        Debug.Log("OnSoltarPalo");
        if(value.isPressed)
        {
            Debug.Log("Soltar Palo");
            hacerseHijoMano.SoltarPalo();
        }
    }
*/
    // Update is called once per frame
    void Update()
    {
        if(!canMove)
        {
            animator.SetFloat("VerticalVelocity", 0);
            return;
        }

        if(characterController == null)
            return; 

        
        MovementControl();

        animator.SetFloat("VerticalVelocity", verticalVelocity);

        StepsSound();
    }

    private void StepsSound()
    {
        if (!canMove)
        {
            if (audioSourceSteps != null && audioSourceSteps.isPlaying)
                audioSourceSteps.Stop();
            return;
        }

        if(audioSourceSteps == null)
            return;
        
        Vector3 v = characterController.velocity;
        v.y = 0;
        bool andando = characterController.isGrounded && v.magnitude > minSpeedSound;
        if(andando)
        {
            if(!audioSourceSteps.isPlaying)
                audioSourceSteps.Play();
        }
        else if(audioSourceSteps.isPlaying)
            audioSourceSteps.Stop();
        
    }

    private void MovementControl()
    {
        bool isGrounded = characterController.isGrounded;
        // Reset vertical al tocar el suelo
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; 
        }

        // Movimiento local
        Vector3 localMove = new Vector3(moveInput.x, 0, moveInput.y);

        // Convertir de local a mundo
        Vector3 worldMove = transform.TransformDirection(localMove);

        if(worldMove.sqrMagnitude > 1f)
        {
            worldMove.Normalize();
        }

        bool runPress = InputSystem.GetDevice<Keyboard>().leftShiftKey.isPressed;
        currentSpeed = runPress ? moveSpeed * sprintSpeed : moveSpeed;
        Vector3 horizontalVelocity = worldMove * currentSpeed;

        // Salto
        if (isGrounded && jumpRequested)
        {
            if(audioSourceJump != null)
                audioSourceJump.Play();

            animator.SetTrigger("Jump");
            
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpRequested = false;
        }



        verticalVelocity += gravity * Time.deltaTime;
        //Vector3 velocity = horizontalVelocity;
        //velocity.y = verticalVelocity;
        horizontalVelocity.y = verticalVelocity;
        characterController.Move(horizontalVelocity * Time.deltaTime);
    }


}
