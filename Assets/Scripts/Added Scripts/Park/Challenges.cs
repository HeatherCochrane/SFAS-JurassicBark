using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Challenges : MonoBehaviour
{
	Button showBoard;
	PlayerCurrency currency;

	public GameObject challengeBoard;

	//Lists of checks on screen
	public List<GameObject> dogsChecks = new List<GameObject>();
	public List<GameObject> hooligansChecks = new List<GameObject>();
	public List<GameObject> levelChecks = new List<GameObject>();
	public GameObject happinessCheck;

	int totalDogs = 0;
	int totalHooligansCaught = 0;

	//Keep track of the challenges completed
	int totalChallengesFinished = 0;

	public Image endScreen;

    // Start is called before the first frame update
    void Start()
    {
		challengeBoard.SetActive(false);

		showBoard = GameObject.Find("ChallengeButton").GetComponent<Button>();
		currency = GameObject.Find("Currency").GetComponent<PlayerCurrency>();

		endScreen.gameObject.SetActive(false);

		showBoard.onClick.AddListener(delegate { if (challengeBoard.activeSelf) { challengeBoard.SetActive(false); } else { challengeBoard.SetActive(true); }; });

		//Dont show the ticks on load
		for(int i=0; i < 3; i++)
		{
			dogsChecks[i].SetActive(false);
			hooligansChecks[i].SetActive(false);
			levelChecks[i].SetActive(false);
		}

		happinessCheck.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void dogsBought(int amount)
	{
		totalDogs += amount;

		if(totalDogs == 5)
		{
			dogsChecks[0].SetActive(true);
			currency.addIncome(20);
			totalChallengesFinished++;
		}
		else if(totalDogs == 10)
		{
			dogsChecks[1].SetActive(true);
			currency.addIncome(50);
			totalChallengesFinished++;
		}
		else if(totalDogs == 30)
		{
			dogsChecks[2].SetActive(true);
			currency.addIncome(500);
			totalChallengesFinished++;
		}

	}

	public void hooligansCaught(int caught)
	{
		totalHooligansCaught += caught;
		if(totalHooligansCaught == 3)
		{
			hooligansChecks[0].SetActive(true);
			currency.addIncome(20);
			totalChallengesFinished++;
		}
		else if(totalHooligansCaught == 6)
		{
			hooligansChecks[1].SetActive(true);
			currency.addIncome(50);
			totalChallengesFinished++;
		}
		else if(totalHooligansCaught == 10)
		{
			hooligansChecks[2].SetActive(true);
			currency.addIncome(150);
			totalChallengesFinished++;
		}
	}

	public void happinessReached(int happiness)
	{
		if(happiness == 100)
		{
			happinessCheck.SetActive(true);
			currency.addIncome(250);
			totalChallengesFinished++;
		}
	}

	public void levelsReached(int level)
	{
		if (level == 2)
		{
			levelChecks[0].SetActive(true);
			currency.addIncome(20);
			totalChallengesFinished++;

		}
		else if (level == 4)
		{
			levelChecks[1].SetActive(true);
			currency.addIncome(50);
			totalChallengesFinished++;
		}
		else if(level == 6)
		{
			levelChecks[2].SetActive(true);
			currency.addIncome(100);
			totalChallengesFinished++;
		}
	}

	public void showChallengeBoard(bool set)
	{
		challengeBoard.SetActive(set);
	}

	void checkEndGame()
	{
		if(totalChallengesFinished == 13)
		{
			endScreen.gameObject.SetActive(true);

			Invoke("hideEndGame", 5);
		}
	}

	void hideEndGame()
	{
		endScreen.gameObject.SetActive(false);
	}
}
