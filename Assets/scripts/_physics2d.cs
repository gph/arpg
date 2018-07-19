using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _physics2d : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.position.y < transform.position.y)
        {
            col.transform.position = new Vector2(col.transform.position.x, col.transform.position.y - 0.1f);
        }
        else if (col.transform.position.y > transform.position.y)
        {
            col.transform.position = new Vector2(col.transform.position.x, col.transform.position.y + 0.1f);
        }

        if (col.transform.position.x > transform.position.x)
        {
            col.transform.position = new Vector2(col.transform.position.x + 0.1f, col.transform.position.y);
        }
        else if (col.transform.position.x < transform.position.x)
        {
            col.transform.position = new Vector2(col.transform.position.x - 0.1f, col.transform.position.y);
        }
    }
}
