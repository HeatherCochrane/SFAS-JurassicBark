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
	Slider progressBar;

    // Start is called before the first frame update
    void Start()
    {
		parkLevelText = GameObject.Find("ParkLevel").GetComponent<Text>();
		progressBar = parkLevelText.gameObject.transform.GetChild(0).GetComponent<Slider>();
		adjustParkLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void addExp(int added)
	{
		currentExp += added;
		progressBar.value += added;
		checkExp();
	}

	void checkExp()
	{
		if(currentExp == xpGain)
		{
			adjustParkLevel();
			xpGain += 500;
			progressBar.value = 0;
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
