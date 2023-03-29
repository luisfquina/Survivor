using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayerScript : MonoBehaviour
{
    private PlayerInput playerInput;
    private CharacterController characterController;
    private Animator animator;

    private Vector2 characterMovementInput;
    private Vector3 characterMovement;
    private bool isMoving;
    
    [SerializeField] private float rotationVelocity = 5f;
    [SerializeField] private float velocity = 10f;

    private void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();



        playerInput.Movement.Walk.started += OnMovementInput;
        
        playerInput.Movement.Walk.canceled += OnMovementInput;

        playerInput.Movement.Walk.performed += OnMovementInput;
    }
    
    private void OnMovementInput(InputAction.CallbackContext context)
    {
        characterMovementInput = context.ReadValue<Vector2>();
        characterMovement.x = characterMovementInput.x;
        characterMovement.y = 0f;
        characterMovement.z = characterMovementInput.y;
        isMoving = characterMovementInput.y != 0 || characterMovementInput.x != 0 ? true : false;
    }
    void Update()
    {
        AnimationHandler();
        MovePlayer();
        PlayerRotationHandler();
    }

    private void AnimationHandler()
    {
        if (animator.GetBool("isWalking") == false && isMoving == true)
        {
            animator.SetBool("isWalking", true);
        }
        else if (animator.GetBool("isWalking") == true && isMoving == false)
        {
            animator.SetBool("isWalking", false);
        }

    }
    private void MovePlayer()
    {
        characterController.Move(characterMovement * Time.deltaTime * velocity);
    }

    private void PlayerRotationHandler()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = characterMovement.x;
        positionToLookAt.y = characterMovement.y;
        positionToLookAt.z = characterMovement.z;
        Quaternion currentRotation = transform.rotation;

        if (isMoving)
        {
            Quaternion lookAtRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, lookAtRotation, rotationVelocity * Time.deltaTime);
        }
    }

    private void OnEnable()
    {
        playerInput.Movement.Enable();
    }

    private void OnDisable()
    {
        playerInput.Movement.Disable();
    }
}
