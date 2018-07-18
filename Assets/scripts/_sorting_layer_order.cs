using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _sorting_layer_order : MonoBehaviour {
    // Use this for initialization
    void Start()
    {
        SortingLayerOrder();
    }

    // Update is called once per frame
    void Update()
    {
        SortingLayerOrder();
    }

    void SortingLayerOrder()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }
}
