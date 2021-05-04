using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IInteractible
{
    

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        
    }

    public void SetVisible()
    {
        gameObject.transform.GetComponent<Renderer>().enabled = true;
    }
}
