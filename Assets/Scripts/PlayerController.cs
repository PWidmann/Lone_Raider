using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1.3f;
    

    Animator animator;
    Rigidbody2D rb2D;

    // Input
    Vector2 input;
    float horizontal;
    float vertical;
    Vector2 movement;



    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        CameraController.Instance.SetCameraTarget(gameObject);
    }
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        input = new Vector2(horizontal, vertical).normalized;
        
        if (input != Vector2.zero)
        {
            animator.SetFloat("movementX", horizontal);
            animator.SetFloat("movementY", vertical);
            animator.SetBool("isMoving", true);

            movement = input * moveSpeed * Time.deltaTime;
            //transform.Translate(movement);
            rb2D.transform.Translate(movement);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.GetComponent<IInteractible>() != null)
        {
            collider.gameObject.GetComponent<IInteractible>().Interact();
        }
    }

    
}
