using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1.3f;
    [SerializeField] float maxCameraZoom = 40f;

    Animator animator;
    Rigidbody2D rb2D;

    // Input
    Vector2 input;
    float horizontal;
    float vertical;
    Vector2 movement;
    
    bool canMove = true;
    bool canAttack = true;

    new Camera camera;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        CameraController.Instance.SetCameraTarget(gameObject);
        animator.SetFloat("movementY", -1f);

        camera = Camera.main;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveData data = SaveSystem.LoadWorld(GameManager.CurrentWorldName);

            SaveSystem.SaveWorld(GameManager.CurrentWorldName, data.biomeMapArray, GameManager.CurrentSeed, transform.position.x - 0.5f, transform.position.y - 0.5f);
            Debug.Log("Quicksave current world '" + data.gameName + "'");

            GameObject.Find("GameInterface").GetComponent<GameInterface>().ShowQuicksavePanel();
        }

        // Camera zoom
        if (Input.mouseScrollDelta.y != 0)
        {
            camera.orthographicSize -= Input.mouseScrollDelta.y;

            if (camera.orthographicSize < 2f)
                camera.orthographicSize = 2f;
            if (camera.orthographicSize > maxCameraZoom)
                camera.orthographicSize = maxCameraZoom;
        }


        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        input = new Vector2(horizontal, vertical).normalized;
        
        if (input != Vector2.zero && canMove)
        {
            animator.SetFloat("movementX", horizontal);
            animator.SetFloat("movementY", vertical);
            animator.SetBool("isMoving", true);

            movement = input * moveSpeed * Time.deltaTime;
            rb2D.transform.Translate(movement);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        // Sword attack
        if (Input.GetMouseButtonDown(0))
        {
            if (canAttack)
            {
                animator.SetTrigger("attack");
                canAttack = false;
                canMove = false;
            }  
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 5f;
        }
        else
        {
            moveSpeed = 1.3f;
        }
    }

    public void FreeActions()
    {
        canAttack = true;
        canMove = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.GetComponent<IInteractible>() != null)
        {
            collider.gameObject.GetComponent<IInteractible>().Interact();
        }
    }
}
