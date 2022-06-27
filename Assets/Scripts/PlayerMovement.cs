using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;

    Vector2 moveInput;
    Rigidbody2D playerRB2D;

    void Start()
    {

        playerRB2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        Run();
        FlipSprite();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        // Debug.Log(moveInput.x);
        // Debug.Log(moveInput.y);
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, playerRB2D.velocity.y);
        Debug.Log(playerVelocity);
        playerRB2D.velocity = playerVelocity;
    }

    void FlipSprite() {
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRB2D.velocity.x) > Mathf.Epsilon;

        if(playerHasHorizontalSpeed) {
            transform.localScale = new Vector2(Mathf.Sign(-playerRB2D.velocity.x), 1.0f);
        }
    }

}