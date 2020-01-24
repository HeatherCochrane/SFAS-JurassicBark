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
	HUD buttons;
	Challenges challenge;

    // Start is called before the first frame update
    void Start()
    {
		parkLevelText = GameObject.Find("ParkLevel").GetComponent<Text>();
		progressBar = parkLevelText.gameObject.transform.GetChild(0).GetComponent<Slider>();
		buttons = GameObject.Find("HUD").GetComponent<HUD>();
		challenge = GameObject.Find("Challenges").GetComponent<Challenges>();
		Invoke("adjustParkLevel", 9);
		progressBar.maxValue = xpGain;
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
	}
	void adjustParkLevel()
	{
		parkLevel++;
		parkLevelText.text = "Park Level: " + parkLevel.ToString();
		buttons.unlockButtons(parkLevel);
		challenge.levelsReached(parkLevel);
	}

	int getParkLevel()
	{
		return parkLevel;
	}

}
