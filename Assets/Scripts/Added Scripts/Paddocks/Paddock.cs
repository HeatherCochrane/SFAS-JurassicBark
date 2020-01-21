using System.Collections;
using System.Collections.Generic;
using UnityEngine;



struct PaddockStats
{
	public int numOfDogs;
	public int foodLevels;
	public int happinessLevels;
}

public class Paddock : MonoBehaviour
{
	PaddockHandler handler;
	PaddockStats stats;
	Park park;

	int hungerDrainRate = 0;

	public List<GameObject> dogsInPaddock = new List<GameObject>();

	List<EnvironmentTile> paddockTiles = new List<EnvironmentTile>();

	//Reduce Food 
	IEnumerator paddockFoodLevels()
	{
		while(true)
		{
			stats.foodLevels -= hungerDrainRate;

			//If the food levels drop below a certain number, reduce the dogs happiness
			if(stats.foodLevels <= 50)
			{
				for (int i = 0; i < dogsInPaddock.Count; i++)
				{
					dogsInPaddock[i].GetComponentInChildren<DogBehaviour>().changehappiness(-2);
				}
			}
			else
			{
				//Otherwise 
				for (int i = 0; i < dogsInPaddock.Count; i++)
				{
					dogsInPaddock[i].GetComponentInChildren<DogBehaviour>().changehappiness(2);
				}
			}
			
			if(stats.foodLevels < 1)
			{
				stats.foodLevels = 0;
			}

			updateHappiness();


			yield return new WaitForSeconds(20);
		}
	}


	int averageHappiness = 0;

	int numOfDogsAvaliable = 0;
	int dogCount = 0;

	List<Color32> grassColours = new List<Color32>();

	// Start is called before the first frame update
	void Start()
    {
		handler = GameObject.Find("PaddockHandler").GetComponent<PaddockHandler>();

		grassColours.Add(new Color32(98, 214, 164, 1));
		grassColours.Add(new Color32(122, 221, 159, 1));
		grassColours.Add(new Color32(105, 229, 140, 1));
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	void paddockSpawned()
	{
		park = GameObject.Find("ParkHandler").GetComponent<Park>();
		park.addPaddock(this.gameObject);


		StartCoroutine(paddockFoodLevels());
		stats.happinessLevels = 50;
		stats.foodLevels = 50;

		park.generateHappinessAverage();
	}

	public bool addDogs()
	{
		if(dogCount != numOfDogsAvaliable)
		{
			dogCount += 1;
			hungerDrainRate += 1;
			updateHappiness();

			return true;
		}
		else
		{
			return false;
		}
		
	}

	public void updateHappiness()
	{
		if (dogsInPaddock.Count > 0)
		{
			averageHappiness = 0;

			for (int i= 0; i < dogsInPaddock.Count; i++)
			{
				averageHappiness += dogsInPaddock[i].GetComponentInChildren<DogBehaviour>().getHappiness();
			}

			averageHappiness = averageHappiness / dogsInPaddock.Count;

		}

		stats.happinessLevels = averageHappiness;


		park.generateHappinessAverage(); 
		
	}

	public void updateDogList(GameObject newDog)
	{
		dogsInPaddock.Add(newDog);
	}

	public int getDogs()
	{
		return dogsInPaddock.Count;
	}
	public void addFood(int food)
	{
		stats.foodLevels += food;

		//Only increase dog happiness if the food goes over the minimum amount
		if (stats.foodLevels > 50)
		{
			for (int i = 0; i < dogsInPaddock.Count; i++)
			{
				dogsInPaddock[i].GetComponentInChildren<DogBehaviour>().changehappiness(1);
			}
		}
	}

	public void takeFood(int food)
	{
		stats.foodLevels -= food;
	}

	public int getHunger()
	{
		return stats.foodLevels;
	}

	public int getHappiness()
	{
		return stats.happinessLevels;
	}

	public void paddockSetUp(int size)
	{
		numOfDogsAvaliable = size;
		paddockSpawned();
	}

	public Paddock getPaddock()
	{
		return this;
	}

	public List<EnvironmentTile>getPaddockTiles()
	{
		foreach(Transform child in this.transform)
		{
			if(child.GetComponent<EnvironmentTile>())
			{
				EnvironmentTile tile = child.GetComponent<EnvironmentTile>();
				paddockTiles.Add(tile);
			}
		}

		return paddockTiles;
	}

	public void emptyPaddock()
	{
		for(int i =0; i < dogsInPaddock.Count; i++)
		{
			Destroy(dogsInPaddock[i]);
		}

		dogsInPaddock.Clear();

		for(int i=0; i < paddockTiles.Count; i++)
		{
			paddockTiles[i].GetComponent<EnvironmentTile>().IsAccessible = true;
			paddockTiles[i].GetComponent<EnvironmentTile>().isPaddock = false;
			paddockTiles[i].GetComponent<Renderer>().materials[1].color = grassColours[Random.Range(0, grassColours.Count)];
		}

		paddockTiles.Clear();
		handler.deletePaddock(this.gameObject);
		park.deletePaddock(this.gameObject);
	}
}
