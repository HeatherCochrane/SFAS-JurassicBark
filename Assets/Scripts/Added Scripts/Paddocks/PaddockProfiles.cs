using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaddockProfiles : MonoBehaviour
{
	//Get the UI elements
	Text hunger;
	Text happiness;
	Paddock profiles;

	string hungerText;
	string happinessText;

	// Start is called before the first frame update
	void Start()
    {
		hunger = GameObject.Find("HungerPaddock").GetComponent<Text>();
		happiness = GameObject.Find("HappinessPaddock").GetComponent<Text>();

		profiles = this.transform.GetComponentInParent<Paddock>();
		updateValues();
	}

    // Update is called once per frame
    void Update()
    {
	}

	public void showProfile()
	{
		if(profiles != null)
		{
			updateValues();
		}
	}

	public void updateValues()
	{
		hunger.text = "Food Levels: " + profiles.getHunger();
		happiness.text = "Happiness Levels: " + profiles.getHappiness();
	}
}
