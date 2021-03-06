using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRendererSorter : MonoBehaviour
{
    [SerializeField] private int sortingOrderBase = 5000;
    [SerializeField] private float offset = 0;
    [SerializeField] bool runOnlyOnce = true;

    private float timer;
    private float timerMax = .1f;
    private Renderer myRenderer;

    private void Awake()
    {
        myRenderer = GetComponent<Renderer>();
    }
    private void LateUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = timerMax;
            myRenderer.sortingOrder = (int)(sortingOrderBase - (transform.position.y - offset) * 100);
            if (runOnlyOnce)
            {
                Destroy(this);
            }
        }
    }
}
