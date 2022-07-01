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
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;
    float playerStartingGravity;
    bool isAlive = true;
    bool isGrounded;
    bool isLaddering;
    [SerializeField] Vector2 hitImpact = new Vector2(2f, 2f);

    void Start() {
        playerRB2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
        playerStartingGravity = playerRB2D.gravityScale;
    }

    void Update() {
        if(!isAlive) { return; }
        Run();
        FlipSprite();
        Jump();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value) {
        if(!isAlive) { return; }

        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value) {
        if(!isAlive) { return; }

        if(!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
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

        if(playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            isGrounded = true;
        } else if (playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) {
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

        isLaddering = playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));

        if(!isLaddering) {
            playerRB2D.gravityScale = playerStartingGravity;
            return;
        }

        if(isLaddering) {
            playerAnimator.SetBool("isJumping", false);
        }

        Vector2 climbVelocity = new Vector2(playerRB2D.velocity.x, moveInput.y * climbSpeed);
        playerRB2D.velocity = climbVelocity;
        playerRB2D.gravityScale = 0f;
        bool climbingLadder = Mathf.Abs(playerRB2D.velocity.y) > Mathf.Epsilon;
        playerAnimator.SetBool("isLaddering", climbingLadder);
    }

    // Sprite functionality
    void FlipSprite() {
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRB2D.velocity.x) > Mathf.Epsilon;

        if(playerHasHorizontalSpeed) {
            transform.localScale = new Vector2(Mathf.Sign(playerRB2D.velocity.x), 1.0f);
        }
    }

    void Die() {
        if(playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy"))) {
            isAlive = false;
            playerAnimator.SetTrigger("isHurt");
            playerRB2D.velocity = hitImpact;
        } 
    }

}