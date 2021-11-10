using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitDetection : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.GetComponent<IMonster>() != null)
        {
            collider.gameObject.GetComponent<IMonster>().TakeDamage();
        }
    }
}
