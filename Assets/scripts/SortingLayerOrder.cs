using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerOrder : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SortingSprites();
    }
	
	// Update is called once per frame
	void Update () {
        SortingSprites();
    }

    void SortingSprites()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }
}
