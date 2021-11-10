using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerFromParent : MonoBehaviour
{
    [SerializeField] int sortingLayerOffset;
    [SerializeField] GameObject parent;

    private new Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        renderer.sortingOrder = parent.GetComponent<Renderer>().sortingOrder + sortingLayerOffset;
    }
}
