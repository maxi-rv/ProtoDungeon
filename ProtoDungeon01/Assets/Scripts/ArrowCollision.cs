using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCollision : MonoBehaviour
{

    //Sent when ANOTHER object trigger collider enters a trigger collider attached to this object.
    void OnTriggerEnter2D(Collider2D other)
    {
        //Compares the hitbox tag with its own tag.
        if(other.gameObject.CompareTag("Enemy"))
        {
            DestroyArrow();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //Compares the hitbox tag with its own tag.
       if(other.gameObject.CompareTag("Obstacle"))
        {
            Invoke("DestroyArrow", 30f);
        }
    }

    void DestroyArrow()
    {
        Destroy(gameObject);
    }
}
