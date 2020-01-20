using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class DogProfile : MonoBehaviour
{
	DogBehaviour dog;

	//Text On Screen
	Text dogName;
	Text dogHappiness;

	string nameS;
	int happinessI;

	// Start is called before the first frame update
	void Start()
    {
		dogName = GameObject.Find("Name").GetComponent<Text>();
		dogHappiness = GameObject.Find("Happiness").GetComponent<Text>();

		dog = this.transform.GetComponent<DogBehaviour>();
	}

    // Update is called once per frame
    void Update()
    {
		happinessI = dog.getHappiness();
	}

	public void showProfile()
	{
		if (dog != null)
		{
			nameS = dog.getName();
			happinessI = dog.getHappiness();

			dogName.text = "Name: " +  nameS;
			dogHappiness.text = "Happiness: " + happinessI.ToString();
		}
	}
}
