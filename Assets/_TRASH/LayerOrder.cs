
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerOrder : MonoBehaviour
{

    private int sortingOrder = 0;
    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sortingOrder = (int)transform.position.y + 1;
    }

    void Update()
    {
        if (sprite)
            sprite.sortingOrder = sortingOrder;
    }
}

