using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1.3f;
    [SerializeField] Light2D lightPlayer;

    Animator animator;
    Rigidbody2D rb2D;

    // Input
    Vector2 input;
    float horizontal;
    float vertical;
    Vector2 movement;
    
    bool canMove = true;
    bool canAttack = true;

    bool nightOn = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        CameraController.Instance.SetCameraTarget(gameObject);
        animator.SetFloat("movementY", -1f);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveData data = SaveSystem.LoadWorld(GameManager.CurrentWorldName);

            SaveSystem.SaveWorld(GameManager.CurrentWorldName, data.biomeMapArray, GameManager.CurrentSeed, (int)transform.position.x, (int)transform.position.y);
            Debug.Log("Quicksave current world '" + data.gameName + "'");
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

        // Night mode
        if (Input.GetKeyDown(KeyCode.N))
        {
            nightOn = !nightOn;

            if (nightOn)
            {
                lightPlayer.intensity = 0.9f;
                GameInterface.Instance.ambientLight.intensity = 0;
            }
            else
            {
                lightPlayer.intensity = 0.0f;
                GameInterface.Instance.ambientLight.intensity = 0.9f;
            }
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
