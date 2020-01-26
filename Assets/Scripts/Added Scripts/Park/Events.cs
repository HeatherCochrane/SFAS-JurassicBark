﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
	//Timer for when the hooligan is active
	IEnumerator eventTimer()
	{
		while (eventActive)
		{
			timer++;

			//If time runs out and the hooligan is still around
			if (hooliganActive)
			{
				if (timer > 60)
				{
					hooliganCaught(false);
					timer = 0;
					eventActive = false;
				}
			}
		

			yield return new WaitForSeconds(1);
		}
	}

	//Timer countdown for when the hooligan should spawn
	IEnumerator eventCountDown()
	{
		while(countDown != 0)
		{
			countDown--;

			if(countDown == 0)
			{
				hooliganActive = true;
				startEvent();
			}
			yield return new WaitForSeconds(1);
		}
	}

	bool eventActive = false;
	int timer = 0;

	public GameObject hooliganObject;
	GameObject hooligan;

	EnvironmentTile chosenTile;

	EnvironmentTile[][] tiles;
	Environment map;

	PlayerCurrency currency;
	DogHandler dogs;

	HUD hud;


	int ranX = 0;
	int ranY = 0;

	int countDown = 0;

	List<GameObject> dogsInPark = new List<GameObject>();

	bool hooliganActive = false;
	int hooligansSpawned = 0;

	Challenges challenge;
	Game game;
	LevelExp level;


    void Start()
    {
		map = GameObject.Find("Environment").GetComponent<Environment>();
		currency = GameObject.Find("Currency").GetComponent<PlayerCurrency>();
		dogs = GameObject.Find("DogHandler").GetComponent<DogHandler>();
		hud = GameObject.Find("HUD").GetComponent<HUD>();
		challenge = GameObject.Find("Challenges").GetComponent<Challenges>();
		game = GameObject.Find("Game").GetComponent<Game>();
		level = GameObject.Find("Levelling").GetComponent<LevelExp>();

		hud.showHoundsButton(false);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	void spawnHooligan()
	{
		setTile();
		hooligan = Instantiate(hooliganObject);
		hooligan.transform.position = new Vector3(chosenTile.transform.position.x + 5, chosenTile.transform.position.y + 3, chosenTile.transform.position.z + 5);
		hud.showHoundsButton(true);
	}

	public void setTile()
	{
		tiles = map.getMap();
		pickTile();
		chosenTile = tiles[ranX][ranY];

		//If the tile isn't accessible choose another
		while (!chosenTile.IsAccessible)
		{
			pickTile();
			chosenTile = tiles[ranX][ranY];
		}	
	}

	void pickTile()
	{
		ranX = Random.Range(0, map.getMapSize().x);
		ranY = Random.Range(0, map.getMapSize().y);
	}

	public void startEvent()
	{
		spawnHooligan();
		eventActive = true;
		timer = 0;
		hooliganActive = true;
		game.setEventActive(true);
		StartCoroutine(eventTimer());
	}

	public void releaseTheDogs()
	{
		dogsInPark = dogs.getDogList();

		if (dogsInPark.Count > 0 && dogs != null && hooliganActive)
		{
			for (int i = 0; i < dogsInPark.Count; i++)
			{
				dogsInPark[i].GetComponentInChildren<DogBehaviour>().releaseTheHound(chosenTile);
			}
		}
	}
	public void startCountdown(int num)
	{
		countDown = num;
		StartCoroutine(eventCountDown());
	}


	public void hooliganCaught(bool isCaught)
	{
		if (isCaught)
		{
			challenge.hooligansCaught(1);
			currency.addIncome(10);
			level.addExp(50);
		}
		else
		{
			currency.takeIncome(50);
		}

		hooligansSpawned++;
		Destroy(hooligan.gameObject);
		returnDogs();
		hooliganActive = false;
		hud.showHoundsButton(false);
		eventActive = false;
		game.setEventActive(false);

		timer = 0;

		if(hooligansSpawned < 2)
		{
			startCountdown(Random.Range(20, 30));
		}
		else
		{
			hooligansSpawned = 0;
		}

	}

	void returnDogs()
	{
		//Send the dogs back to their paddocks
		if (dogsInPark.Count > 0 && dogs != null)
		{
			for (int i = 0; i < dogsInPark.Count; i++)
			{
				dogsInPark[i].GetComponentInChildren<DogBehaviour>().returnToPaddock();
			}
		}

	}

	public EnvironmentTile hooliganTile()
	{
		return chosenTile;
	}
}
