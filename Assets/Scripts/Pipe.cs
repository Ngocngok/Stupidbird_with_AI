using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public float speed;
    private Rigidbody2D pipe;

	// Use this for initialization
	void Start ()
    {
        pipe = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        pipe.transform.Translate(transform.right * -1 * speed);
        
	}

    

}
