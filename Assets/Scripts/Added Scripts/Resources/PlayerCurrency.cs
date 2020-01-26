using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrency : MonoBehaviour
{

	//Starting amount
	int totalIncome = 100;
	int remainingFunds = 0;

	public Text money;
	public GameObject fundWarning;
	Challenges challenges;


	public Sprite added;
	public Sprite taken;

	GameObject moneyAnimation;


	// Start is called before the first frame update
	void Start()
    {
		money.text = "£" + totalIncome.ToString();
		fundWarning.SetActive(false);

		challenges = GameObject.Find("Challenges").GetComponent<Challenges>();


		moneyAnimation = GameObject.Find("MoneyAnim");
		moneyAnimation.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void addIncome(int income)
	{
		totalIncome += income;
		updateUI();
		moneyAdded();
	}

	public void takeIncome(int income)
	{
		totalIncome -= income;

		if(totalIncome < 0)
		{
			totalIncome = 0;
		}
		updateUI();
		moneyTaken();
	}

	void updateUI()
	{
		money.text = "£" + totalIncome.ToString();
	}
	public int getTotalIncome()
	{
		return totalIncome;
	}

	public bool sufficientFunds(int income)
	{
		remainingFunds = totalIncome - income;
		if ( remainingFunds >= 0)
		{
			return true;
		}
		else
		{
			fundWarning.SetActive(true);
			Invoke("hideFundWarning", 5);
			return false;
		}
	}

	void hideFundWarning()
	{
		fundWarning.SetActive(false);
	}

	public void moneyAdded()
	{
		moneyAnimation.SetActive(true);
		moneyAnimation.GetComponentInChildren<Image>().sprite = added;
		moneyAnimation.GetComponentInChildren<Animation>().Play("MoneyAnim");
	}

	public void moneyTaken()
	{
		moneyAnimation.SetActive(true);
		moneyAnimation.GetComponentInChildren<Image>().sprite = taken;
		moneyAnimation.GetComponentInChildren<Animation>().Play("MoneyAnim");
	}
}
