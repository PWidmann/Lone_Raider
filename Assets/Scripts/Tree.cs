using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IInteractible
{
    void Start()
    {
        gameObject.transform.GetComponent<Renderer>().enabled = false;
    }

    void Update()
    {
        
    }

    public void Interact()
    {
        
    }

    public void SetVisibility(bool isVisible)
    {
        if (isVisible)
        {
            gameObject.transform.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            gameObject.transform.GetComponent<Renderer>().enabled = false;
        }
        
    }
}
