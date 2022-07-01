using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{   
    Rigidbody2D enemyRB2D;
    BoxCollider2D enemyWallCollider;
    Animator enemyAnimator;
    [SerializeField] float moveSpeed = 1f;


    void Start()
    {
        enemyRB2D = GetComponent<Rigidbody2D>();
        enemyWallCollider = GetComponent<BoxCollider2D>();
        enemyAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        enemyRB2D.velocity = new Vector2(moveSpeed, 0); 
        Run();

    }

    void OnTriggerExit2D(Collider2D other) {
        moveSpeed = -moveSpeed;
        FlipEnemeyFacing();
    }     

    void FlipEnemeyFacing() {
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRB2D.velocity.x)), 1.0f);
    }

    void Run() {
        if(Mathf.Abs(enemyRB2D.velocity.x) > Mathf.Epsilon) {
            enemyAnimator.SetBool("isWalking", true);
        } else {
            enemyAnimator.SetBool("isWalking", false);
        }
    }
}
