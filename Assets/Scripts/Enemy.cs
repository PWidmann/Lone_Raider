using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IMonster
{
    Animator animator;
    Rigidbody2D rb2D;
    Vector2 movement;

    Vector2 startPosition;
    Vector2 nextPosition;

    public enum EnemyState { Wandering, Waiting, Attacking }

    public EnemyState enemyState;

    public float speed = 4f;

    private int wayPointCount = 0;
    private float wayPointTimer = 0;
    private float waitTimer = 0;
    private float maxWaitTimer = 3f;


    private void Awake()
    {
        gameObject.transform.GetComponent<Renderer>().enabled = false;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        

        enemyState = EnemyState.Wandering;

        startPosition = transform.position;

        nextPosition = NewRandomPosition();
    }

    private void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Wandering:
                Wandering();
                break;
            case EnemyState.Waiting:
                Waiting();
                break;
        }   
    }

    private void Waiting()
    {
        animator.SetBool("isMoving", false);

        waitTimer += Time.deltaTime;

        if (waitTimer >= maxWaitTimer)
        {
            waitTimer = Random.Range(0f, maxWaitTimer);
            enemyState = EnemyState.Wandering;
        }
    }

    private void Wandering()
    {
        animator.SetBool("isMoving", true);

        float distance = Vector2.Distance(transform.position, nextPosition);

        wayPointTimer += Time.deltaTime;

        if (distance <= 0.2f)
        {
            
            wayPointCount++;
            if (wayPointCount == 7)
            {
                nextPosition = startPosition;
                wayPointCount = 0;
            }
            else
            {

                nextPosition = NewRandomPosition();
                enemyState = EnemyState.Waiting;
            }
        }
        else
        {
            if (wayPointTimer > 5f)
            {
                nextPosition = NewRandomPosition();
                wayPointTimer = 0;
            }
            else
            {
                Vector2 direction = (nextPosition - new Vector2(transform.position.x, transform.position.y)).normalized;
                Movement(direction);
            }
        }
    }

    Vector2 NewRandomPosition()
    {
        return startPosition + new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * Random.Range(1.0f, 4.0f);
    }

    private void Movement(Vector2 direction)
    {
        movement = direction * speed * Time.deltaTime;

        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetFloat("horizontalMovement", direction.x);
            animator.SetFloat("verticalMovement", direction.y);
            transform.Translate(new Vector3(movement.x, movement.y, 0));
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.GetComponent<IInteractible>() != null)
        {
            collider.gameObject.GetComponent<IInteractible>().Interact();
        }
    }

    public void SetVisible()
    {
        
    }
}
