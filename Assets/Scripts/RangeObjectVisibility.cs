using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeObjectVisibility : MonoBehaviour
{
    [SerializeField] int visibilityRadius = 10;
    CircleCollider2D circleCollider2D;

    private void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.radius = visibilityRadius;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.transform.GetComponent<IInteractible>() != null)
        {
            collider.transform.GetComponent<Animator>().enabled = true;
            collider.transform.GetComponent<Renderer>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<IInteractible>() != null)
        {
            collision.transform.GetComponent<Animator>().enabled = false;
            collision.transform.GetComponent<Renderer>().enabled = false;
            
        }
    }
}
