using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1.3f;

    CharacterController controller;
    Animator animator;

    // Input
    Vector2 input;
    float horizontal;
    float vertical;
    Vector2 movement;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

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
            controller.Move(movement);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
}
