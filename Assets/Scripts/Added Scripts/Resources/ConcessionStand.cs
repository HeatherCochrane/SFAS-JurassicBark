using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcessionStand : MonoBehaviour
{
	IEnumerator produceIncome()
	{
		while (true)
		{
			if(pathConnecting && !hours.getOpeningHour())
			{
				currency.addIncome(income);
				totalIncome += income;

				incomeSprite.GetComponent<SpriteRenderer>().sprite = producing;
			}
			else
			{
				incomeSprite.GetComponent<SpriteRenderer>().sprite = notProducing;
			}

			yield return new WaitForSeconds(10f);
		}
	}


	Camera cam;

	PlayerCurrency currency;
	int income = 3;
	int totalIncome = 0;

	bool pathConnecting = false;

	VisitorHandler hours;
	Park parkStats;

	EnvironmentTile tilePlaced;

	RaycastHit[] hit;

	List<GameObject> collidedObjects = new List<GameObject>();

	int tileSize = 5;

	GameObject incomeSprite;
	public Sprite producing;
	public Sprite notProducing;

	bool shopPlaced = false;
	bool incomeProducing = false;

    // Start is called before the first frame update
    void Start()
    {
    }

	public void startProduction()
	{
		currency = GameObject.Find("Currency").GetComponent<PlayerCurrency>();
		hours = GameObject.Find("VisitorHandler").GetComponent<VisitorHandler>();
		parkStats = GameObject.Find("ParkHandler").GetComponent<Park>();

		incomeSprite = this.transform.GetChild(0).gameObject;

		cam = GameObject.Find("MainCamera").GetComponent<Camera>();
		shopPlaced = true;

	}

    // Update is called once per frame
    void Update()
    {
		if (shopPlaced)
		{
			incomeSprite.transform.LookAt(cam.transform.position, Vector3.up);
		}
	}

	public int getIncome()
	{
		return totalIncome;
	}

	public void tilePlacedOn(EnvironmentTile tile)
	{
		tilePlaced = tile;
		checkTiles();

		StartCoroutine(produceIncome());
	}

	public void checkTiles()
	{
		//Check the surrounding tiles around the tile placed on, call this check when placed and when a path is placed

		if (!hours.getOpeningHour())
		{
			//Check right
			hit = Physics.RaycastAll(tilePlaced.Position, tilePlaced.transform.right, tileSize);
			if (cycleThrough(hit))
			{
				return;
			}

			//Check left
			hit = Physics.RaycastAll(tilePlaced.Position, -tilePlaced.transform.right, tileSize);
			if (cycleThrough(hit))
			{
				return;
			}

			//Down
			hit = Physics.RaycastAll(tilePlaced.Position, Vector3.back, tileSize);
			if (cycleThrough(hit))
			{
				return;
			}
			//Up
			hit = Physics.RaycastAll(tilePlaced.Position, -Vector3.back, tileSize);
			if (cycleThrough(hit))
			{
				return;
			}
		}
	}

	bool cycleThrough(RaycastHit[] hit)
	{
		//Check to see if the immediate tiles are in-fact paths
		for (int i = 0; i < hit.Length; i++)
		{
			RaycastHit hits = hit[i];

			EnvironmentTile tl = hits.transform.GetComponent<EnvironmentTile>();

			if (tl.isPath)
			{
				//If it is a path, the shop can be accessed by customers
				pathConnecting = true;
				incomeSprite.GetComponent<SpriteRenderer>().sprite = producing;
				return true;
			}
			else
			{
				pathConnecting = false;
				incomeSprite.GetComponent<SpriteRenderer>().sprite = notProducing;
				return false;
			}
		}
		return false;
	}
}
