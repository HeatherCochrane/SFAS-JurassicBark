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

    // Start is called before the first frame update
    void Start()
    {
		toggleShop = GameObject.Find("OpenShop").GetComponent<Button>();
		game = GameObject.Find("Game").GetComponent<Game>();

		toggleShop.onClick.AddListener(delegate { if (screenParent.activeSelf == true) { screenParent.SetActive(false); game.setInMenu(false); } else { screenParent.SetActive(true); selectScreen(0); game.setInMenu(true); }; });

		screenParent.SetActive(false);
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

}
