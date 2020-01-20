using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;


public class Game : MonoBehaviour
{
	[SerializeField] private Camera MainCamera;
    [SerializeField] private Character Character;
    [SerializeField] private Canvas Menu;
    [SerializeField] private Canvas Hud;
    [SerializeField] private Transform CharacterStart;
	[SerializeField] private Environment environment;

    private RaycastHit[] mRaycastHits;
    private Character mCharacter;
    private Environment mMap;

    private readonly int NumberOfRaycastHits = 2;

	//Paddock Script
	HUD buttons;
	PaddockHandler paddockHandle;

	//Paddock Placement - player interactions
	private bool placingPaddock = false;

	//Paddock

	//Prefabs
	GameObject paddockStandIn;

	//Lists
	public List<GameObject> paddockChildren = new List<GameObject>();
	public List<EnvironmentTile> placeablePaddock = new List<EnvironmentTile>();

	//Tile Colour
	Color32 paddockColor;

	//Bool for when placing down a paddock
	bool canPlacePaddock = false;

	//Size of Paddock
	int paddockSize = 0;

	//For spawning fences
	GameObject fence;

	//Dog 
	DogHandler dog;
	GameObject spawnedDog;
	GameObject dogClone;
	DogBehaviour dogB;
	bool placingDog = false;

	//Map
	EnvironmentTile[][] storedMap;

	//Tiles
	EnvironmentTile tile;


	//Debris Remover
	bool removingDebris = false;
	public EnvironmentTile clearTile;

	//Camera Script
	private CameraScript cam;

	//Paths
	bool placingPaths = false;
	List<Color> grassColours = new List<Color>();

	//Shops
	bool placingShops = false;
	public GameObject burgerShop;
	GameObject burgerStandIn;
	GameObject shopClone;
	List<GameObject> shops = new List<GameObject>();

	//Currency
	PlayerCurrency currency;

	//Item costs
	int shopCost = 10;
	int dogCost = 20;
	int paddockCost = 10;
	int pathCost = 5;
	int foodCost = 5;
	int decorationCost = 10;

	//Dog Profile
	GameObject profile;
	DogProfile profiles;

	//Paddock  Profile
	GameObject paddockProfile;
	PaddockProfiles PProfiles;

	//Food
	bool placingFood = false;
	Paddock interactingPaddock;

	//Sprites for when doing actions
	GameObject actionSprite;
	public Sprite foodSprite;
	public Sprite hammer;
	public Sprite remove;

	//Visitors
	VisitorHandler vis;
	bool spawnedVisitors = false;

	//GameState
	bool inGame = false;
	bool inMenu = false;

	//Park 
	Park park;

	//Decorations
	Decorations decorations;
	bool placingDeco = false;
	GameObject spawnedDeco;
	GameObject decoClone;
	int decoSelected = 0;

	public GameObject bounderyArea;

	//Pause
	public GameObject pauseScreen;
	//Tutorial
	public GameObject tutorialScreen;
	//HUD
	public GameObject HUDObj;
	//Public
	Events events;


	void Start()
    {
        mRaycastHits = new RaycastHit[NumberOfRaycastHits];
        mMap = GetComponentInChildren<Environment>();
      //  mCharacter = Instantiate(Character, transform);
		Character.setCharacterType(1);

		//Setting up gameobjects
		paddockProfile = GameObject.Find("PaddockStats");
		actionSprite = GameObject.Find("Sprite");
		profile = GameObject.Find("DogStats");

		//Setting up gameobjects with the appropriate scripts
		paddockHandle = GameObject.Find("PaddockHandler").GetComponent<PaddockHandler>();
		dog = GameObject.Find("DogHandler").GetComponent<DogHandler>();
		environment = GameObject.Find("Environment").GetComponent<Environment>();
		cam = GameObject.Find("MainCamera").GetComponent<CameraScript>();
		currency = GameObject.Find("Currency").GetComponent<PlayerCurrency>();
		park = GameObject.Find("ParkHandler").GetComponent<Park>();
		vis = GameObject.Find("VisitorHandler").GetComponent<VisitorHandler>();
		decorations = GameObject.Find("Decoration").GetComponent<Decorations>();
		events = GameObject.Find("Event").GetComponent<Events>();

		//Set up the paddock colour - darker green than the tiles
		paddockColor = new Color32(60, 107, 62, 1);
		actionSprite.SetActive(false);

		//Three seperate colours for grass - used for when deleting paths
		grassColours.Add(new Color32(98, 214, 164, 1));
		grassColours.Add(new Color32(122, 221, 159, 1));
		grassColours.Add(new Color32(105, 229, 140, 1));

		pauseScreen.SetActive(false);
		tutorialScreen.SetActive(false);

			ShowMenu(true);
	}

	private void Update()
	{
		// Check to see if the player has clicked a tile and if they have, try to find a path to that 
		// tile. If we find a path then the character will move along it to the clicked tile. 
		Ray screenClick = MainCamera.ScreenPointToRay(Input.mousePosition);
		int hits = Physics.RaycastNonAlloc(screenClick, mRaycastHits);

		//If the ray cast hits anything
		if (hits > 0)
		{
			//Grab the first ray cast hit and assign the tile to that transform
			tile = mRaycastHits[0].transform.GetComponent<EnvironmentTile>();

			//Only update when asked to (bool)
			if (placingPaddock)
			{
				paddockStandIn.SetActive(true);
				placePaddockUpdate(tile, getPaddockSize());
			}
			else if (placingDog)
			{
				placeDog(tile);
			}
			else if (removingDebris)
			{
				actionSprite.SetActive(true);
				actionSprite.GetComponent<Image>().sprite = remove;
				actionSprite.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 20, Input.mousePosition.z);
				clearDebris(tile);
			}
			else if (placingPaths)
			{
				actionSprite.SetActive(true);
				actionSprite.GetComponent<Image>().sprite = hammer;
				actionSprite.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 20, Input.mousePosition.z);
				placePathways(tile);
			}
			else if (placingShops)
			{
				burgerShop.SetActive(true);
				placeShopUpdate(tile);
			}
			else if (placingFood)
			{
				actionSprite.SetActive(true);
				actionSprite.GetComponent<Image>().sprite = foodSprite;
				actionSprite.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 20, Input.mousePosition.z);
			}
			else if(placingDeco)
			{
				if (tile == null)
				{
					tile = mRaycastHits[1].transform.GetComponent<EnvironmentTile>();
				}
				placeDecoUpdate(tile);
			}

			//Unselect all buttons
			//Right mouse click
			//For stopping current action
			if (Input.GetMouseButtonDown(1))
			{
				//Clear up any objects and variables within the scene
				placingDog = false;
				placingPaths = false;
				removingDebris = false;
				placingShops = false;
				placingFood = false;
				placingDeco = false;

				//Stand in
				profile.transform.localPosition = new Vector3(0, 300, 0);
				paddockProfile.transform.localPosition = new Vector3(0, 300, 0);

				//Sprite
				actionSprite.SetActive(false);

				if (spawnedDog != null)
				{
					Destroy(spawnedDog);
				}

				if(spawnedDeco != null)
				{
					Destroy(spawnedDeco);
				}

				if (placingPaddock)
				{
					Destroy(paddockStandIn);
					placingPaddock = false;
				}

				Destroy(burgerStandIn);
			}

			if (!inMenu && !doingAction())
			{
				//Only move character when not currently doing something
				//if (Input.GetMouseButtonDown(0))
				//{
				//	//If that tile exists, plan a route to walk on
				//	if (tile != null)
				//	{
				//		List<EnvironmentTile> route = mMap.Solve(mCharacter.CurrentPosition, tile, Character.getCharacterType());
				//		mCharacter.GoTo(route);
				//	}

				//}

				if (Physics.Raycast(screenClick, out mRaycastHits[0]))
				{
					//If the raycast is hitting a dog, and not just placing one
					if (mRaycastHits[0].transform.tag == "Dog" && !placingDog)
					{
						//Get the script and assign it to a gameobject
						profiles = mRaycastHits[0].transform.GetComponentInChildren<DogProfile>();

						//Makes sure profiles has been initiated first before calling functions
						if (profiles != null && !inMenu)
						{
							profiles.showProfile();
							showDogProfile();
						}
					}
					else
					{
						//Stand in
						profile.transform.localPosition = new Vector3(0, 300, 0);
					}


					//If the raycast is hitting a dog, and not just placing one
					if (mRaycastHits[0].transform.tag == "Paddock" && !doingAction())
					{
						if (Input.GetMouseButtonDown(0))
						{
							if (placingFood)
							{
								if (currency.sufficientFunds(foodCost))
								{
									interactingPaddock = mRaycastHits[0].transform.GetComponentInParent<Paddock>().getPaddock();
									placeFood(interactingPaddock);
									actionSprite.SetActive(false);

									currency.takeIncome(foodCost);
								}
							}
						}

						//If on the specific part of the paddock
						if (!placingFood && !inMenu)
						{
							PProfiles = mRaycastHits[0].transform.GetComponentInParent<PaddockProfiles>();
							PProfiles.showProfile();
							showPaddockProfile();
						}
					}
					else
					{
						paddockProfile.transform.localPosition = new Vector3(0, 300, 0);
					}


					if (mRaycastHits[0].transform.name == "Hooligan(Clone)" && !doingAction())
					{
						if (Input.GetMouseButtonDown(0))
						{
							events.releaseTheDogs();
						}
					}
				}
			}
		}
	}


	void showDogProfile()
	{
		profile.SetActive(true);
		profile.transform.position = new Vector3(Input.mousePosition.x + 50, Input.mousePosition.y + 50, Input.mousePosition.z);
	}

	void showPaddockProfile()
	{
		paddockProfile.transform.position = new Vector3(Input.mousePosition.x + 50, Input.mousePosition.y + 50, Input.mousePosition.z);
	}

	void placeShopUpdate(EnvironmentTile tile)
	{
		if(tile != null)
		{
			//Shpw the shop at the mouse position in the centre of the tile
			burgerStandIn.transform.position = new Vector3(mRaycastHits[0].transform.position.x + 5, mRaycastHits[0].transform.position.y + 3, mRaycastHits[0].transform.position.z + 5);
		}

		//Rotate the shop using the scroll click or space
		if(Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.Space))
		{
			burgerStandIn.transform.Rotate(Vector3.up, 90.0f);

		}
		else if(Input.GetMouseButtonDown(0) && !tile.isPaddock && !tile.isPath)
		{
			if (tile.IsAccessible)
			{
				if (currency.sufficientFunds(shopCost))
				{
					shopClone = Instantiate(burgerShop);
					shopClone.transform.rotation = burgerStandIn.transform.rotation;
					shopClone.transform.position = new Vector3(tile.transform.position.x + 5, tile.transform.position.y + 3, tile.transform.position.z + 5);
					shopClone.GetComponent<ConcessionStand>().startProduction();
					shopClone.GetComponent<ConcessionStand>().tilePlacedOn(tile);

					shops.Add(shopClone);

					tile.IsAccessible = false;

					currency.takeIncome(shopCost);

					shopClone.transform.parent = tile.transform;
					
				}
			}

			//buttons.setButtons(true);
			Destroy(burgerStandIn);

			placingShops = false;
		}
	}

	void placeDecoUpdate(EnvironmentTile tile)
	{
		if (tile != null)
		{
			//Shpw the shop at the mouse position in the centre of the tile
			spawnedDeco.transform.position = new Vector3(mRaycastHits[0].transform.position.x + 5, mRaycastHits[0].transform.position.y + 3, mRaycastHits[0].transform.position.z + 5);
		}

		//Rotate the shop using the scroll click or space
		if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.Space))
		{
			spawnedDeco.transform.Rotate(Vector3.up, 90.0f);

		}
		else if (Input.GetMouseButtonDown(0) && !tile.isPaddock && !tile.isPath)
		{
			if (tile.IsAccessible)
			{
				if (currency.sufficientFunds(decorationCost))
				{
					decoClone = decorations.pickDecoration(decoSelected);
					decoClone.transform.rotation = spawnedDeco.transform.rotation;
					decoClone.transform.position = new Vector3(tile.transform.position.x + 5, tile.transform.position.y + 3, tile.transform.position.z + 5);
					tile.IsAccessible = false;
					currency.takeIncome(decorationCost);

					decoClone.transform.parent = tile.transform;

				}
			}

			//	buttons.setButtons(true);
			Destroy(spawnedDeco);
			placingDeco = false;
		}
	}

	void placePaddockUpdate(EnvironmentTile tile, int size)
	{
		if (tile != null)
		{
			paddockStandIn.transform.position = new Vector3(mRaycastHits[0].transform.position.x, mRaycastHits[0].transform.position.y, mRaycastHits[0].transform.position.z);
		}

		if (Input.GetMouseButtonDown(0))
		{
			paddockStandIn.transform.position = new Vector3(paddockStandIn.transform.position.x, paddockStandIn.transform.position.y - 1, paddockStandIn.transform.position.z);

			if (currency.sufficientFunds(paddockCost * size))
			{

				//Get the map
				storedMap = environment.getMap();

				//Store all the children within the paddock gameobject
				foreach (Transform child in paddockStandIn.transform)
				{
					paddockChildren.Add(child.gameObject);
				}

				//CHANGE THIS TO MINIMISE LOOPING THROUGH THE FULL MAP
				for (int i = 0; i < environment.getMapSize().x; i++)
				{
					for (int j = 0; j < environment.getMapSize().y; j++)
					{
						if (storedMap[i][j].GetComponent<BoxCollider>() != null)
						{
							for (int y = 0; y < paddockChildren.Count; y++)
							{
								if (checkCollission(storedMap[i][j].transform.position, paddockChildren[y].transform.position))
								{
									placeablePaddock.Add(storedMap[i][j]);
								}
							}
						}
					}

					placingPaddock = false;
				}

				//Make it so that the paddock is placeable unless decided otherwise
				canPlacePaddock = true;

				//Check to ensure that the paddock is not obstructed
				for (int k = 0; k < placeablePaddock.Count; k++)
				{
					//If any of the positions are blocked by debris the boolean will be set to false to avoid placing the paddock down
					if (!placeablePaddock[k].IsAccessible || placeablePaddock[k].isPaddock)
					{
						Debug.Log("Paddock Position is unavaliable");
						canPlacePaddock = false;
					}
				}

				//Place the paddock down and set the boolean back to false
				if (canPlacePaddock)
				{
					spawnPaddock(placeablePaddock, getPaddockSize());
					//Set it back to false to avoid placing down nummerous paddocks on top of each other
					canPlacePaddock = false;
					currency.takeIncome(paddockCost * size);

				}
				else
				{
					Destroy(paddockStandIn);
					placingPaddock = false;
				}
			}
			else
			{
				Destroy(paddockStandIn);
				placingPaddock = false;
			}

			////Clear the list for the next paddock
			paddockChildren.Clear();
			placeablePaddock.Clear();

		}
	}

	//Spawn the paddock
	void spawnPaddock(List<EnvironmentTile> list, int size)
	{
		for (int y = 0; y < paddockStandIn.transform.childCount; y++)
		{
			Destroy(paddockStandIn.transform.GetChild(y).gameObject);
		}

		//Parameters of the paddock
		paddockStandIn.GetComponent<Paddock>().paddockSetUp(size);

		//Set up the tiles as a paddock including colour and whether its accessible
		for (int i= 0; i < list.Count; i++)
		{
			list[i].isPaddock = true;

			//Change the color of the tile to be that of the paddock
			int ran = Random.Range(0, grassColours.Count);
			list[i].GetComponent<Renderer>().materials[1].color = grassColours[ran];

			list[i].transform.parent = paddockStandIn.transform;
		}

		//Place fence around the paddock at the centre position
		fence = Instantiate(paddockHandle.getFence(getPaddockSize()));

		//Positions for each fence are different 
		switch(getPaddockSize())
		{
			case 3: fence.transform.localPosition = new Vector3(paddockStandIn.transform.localPosition.x + 5, paddockStandIn.transform.localPosition.y + 14.5f, paddockStandIn.transform.localPosition.z + 15);
				break;
			case 4: fence.transform.localPosition = new Vector3(paddockStandIn.transform.localPosition.x + 25, paddockStandIn.transform.localPosition.y + -29, paddockStandIn.transform.localPosition.z + 48);
				break;
			case 5: fence.transform.localPosition = new Vector3(paddockStandIn.transform.localPosition.x + 20, paddockStandIn.transform.localPosition.y - 8, paddockStandIn.transform.localPosition.z + 32);
				break;
			default:
				fence.transform.localPosition = new Vector3(paddockStandIn.transform.localPosition.x + 25, paddockStandIn.transform.localPosition.y + -28, paddockStandIn.transform.localPosition.z + 48);
				break;
		}

		fence.transform.parent = paddockStandIn.transform;

		paddockHandle.addPaddock(paddockStandIn);
	}

	void placePathways(EnvironmentTile tile)
	{
		//Create a path at the given tile, mark as a path
		if (Input.GetMouseButtonDown(0) && !tile.isPaddock && !tile.isPath && tile.IsAccessible)
		{
			if (currency.sufficientFunds(pathCost))
			{
				tile.GetComponent<Renderer>().materials[1].color = new Color32(196, 136, 75, 1);
				tile.isPath = true;

				vis.addPath(tile);

				currency.takeIncome(pathCost);

				for(int i=0; i < shops.Count; i++)
				{
					shops[i].GetComponent<ConcessionStand>().checkTiles();
				}
			}
		}
	}

	void placeDog(EnvironmentTile tile)
	{
		if (tile != null)
		{
			//Shpw the shop at the mouse position in the centre of the tile
			spawnedDog.transform.position = new Vector3(mRaycastHits[0].transform.position.x + 5, mRaycastHits[0].transform.position.y + 3, mRaycastHits[0].transform.position.z + 5);

			//Only place a dog if within a paddock and a dog isn't currently within that position
			if (Input.GetMouseButtonDown(0))
			{
				if (tile.isPaddock)
				{
					
					if (currency.sufficientFunds(dogCost))
					{
						if (tile.GetComponentInParent<Paddock>().addDogs())
						{
							if (!tile.hasDog)
							{
								dogClone = dog.spawnDog();
								dogClone.transform.position = tile.Position;

								//Assign the paddock to the dog
								dogB = dogClone.GetComponentInChildren<DogBehaviour>();
								dogB.setTile(tile);
								dogB.setPaddock(tile.GetComponentInParent<Paddock>().getPaddock());
								dogB.setPaddockTiles(tile.GetComponentInParent<Paddock>().getPaddockTiles());


								interactingPaddock = mRaycastHits[0].transform.GetComponentInParent<Paddock>().getPaddock();
								interactingPaddock.updateDogList(dogClone);

								placingDog = false;

								currency.takeIncome(dogCost);
							}
						}
					}
				}

				//Destroy stand in

				Destroy(spawnedDog.gameObject);

				placingDog = false;
			}
		}

	}


	void placeFood(Paddock paddock)
	{
		paddock.addFood(10);
	}

	void clearDebris(EnvironmentTile tile)
	{
		//Only change the tile is it is appropriate
		if (Input.GetMouseButtonDown(0))
		{
			if (!tile.IsAccessible && !tile.isPaddock)
			{
				//Remove the object on top of the tile and set it to accessible
				if (tile.gameObject.transform.childCount > 0)
				{
					//SHOPS
					if(tile.gameObject.transform.GetChild(0).GetComponent<ConcessionStand>())
					{
						shops.Remove(tile.gameObject.transform.GetChild(0).gameObject);
						//Give back shop money
						currency.addIncome(shopCost);

						//Delete the object
						Destroy(tile.gameObject.transform.GetChild(0).gameObject);

						tile.IsAccessible = true;
					}
					//DECORATIONS
					else if(tile.gameObject.transform.GetChild(0).tag == "Decoration")
					{
						//Give back the money for destoying decorations
						currency.addIncome(decorationCost);
						//Delete the object
						Destroy(tile.gameObject.transform.GetChild(0).gameObject);

						tile.IsAccessible = true;
					}
					//DEBRIS
					else if(currency.sufficientFunds(5))
					{
						//clearing debris costs money
						currency.takeIncome(5);

						//Delete the object
						Destroy(tile.gameObject.transform.GetChild(0).gameObject);

						tile.IsAccessible = true;
					}
				}
			}
			//PATHS
			else if(tile.isPath)
			{
				tile.isPath = false;
				int ran = Random.Range(0, grassColours.Count);
				tile.GetComponent<Renderer>().materials[1].color = grassColours[ran];
				currency.addIncome(pathCost);
			}
		}
	}

	//Function to return true if the player is currently doing any action
	bool doingAction()
	{
		if(placingPaddock || removingDebris || placingDog || placingPaths || placingShops || placingDeco)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	//Used for checking is when placing a paddock there is a position that can be used
	bool checkCollission(Vector3 pos1, Vector3 pos2)
	{
		//Check that the positions are equal to each other
		if (pos1 == pos2)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void ShowMenu(bool show)
    {

		if (Menu != null && Hud != null)
        {
            Menu.enabled = show;
            Hud.enabled = !show;

            if( show )
            {
               // mCharacter.transform.position = CharacterStart.position;
               // mCharacter.transform.rotation = CharacterStart.rotation;
                mMap.CleanUpWorld();

				spawnedVisitors = false;
				inGame = false;
				inMenu = true;

			}
            else
            {
               // mCharacter.transform.position = mMap.Start.Position;

				//mCharacter.transform.rotation = Quaternion.identity;

              //  mCharacter.CurrentPosition = mMap.Start;

				inGame = true;
				inMenu = false;

				bounderyArea.transform.localPosition = new Vector3(-505, -9, -440);

				if (!spawnedVisitors)
				{
					vis.spawnVisitor();
					vis.startTimer();
					spawnedVisitors = true;
				}
			}
        }
    }

    public void Generate()
    {
        mMap.GenerateWorld();
    }

    public void Exit()
    {
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }

	public void placePaddock(bool set)
	{
		placingPaddock = set;
	}

	public void setPaddockSize(int size)
	{
		paddockStandIn = paddockHandle.createPaddock(new Vector3(0, 0, 0), size);
		paddockSize = size;

	}

	public int getPaddockSize()
	{
		return paddockSize;
	}

	public void placeDog(bool set)
	{
		placingDog = set;
	}

	public void removeDebris(bool set)
	{
		removingDebris = set;
	}

	public void placePath(bool set)
	{
		placingPaths = set;
	}

	public void placeShop(bool set)
	{
		placingShops = set;
	}

	public void placeFood(bool set)
	{
		placingFood = set;
	}

	public void placeDeco(bool set)
	{
		placingDeco = set;
	}

	public void createBurgerShop()
	{
		burgerStandIn = Instantiate(burgerShop);
	}

	public void createDog(int dogSelected)
	{
		spawnedDog = dog.selectDog(dogSelected);
	}

	public void createDeco(int deco)
	{
		spawnedDeco = decorations.pickDecoration(deco);
		decoSelected = deco;
	}

	public bool getGameState()
	{
		return inGame;
	}

	public bool getInMenu()
	{
		return inMenu;
	}

	public void hideActionSprite()
	{
		actionSprite.SetActive(false);
	}

	public void setInMenu(bool set)
	{
		inMenu = set;
	}

	public void gamePaused(bool set)
	{
		if(set)
		{
			Time.timeScale = 0;
			HUDObj.SetActive(false);
		}
		else
		{
			Time.timeScale = 1;
			HUDObj.SetActive(true);
		}
	}

	public void showPause(bool show)
	{
		pauseScreen.SetActive(show);
	}

	public void showTutorial(bool show)
	{
		tutorialScreen.SetActive(show);

		if(show)
		{
			pauseScreen.SetActive(false);
		}
		else
		{
			pauseScreen.SetActive(true);
		}
	}
}
