using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float climbSpeed = 5f;

    Vector2 moveInput;
    Rigidbody2D playerRB2D;
    Animator playerAnimator;
    CapsuleCollider2D playerCollider;
    bool isGrounded;
    bool isClimbing;

    void Start() {
        playerRB2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update() {
        Run();
        FlipSprite();
        Jump();
        ClimbLadder();
    }

    void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value) {

        if(!playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            return;
        }

        if(value.isPressed) {
            playerRB2D.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    // Player actions
    void Run() {   
        float isMoving = Mathf.Sign(playerRB2D.velocity.x);
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, playerRB2D.velocity.y);
        playerRB2D.velocity = playerVelocity;

        if(Mathf.Abs(playerRB2D.velocity.x) > Mathf.Epsilon) {
            playerAnimator.SetBool("isRunning", true);
        } else {
            playerAnimator.SetBool("isRunning", false);
        }
    }

    void Jump() {

        if(playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }

        if(!isGrounded) {
            playerAnimator.SetBool("isJumping", true);
        } else {
            playerAnimator.SetBool("isJumping", false);
        }
    }

    void ClimbLadder() {

        if(!playerCollider.IsTouchingLayers(LayerMask.GetMask("Climb"))) {
            isClimbing = false;
        } else {
            isClimbing = true;
        }

        if(isClimbing) {
            playerAnimator.SetBool("isClimbing", true);
        } else {
            playerAnimator.SetBool("isClimbing", false);
        }
        
        Vector2 playerVelocity = new Vector2(playerRB2D.velocity.x, moveInput.y * climbSpeed);
        playerRB2D.velocity = climbVelocity;
    }

    // Sprite functionality
    void FlipSprite() {
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRB2D.velocity.x) > Mathf.Epsilon;

        if(playerHasHorizontalSpeed) {
            transform.localScale = new Vector2(Mathf.Sign(playerRB2D.velocity.x), 1.0f);
        }
    }

}