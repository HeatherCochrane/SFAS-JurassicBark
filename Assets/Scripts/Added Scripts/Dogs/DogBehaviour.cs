using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Dogs stats
struct Stats
{
	public string name;
	public int happiness;
}


public class DogBehaviour : Character
{
	Stats stats;
	List<string> names = new List<string>();

	//Random int for picking the dogs name
	int rand = 0;

	//The dogs paddock it has been assigned to
	Paddock paddock;
	PaddockProfiles Pprofiles;

	//Sets the max values for both happiness and hunger
	int max = 100;
	int min = 0;


	//Movement
	DogHandler handler;

	//Environment object
	Environment mMap;


	//Map
	EnvironmentTile[][] storedMap;

	int randTime = 0;

	EnvironmentTile randTile;

	//Tiles avaliable for the specified dog
	public List<EnvironmentTile> paddockTiles = new List<EnvironmentTile>();

	//Tile the dog was placed on
	EnvironmentTile placedTile;

	bool invokeStarted = false;

	// Start is called before the first frame update
	void Start()
    {
		mMap = GameObject.Find("Environment").GetComponent<Environment>();
		handler = GameObject.Find("DogHandler").GetComponent<DogHandler>();

		//Random names
		names.Add("Rupert");
		names.Add("Spot");
		names.Add("George");
		names.Add("Skye");
		names.Add("Woody");
		names.Add("Willow");
		names.Add("Boomer");
		names.Add("Flynn");
		names.Add("Bobbi");

		//Dog must be set up before moving
		setUpDog();

		//Move the dog as soon as it has been placed
		pickRandomTile();
		moveDog();
	}

    // Update is called once per frame
    void Update()
    {
		//Allow the dogs to chase the hooligan without stopping and returning to their paddock
		if(this.returnGoToState() == true && invokeStarted == false)
		{
			moveDog();
			invokeStarted = true;
		}
        
    }

	void setUpDog()
	{
		//Tell paddock another dog has been added
		paddock.updateDogList(this.gameObject);

		rand = Random.Range(0, names.Count);

		//Will randomly pick name
		stats.name = names[rand];

		//Starting Happiness
		stats.happiness = 60;

		this.gameObject.name = stats.name;

		//Set this 'character' to be of type dog
		this.setCharacterType(3);

		//Set the paddock as its parent object
		this.gameObject.transform.parent.transform.parent = paddock.gameObject.transform;

		paddock.updateHappiness();
	}

	void checkForDogs()
	{
		//Clamp the dogs happiness based on how many friends are in the paddock
		if(paddock.getDogs() == 1)
		{
			stats.happiness = Mathf.Clamp(stats.happiness, 0, 70);
		}
		else if(paddock.getDogs() == 2)
		{
			stats.happiness = Mathf.Clamp(stats.happiness, 0, 80);
		}
		else if(paddock.getDogs() ==3)
		{
			stats.happiness = Mathf.Clamp(stats.happiness, 0, 100);
		}
	}

	public void setTile(EnvironmentTile tile)
	{
		placedTile = tile;

		this.transform.position = placedTile.transform.position;
		this.transform.rotation = Quaternion.identity;
		this.CurrentPosition = placedTile;
	}

	public void changehappiness(int Hchange)
	{
		stats.happiness += Hchange;
		checkForDogs();

		Pprofiles.updateValues();
	}

	public void setPaddock(Paddock setPaddock)
	{
		paddock = setPaddock;
		Pprofiles = paddock.GetComponentInChildren<PaddockProfiles>();
	}

	public string getName()
	{
		return stats.name;
	}

	public int getHappiness()
	{
		return stats.happiness;
	}

	//Dog Movement
	EnvironmentTile pickRandomTile()
	{
		//Assing the variable tile to a randomly selected tile within the avaliable tiles
		if (paddockTiles.Count > 0)
		{
			randTile = paddockTiles[Random.Range(0, paddockTiles.Count)];
		}

		return randTile;

	}

	void moveDog()
	{
		//Solve the path from where the visitor is currently standing, to the desired random tile
		List<EnvironmentTile> visRoute = mMap.Solve(this.CurrentPosition, pickRandomTile(), this.getCharacterType());
		this.GoTo(visRoute);


		//Once the character has moved, call the function after a certain amount of time for continuos movement
		randTime = Random.Range(8, 12);

		//Constantly move the dog around the paddock
		Invoke("moveDog", randTime);

	}


	public void setPaddockTiles(List<EnvironmentTile> tiles)
	{
		paddockTiles = tiles;
	}
	public List<EnvironmentTile> getPaddockTiles()
	{
		return paddockTiles;
	}

	public void releaseTheHound(EnvironmentTile tilePassedIn)
	{
		CancelInvoke();
		List<EnvironmentTile> visRoute = mMap.Solve(this.CurrentPosition, tilePassedIn, 0);
		this.GoTo(visRoute);
	}

	public void returnToPaddock()
	{
		List<EnvironmentTile> visRoute = mMap.Solve(this.CurrentPosition, pickRandomTile(), 0);
		this.GoTo(visRoute);

		invokeStarted = false;
	}

}
