using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    //for game
    public GameObject pipe;
    public GameObject bird;
    public float pipesSpawnRate;
    private float timer;
    
    //for training stupid birds
    //-----------------------------------------------------------------

    private GameObject nearestPipe;
    GameObject population;
    public LinkedList<GameObject> pipes = new LinkedList<GameObject>();
    int maxFittestIndex;
    int secondFittestIndex;
    int leastFittestIndex;
    float maxScore;
    int generation = 0;
    Population myPopulation;
    double[] bestGenes;

    //private GameObject testBird;

    //-----------------------------------------------------------------

	void Start ()
    {
        timer = 0;
        population = new GameObject();
        myPopulation = population.AddComponent<Population>();

        myPopulation.InitializePopulation(Constant.POPULATION_SIZE, bird);
        pipes.AddLast(Instantiate(pipe, new Vector3(9, Random.Range(-3, 3), 0), Quaternion.identity));
        nearestPipe = pipes.First.Value;
        //testBird = Instantiate(bird, new Vector3(0, 0, 0), Quaternion.identity);
        bestGenes = new double[Constant.GENES_LENGTH];
    }
	
	void FixedUpdate ()
    {
        timer += 0.1f;
        if(timer >= pipesSpawnRate)
        {
            pipes.AddLast(Instantiate(pipe, new Vector3(9, Random.Range(-3, 3), 0) , Quaternion.identity));
            //remove unused pipes
            if(pipes.Count > 4)
            {
                Destroy(pipes.First.Value);
                pipes.RemoveFirst();
            }
            timer = 0;
        }

        //tracking pipes to find the next pipe to overcome!
        for(LinkedListNode<GameObject> pipe = pipes.First; true; pipe = pipe.Next)
        {
            nearestPipe = pipes.First.Value;
            if(pipe == null)
            {
                //nearestPipe = pipe.Value;
                continue;
            }

            
            if(pipe.Value.transform.position.x > 0)
            {
                nearestPipe = pipe.Value;
                break;
            }
            
        }

        if(nearestPipe != null)
        {
            //testBird.GetComponent<Bird>().Think(nearestPipe);
            for(int i = 0; i < Constant.POPULATION_SIZE; i++)
            {
                if(myPopulation.GetChildAtIndex(i).activeSelf)
                {
                    myPopulation.GetChildAtIndex(i).SendMessage("Think", value: nearestPipe);
                    
                }
                
            }
            //Debug.Log(nearestPipe.transform.position.x);
        }

        if (myPopulation.AllDead())
        //Input.GetButtonDown("Fire1"))
        {

            

            generation++;
            
            myPopulation.ReincarnatedPopulation();
            myPopulation.SortIndividual();

            
            
           
            Selection();
            Debug.Log("generation: " + generation + " maxScore: " + maxScore);
            /*
            myPopulation.ChangeGenesOfIndividual(Constant.POPULATION_SIZE - 2, bestGenes);
            myPopulation.GetChildAtIndex(Constant.POPULATION_SIZE - 2).SendMessage("PrintGenes");
            */

            Crossover();
            Mutate();

            if(maxScore < myPopulation.GetChildAtIndex(Constant.POPULATION_SIZE-1).GetComponent<Bird>().GetFitness())
            {
                maxScore = myPopulation.GetChildAtIndex(Constant.POPULATION_SIZE-1).GetComponent<Bird>().GetFitness();
                System.Array.Copy(myPopulation.GetChildAtIndex(Constant.POPULATION_SIZE - 1).GetComponent<Bird>().GetGenes(), bestGenes , Constant.GENES_LENGTH);
                Debug.Log("Change!");
                
            }



            //clear pipe
            while (pipes.Count != 0)
            {
                Destroy(pipes.Last.Value);
                pipes.RemoveLast();
            }

            pipes.AddLast(Instantiate(pipe, new Vector3(9, Random.Range(-3, 4), 0), Quaternion.identity));
            nearestPipe = pipes.Last.Value;
            timer = 0;

            

            myPopulation.ResetFitness();


        }

        if(Input.GetButtonDown("Fire1"))
        {

            Debug.Log(bestGenes[0] + " " + bestGenes[1] + " " + bestGenes[2] + " " + bestGenes[3] + " " + bestGenes[4]);

            
        }
        
	}


    //_____________for training stupid birds______________
    //----------------------------------------------------



    void Selection()
    {
        leastFittestIndex = myPopulation.GetLeastFittestIndex();
        maxFittestIndex = myPopulation.GetFittestIndex();
        secondFittestIndex = myPopulation.GetSecondFittestIndex();
    }

    void Crossover()
    {
        //get best parents genes
        double[] fittest = new double[Constant.GENES_LENGTH];
        System.Array.Copy(myPopulation.GetChildAtIndex(maxFittestIndex).GetComponent<Bird>().GetGenes(), fittest, Constant.GENES_LENGTH);
        double[] secondFittest = new double[Constant.GENES_LENGTH];
        System.Array.Copy(myPopulation.GetChildAtIndex(secondFittestIndex).GetComponent<Bird>().GetGenes() , secondFittest , Constant.GENES_LENGTH);

        double[] combinedGenes = new double[Constant.GENES_LENGTH];

        /*
        for(int k = 3 / 4 * Constant.POPULATION_SIZE; k < Constant.POPULATION_SIZE - 5; k++)
        {
            
            combinedGenes = fittest;
            int crossoverPoint = Random.Range(0, Constant.GENES_LENGTH);
            for(int l = 0; l < crossoverPoint; l++)
            {
                combinedGenes[l] = secondFittest[l];
            }
            
            population.GetComponent<Population>().GetChildAtIndex(k).GetComponent<Bird>().Mutate(combinedGenes);
        }
        */

        //Debug.Log(myPopulation.GetChildAtIndex(0).GetComponent<Bird>().GetGenes()[0]);
        for (//int i = 1/4 * Constant.POPULATION_SIZE; i < 3/4 * Constant.POPULATION_SIZE; i++)
            int i = 0; i < Constant.POPULATION_SIZE * 9/10; i++)
        {
            int chance = Random.Range(0, 10);
            if (chance > 8)
            {
                for (int j = 0; j < Constant.GENES_LENGTH; j++)
                {
                    combinedGenes[j] = (fittest[j] + secondFittest[j]) / 2;
                }
            }

            else
            {
                for (int j = 0; j < Constant.GENES_LENGTH; j++)
                {
                    if (Random.Range(0, 10) < 5)
                    {
                        combinedGenes[j] = fittest[j];
                    }
                    else
                    {
                        combinedGenes[j] = secondFittest[j];
                    }
                        
                }
            }


            myPopulation.ChangeGenesOfIndividual(i, combinedGenes);
        }
        
        //Debug.Log(myPopulation.GetChildAtIndex(0).GetComponent<Bird>().GetGenes()[0]);

        //population.GetComponent<Population>().GetChildAtIndex(secondFittestIndex).GetComponent<Bird>().Mutate(secondFittest);

    }

    void Mutate()
    {
        
        for(int i = 0; i < Constant.POPULATION_SIZE * 3/4; i++)
        {
            int mutationRate = Random.Range(0, 20);
            double[] mutatedGenes = new double[Constant.GENES_LENGTH];
            System.Array.Copy(myPopulation.GetChildAtIndex(i).GetComponent<Bird>().GetGenes(), mutatedGenes, Constant.GENES_LENGTH);


            if (mutationRate < 14)
            {
                //minnor mutatation
                for (int j = 0; j < Constant.GENES_LENGTH; j++)
                {
                    mutatedGenes[j] += Random.Range(-0.1f, 0.1f);
                }
                
            }

            else if (17 < mutationRate)
            {
                //partial mutation
                
                for (int j = 0; j < Constant.GENES_LENGTH; j++)
                {
                    if (Random.Range(0, 5) > 2)
                    {
                        continue;
                    }
                    mutatedGenes[j] += Random.Range(-0.5f, 0.5f);
                }
                

            }
            else
            {
                
                for(int j = 0; j < Constant.GENES_LENGTH; j++)
                {
                    mutatedGenes[j] = Random.Range(-1f, 1f);
                }
            }

            myPopulation.ChangeGenesOfIndividual(i, mutatedGenes);

        }
        
        
        
    }

    string PrintGenes(double[] genes)
    {
        string tmp = "";
        for(int i = 0; i < Constant.GENES_LENGTH; i++)
        {
            tmp += genes[i];
            tmp += " ";
        }

        return tmp;
    }

}

