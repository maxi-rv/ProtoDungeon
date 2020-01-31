using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAll : MonoBehaviour
{
    private BoxCollider2D hitBox;

    // Start is called before the first frame update
    void Awake()
    {
        hitBox = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        hitBox.enabled = false;
        
        bool boton = Input.GetButtonDown("DamageAll");

        if (boton)
        {
            hitBox.enabled = true;
        }
    }
}
