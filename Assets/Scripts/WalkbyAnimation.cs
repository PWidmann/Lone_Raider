using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkbyAnimation : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetTrigger("PlaybackTrigger");

            Debug.Log("Player trigger");
        }
    }

    
}
