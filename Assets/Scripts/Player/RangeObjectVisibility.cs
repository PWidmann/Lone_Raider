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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Set objects active

        //if (collider.transform.GetComponent<IInteractible>() != null)
        //{
        //    collider.transform.GetComponent<IInteractible>().SetVisibility(true);
        //}
        //
        //if (collider.transform.GetComponent<IMonster>() != null)
        //{
        //    collider.transform.GetComponent<IMonster>().SetVisibility(true);
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Deactivate objects
        //if (collision.transform.GetComponent<IInteractible>() != null)
        //{
        //    collision.transform.GetComponent<IInteractible>().SetVisibility(false);
        //}
        //
        //if (collision.transform.GetComponent<IMonster>() != null)
        //{
        //    collision.transform.GetComponent<IMonster>().SetVisibility(false);
        //}
    }
}
