using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelExp : MonoBehaviour
{
	//Variables
	int parkLevel = 0;
	int xpGain = 500;
	int currentExp = 0;

	//HUD
	Text parkLevelText;

    // Start is called before the first frame update
    void Start()
    {
		parkLevelText = GameObject.Find("ParkLevel").GetComponent<Text>();
		adjustParkLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void addExp(int added)
	{
		currentExp += added;
		checkExp();
	}

	void checkExp()
	{
		if(currentExp == xpGain)
		{
			adjustParkLevel();
			xpGain += 500;
		}

		//Debug.Log("Exp: " + currentExp + "  XpGain: " + xpGain + "  Park Level: " + parkLevel);
	}
	void adjustParkLevel()
	{
		parkLevel++;
		parkLevelText.text = "Park Level: " + parkLevel.ToString();
	}

	int getParkLevel()
	{
		return parkLevel;
	}
}
