using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{



    public Rigidbody2D bird;
    private bool dead = false;
    public float jumpForce;



    // Update is called once per frame
    void FixedUpdate()
    {
        if (!dead)
        {
            /*
            if (Input.GetButtonDown("Fire1"))
            {
                bird.velocity = new Vector2(0, jumpForce);
                Debug.Log("Jump!");
            }
            */

            //calculate fitness


        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Bird")
        {
            dead = true;
            this.gameObject.SetActive(false);
        }

    }


    //___________________for training_______________________
    //------------------------------------------------------


    private double[] genes;
    private float fitness;


    void Awake()
    {
        genes = new double[Constant.GENES_LENGTH];
        for (int i = 0; i < Constant.GENES_LENGTH; i++)
        {
            genes[i] = UnityEngine.Random.Range(-1f, 1f);
        }
    }


    public void Think(GameObject nearestPipe)
    {
        if (!dead)
        {
            fitness++;
        }

        float input0 = transform.position.y;
        float input1 = nearestPipe.transform.position.y - 1.5f;
        //float input2 = input1 + 3f;
        float input3 = nearestPipe.transform.position.x;
        float input4 = bird.velocity.y;



        float decision = (float)System.Math.Tanh(genes[0] * input0 + genes[1] * input1 + genes[2] * input3 + genes[3] * input4);



        if (decision > 0.5f)
        {
            bird.AddForce(bird.velocity = new Vector2(0, jumpForce));
        }

        //Debug.Log("Thinking!");
    }

    public float GetFitness()
    {
        return fitness;
    }



    public double[] GetGenes()
    {
        return genes;
    }

    public void Mutate(double[] newGenes)
    {
        System.Array.Copy(newGenes, genes, Constant.GENES_LENGTH);
    }

    public void Revive()
    {
        dead = false;

    }

    public void ResetFitness()
    {
        fitness = 0;
    }

    

}
