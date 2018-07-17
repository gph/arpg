using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("PROJECTILE>COLLISION_NAME>PROJECTILE_ROOT_OBJ: " + transform.name + ">" + collision.transform.name + ">" + transform.parent.name);
        var hit = collision.gameObject;

        //collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        //collision.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;

        var health = hit.GetComponent<Health>();
        if (health != null && collision.transform.name != transform.parent.name)
        {
            health.TakeDamage(10);
        }
        Destroy(gameObject);
    }
}
