using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population : MonoBehaviour
{
    GameObject[] birds;

    //Initialize population
    public void InitializePopulation(int size, GameObject species)
    {
        birds = new GameObject[Constant.POPULATION_SIZE];

        for (int i = 0; i < size; i++)
        {
            birds[i] = Instantiate(species, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

    /*
    public int GetFittestIndex()
    {
        float maxFit = 0;
        int maxFitIndex = 0;

        for (int i = 0; i < birds.Length; i++)
        {
            if (maxFit <= birds[i].GetComponent<Bird>().GetFitness())
            {
                maxFit = birds[i].GetComponent<Bird>().GetFitness();
                maxFitIndex = i;
            }
        }

        return maxFitIndex;
    }

    public int GetSecondFittestIndex()
    {
        int maxFit = 0;
        int maxFit2 = 0;

        for (int i = 0; i < birds.Length; i++)
        {
            if (birds[i].GetComponent<Bird>().GetFitness() > birds[maxFit].GetComponent<Bird>().GetFitness())
            {
                maxFit2 = maxFit;
                maxFit = i;
            }
            else if (birds[i].GetComponent<Bird>().GetFitness() > birds[maxFit2].GetComponent<Bird>().GetFitness())
            {
                maxFit2 = i;
            }
        }

        if (maxFit == maxFit2) maxFit2++;
        return maxFit2;
    }

    public int GetLeastFittestIndex()
    {
        float minFitVal = birds[0].GetComponent<Bird>().GetFitness();
        int minFitIndex = 0;
        for (int i = 0; i < birds.Length; i++)
        {
            if (minFitVal >= birds[i].GetComponent<Bird>().GetFitness())
            {
                minFitVal = birds[i].GetComponent<Bird>().GetFitness();
                minFitIndex = i;
            }
        }

        return minFitIndex;
    }
    */

    public int GetFittestIndex()
    {
        return Constant.POPULATION_SIZE - 1;
    }

    public int GetSecondFittestIndex()
    {
        //return Random.Range(Constant.POPULATION_SIZE - 5, Constant.POPULATION_SIZE - 1);
        return Constant.POPULATION_SIZE - 2;
    }

    public int GetLeastFittestIndex()
    {
        return 0;
    }


    public GameObject GetChildAtIndex(int index)
    {
        return birds[index];
    }

    public bool AllDead()
    {
        bool check = true;
        for(int i = 0; i < Constant.POPULATION_SIZE; i++)
        {
            if (birds[i].activeSelf == true)
                check = false;
        }

        return check;
    }

    public void ReincarnatedPopulation()
    {
        for (int i = 0; i < Constant.POPULATION_SIZE; i++)
        {
            birds[i].transform.position = new Vector3(0, 0, 0);
            birds[i].SetActive(true);
            birds[i].SendMessage("Revive");
            
        }
    }

    public void ResetFitness()
    {
        for (int i = 0; i < Constant.POPULATION_SIZE; i++)
        {
            birds[i].SendMessage("ResetFitness");
        }
    }

    public void SortIndividual()
    {
        
        System.Array.Sort(birds, new IndividualComparer());
    }

    public void PrintIndividualsFitness()
    {
        string tmp = "";
        for(int i = 0; i < Constant.POPULATION_SIZE; i++)
        {
            tmp += birds[i].GetComponent<Bird>().GetFitness();
            tmp += " ";
        }
        Debug.Log(tmp);
    }

    public void ChangeGenesOfIndividual(int index, double[] newGenes)
    {
        birds[index].SendMessage("Mutate", value : newGenes);
    }
}

public class IndividualComparer : IComparer<GameObject>
{
    public int Compare(GameObject x, GameObject y)
    {
        return x.GetComponent<Bird>().GetFitness().CompareTo(y.GetComponent<Bird>().GetFitness());
    }
}



