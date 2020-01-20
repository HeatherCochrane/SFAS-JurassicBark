using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Park : MonoBehaviour
{
	struct ParkStats
	{
		public int happiness;
		public int admission;
	}

	ParkStats stats;

	List<GameObject> paddocksInPark = new List<GameObject>();
	int averageHappiness = 0;

	public Text parkHappiness;

	//Starting price
	int parkEntryFee = 5;
	int count = 0;


	int min = 0;
	int max = 0;

    // Start is called before the first frame update
    void Start()
    {
		averageHappiness = 50;
		stats.happiness = averageHappiness;
		parkHappiness.text = "Park Happiness: " + averageHappiness.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public int generateHappinessAverage()
	{
		checkPaddocks();

		if (paddocksInPark.Count > 0 && count > 0)
		{
			averageHappiness = 0;
			count = 0;

			checkPaddocks();

			if (count != 0)
			{
				//Only divide by the paddocks that have dogs within them
				averageHappiness = averageHappiness / count;
			}

			clampValues(count);

			stats.happiness = averageHappiness;
			parkHappiness.text = "Park Happiness: " + averageHappiness.ToString();
		}

		return averageHappiness;
	}

	void clampValues(int numOfPaddocks)
	{
		if(numOfPaddocks == 1)
		{
			averageHappiness = Mathf.Clamp(averageHappiness, 0, 70);
		}
		else if(numOfPaddocks == 2)
		{
			averageHappiness = Mathf.Clamp(averageHappiness, 0, 80);
		}
		else if(numOfPaddocks == 3)
		{
			averageHappiness = Mathf.Clamp(averageHappiness, 0, 90);
		}
		else
		{
			averageHappiness = Mathf.Clamp(averageHappiness, 0, 100);
		}
	}

	void checkPaddocks()
	{
		//Get the average happiness of all the paddocks to generate the overall park happiness
		for (int i = 0; i < paddocksInPark.Count; i++)
		{
			if (paddocksInPark[i].GetComponent<Paddock>().getDogs() > 0)
			{
				averageHappiness += paddocksInPark[i].GetComponent<Paddock>().getHappiness();
				count += 1;
			}
		}
	}

	public void setEntryFee()
	{
		if(averageHappiness < 10)
		{
			parkEntryFee = 0;
		}
		else if(averageHappiness < 40)
		{
			parkEntryFee = 1;
		}
		else if(averageHappiness < 60)
		{
			parkEntryFee = 3;
		}
		else if(averageHappiness < 80)
		{
			parkEntryFee = 5;
		}
		else if(averageHappiness == 100)
		{
			parkEntryFee = 10;
		}
	}

	public int getEntryFee()
	{
		setEntryFee();
		return parkEntryFee;
	}

	public void addPaddock(GameObject pad)
	{
		paddocksInPark.Add(pad);
	}

	public int getAdmissionCost()
	{
		return stats.admission;
	}

	public int getHappiness()
	{
		return stats.happiness;
	}

}
