using System.Collections;
using System.Collections.Generic;
using UnityEngine;


struct Stats
{
	public string name;
	public int happiness;
}


public class DogBehaviour : Character
{

	Stats stats;
	List<string> names = new List<string>();

	int rand = 0;

	Paddock paddock;
	PaddockProfiles Pprofiles;

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

	public List<EnvironmentTile> paddockTiles = new List<EnvironmentTile>();

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

		setUpDog();

		pickRandomTile();
		moveDog();
	}

    // Update is called once per frame
    void Update()
    {
		if(this.returnGoToState() == true && invokeStarted == false)
		{
			moveDog();
			invokeStarted = true;
		}
        
    }

	void setUpDog()
	{
		rand = Random.Range(0, names.Count);

		//Will randomly pick name
		stats.name = names[rand];

		stats.happiness = 60;

		this.gameObject.name = stats.name;

		this.setCharacterType(3);

		paddock.updateHappiness();
	}

	void checkForDogs()
	{
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
