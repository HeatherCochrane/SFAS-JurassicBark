using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Challenges : MonoBehaviour
{
	Button showBoard;
	PlayerCurrency currency;

	public GameObject challengeBoard;

	public List<GameObject> moneyChecks = new List<GameObject>();
	public List<GameObject> dogsChecks = new List<GameObject>();
	public List<GameObject> hooligansChecks = new List<GameObject>();

	int totalDogs = 0;
	int totalHooligansCaught = 0;
    // Start is called before the first frame update
    void Start()
    {
		challengeBoard.SetActive(false);

		showBoard = GameObject.Find("ChallengeButton").GetComponent<Button>();
		currency = GameObject.Find("Currency").GetComponent<PlayerCurrency>();


		showBoard.onClick.AddListener(delegate { if (challengeBoard.activeSelf) { challengeBoard.SetActive(false); } else { challengeBoard.SetActive(true); }; });

		for(int i=0; i < 3; i++)
		{
			moneyChecks[i].SetActive(false);
			dogsChecks[i].SetActive(false);
			hooligansChecks[i].SetActive(false);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void moneyEarned(int amount)
	{
		if (amount > 299)
		{
			moneyChecks[0].SetActive(true);
		}
		if (amount > 699)
		{
			moneyChecks[1].SetActive(true);
		}
		if (amount > 999)
		{
			moneyChecks[2].SetActive(true);
		}

	}

	public void dogsBought(int amount)
	{
		totalDogs += amount;

		if(totalDogs > 4 && totalDogs < 10)
		{
			dogsChecks[0].SetActive(true);
			currency.addIncome(50);
		}
		if(totalDogs > 9 && totalDogs < 30)
		{
			dogsChecks[1].SetActive(true);
			currency.addIncome(100);
		}
		if(totalDogs == 30)
		{
			dogsChecks[2].SetActive(true);
			currency.addIncome(500);
		}

	}

	public void hooligansCaught(int caught)
	{
		totalHooligansCaught += caught;

		if(totalHooligansCaught > 2 && totalHooligansCaught < 6)
		{
			hooligansChecks[0].SetActive(true);
			currency.addIncome(20);
		}
		else if(totalHooligansCaught > 5 && totalHooligansCaught < 10)
		{
			hooligansChecks[1].SetActive(true);
			currency.addIncome(50);
		}
		else if(totalHooligansCaught == 10)
		{
			hooligansChecks[2].SetActive(true);
			currency.addIncome(150);
		}
	}

	public void happinessReached(int happiness)
	{
		if(happiness > 70 && happiness < 79)
		{

		}
		else if(happiness > 80 && happiness < 89)
		{

		}
		else if(happiness > 90 && happiness < 99)
		{

		}
		else if(happiness == 100)
		{

		}
	}

	public void showChallengeBoard(bool set)
	{
		challengeBoard.SetActive(set);
	}
}
