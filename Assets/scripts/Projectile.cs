using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Projectile COLLISION: " + transform.name + " x " + collision.transform.name);
        var hit = collision.gameObject;
        var health = hit.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(10);
        }
        Destroy(gameObject);
    }
}
