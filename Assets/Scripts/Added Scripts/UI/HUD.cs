using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
	//Script References
	private Game game;
	private Events parkEvent;
	private Decorations decoration;

	private Button toggleShop;
	public GameObject screenParent;


	//Shop Screens
	public List<GameObject> screens = new List<GameObject>();
	public GameObject releaseTheHounds;

	//Unlockables

	public Button dog1;
	public Button dog2;
	public Button dog3;
	public Button dog4;
	public Button dog5;
	public Button dog6;

	public Button paddock1;
	public Button paddock2;
	public Button paddock3;

	List<Button> unlockables1 = new List<Button>();

	List<Button> unlockables2 = new List<Button>();

	// Start is called before the first frame update
	void Start()
    {
		toggleShop = GameObject.Find("OpenShop").GetComponent<Button>();
		game = GameObject.Find("Game").GetComponent<Game>();

		toggleShop.onClick.AddListener(delegate { if (screenParent.activeSelf == true) { screenParent.SetActive(false); game.setInMenu(false); } else { screenParent.SetActive(true); selectScreen(0); game.setInMenu(true); }; });

		screenParent.SetActive(false);

		collectButtons();

	}

	// Update is called once per frame
	void Update()
    {
       
    }

	public void hideMenu()
	{
		screenParent.SetActive(false);
	}

	public void selectScreen(int num)
	{
		//Show the correct Screen
		for(int i = 0; i < screens.Count; i++)
		{
			if(i == num)
			{
				screens[i].SetActive(true);
			}
			else
			{
				screens[i].SetActive(false);
			}
		}
	}

	public void showHoundsButton(bool set)
	{
		releaseTheHounds.SetActive(set);
	}

	public void unlockButtons(int level)
	{
		//Take one off of level to make it work for lists

		level -= 1;
		if (unlockables1[level] != null)
		{
			unlockables1[level].interactable = true;
		}
		if (unlockables2[level] != null)
		{
			unlockables2[level].interactable = true;
		}
	}

	public void collectButtons()
	{
		//List of buttons for unlockables
		unlockables1.Add(dog1);
		unlockables1.Add(dog2);
		unlockables1.Add(dog3);
		unlockables1.Add(dog4);
		unlockables1.Add(dog5);
		unlockables1.Add(dog6);

		unlockables2.Add(paddock1);
		unlockables2.Add(paddock2);
		unlockables2.Add(paddock3);

		for (int i = 0; i < unlockables1.Count; i++)
		{
			unlockables1[i].interactable = false;
		}

		for (int i = 0; i < unlockables2.Count; i++)
		{
			unlockables2[i].interactable = false;
		}
	}
}
