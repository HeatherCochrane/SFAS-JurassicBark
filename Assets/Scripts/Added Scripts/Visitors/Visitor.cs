using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visitor : Character
{

	VisitorHandler handler;
	//Environment object
	Environment mMap;


	//Map
	EnvironmentTile[][] storedMap;

	int randTime = 0;

	EnvironmentTile randTile;

	List<EnvironmentTile> paths = new List<EnvironmentTile>();

	bool parkClosed = false;

	// Start is called before the first frame update
	void Start()
    {
		mMap = GameObject.Find("Environment").GetComponent<Environment>();
		handler = GameObject.Find("VisitorHandler").GetComponent<VisitorHandler>();

		this.setCharacterType(2);

		//Call the move function after 5 seconds after spawning
		Invoke("moveVisitor", 5);

		//Get the paths avaliable
		paths = handler.getPaths();
	}

    // Update is called once per frame
    void Update()
    {
		//Destroy the object once it has reached the starting tile with the park closed
		if(parkClosed && this.CurrentPosition == mMap.Start)
		{
			Destroy(this.gameObject);
		}
        
    }

	EnvironmentTile pickRandomTile()
	{

		//Assing the variable tile to a randomly selected tile within the avaliable tiles
		if (paths.Count > 0)
		{
			randTile = paths[Random.Range(0, paths.Count)];
		}

		return randTile;

	}

	void moveVisitor()
	{
		if (!parkClosed)
		{
			//Solve the path from where the visitor is currently standing, to the desired random tile
			List<EnvironmentTile> visRoute = mMap.Solve(this.CurrentPosition, pickRandomTile(), this.getCharacterType());
		this.GoTo(visRoute);

		//Once the character has moved, call the function after a certain amount of time for continuos movement

		randTime = Random.Range(5, 8);

		
		Invoke("moveVisitor", randTime);
		}

	}


	public void goToExit()
	{
		//Set to 0 to avoid visitors getting stuck within the park if a path is destroyed
		List<EnvironmentTile> visRoute = mMap.Solve(this.CurrentPosition, mMap.Start, 1);

		//If a path cannot be found, just destroy the visitor
		if(visRoute == null)
		{
			Destroy(this.gameObject);
		}

		this.GoTo(visRoute);
	}

	public void setPark(bool set)
	{
		parkClosed = set;
	}



}
