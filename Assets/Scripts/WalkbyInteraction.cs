using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkbyInteraction : MonoBehaviour, IInteractible
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        gameObject.transform.GetComponent<Renderer>().enabled = false;
    }

    public void Interact()
    {
        if(animator != null)
            animator.SetTrigger("PlaybackTrigger");
    }

    public void SetVisible()
    {
        gameObject.transform.GetComponent<Renderer>().enabled = true;
    }
}
