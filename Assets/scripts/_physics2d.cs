using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _physics2d : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
 
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
            col.transform.position = new Vector2(col.transform.position.x + 0.5f, col.transform.position.y);
        }
        else
        {
            col.transform.position = new Vector2(col.transform.position.x - 0.5f, col.transform.position.y);
        }
    }
    /*
 void OnTriggerStay2D(Collider2D col)
 {
     if(col.transform.tag == "Player")
     {
         //heading = new Vector2(transform.position.x, transform.position.y) - new Vector2(col.transform.position.x, col.transform.position.y);
         //direction = heading / heading.magnitude;
         //Debug.Log(col.transform.name);
         //Debug.Log(direction);
         //col.transform.position =  new Vector3(direction.x * Time.deltaTime, direction.y * Time.deltaTime, col.transform.position.z);
         //col.transform.Translate(direction.x * 0.5f, direction.y * 0.5f, col.transform.position.z);
         Debug.Log("OnTrigger");
         var rel = new Vector2(transform.position.x, transform.position.y) - col.GetComponent<Rigidbody2D>().position;
         if (rel.y > 0.5f) // if we are over the other
             col.GetComponent<Rigidbody2D>().AddForce(rel * 20f, ForceMode2D.Impulse); // push us away from the other player
     }
 }
     */
}
