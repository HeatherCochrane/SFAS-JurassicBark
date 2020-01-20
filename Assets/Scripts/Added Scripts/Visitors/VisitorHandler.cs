using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorHandler : MonoBehaviour
{

	//Timer
	IEnumerator dayNightCycle()
	{
		while (true)
		{

			if(!parkClosed)
			{
				parkClosed = true;
				closePark();
			}
			else
			{
				parkClosed = false;
				openPark();
			}

			yield return new WaitForSeconds(150);
		}

	}

	//https://gamedev.stackexchange.com/questions/118305/how-do-i-lerp-text-color-over-time
	IEnumerator UpdateLightColor(Color32 start, Color32 end)
	{
		float t = 0;
		while (t < 1)
		{
			// Now the loop will execute on every end of frame until the condition is true
			mainLight.color = Color.Lerp(start, end, t);
			t += Time.deltaTime / 4;
			yield return new WaitForEndOfFrame(); // So that I return something at least.
		}
	}

	Environment mMap;
	Park park;
	Events parkEvent;

	public List<Character> character = new List<Character>();
	PlayerCurrency currency;


	Character visitor;

	int visitorCount = 30;
	int visitorEntryFee = 5;

	int ranNum = 0;

	bool parkClosed = true;

	List<EnvironmentTile> paths = new List<EnvironmentTile>();

	public List<Character> allVisitors = new List<Character>();

	public Light mainLight;

	// Start is called before the first frame update
	void Start()
    {
		mMap = GameObject.Find("Environment").GetComponent<Environment>();
		currency = GameObject.Find("Currency").GetComponent<PlayerCurrency>();
		park = GameObject.Find("ParkHandler").GetComponent<Park>();
		parkEvent = GameObject.Find("Event").GetComponent<Events>();
	}

	public void startTimer()
	{
		StartCoroutine(dayNightCycle());
	}


	// Update is called once per frame
	void Update()
    {

    }

	public void spawnVisitor()
	{
		//Spawn up to a certain number of visitors for the park
		if (visitorCount > 0)
		{
			ranNum = Random.Range(0, character.Count);
			visitor = Instantiate(character[ranNum], transform);

			allVisitors.Add(visitor);

			//Place visitor at the starting position on the map
			visitor.transform.position = mMap.Start.Position;
			visitor.transform.rotation = Quaternion.identity;
			visitor.CurrentPosition = mMap.Start;

			//For each new visitor, add on the entry fee to the total income
			currency.addIncome(park.getEntryFee());

			Invoke("spawnVisitor", 5);

			visitorCount -= 1;
		}
	}


	void closePark()
	{
		//Tell all the current visitors to leave the park
		for(int i=0; i < allVisitors.Count; i++)
		{
			parkClosed = true;
			allVisitors[i].GetComponent<Visitor>().goToExit();
			allVisitors[i].GetComponent<Visitor>().setPark(parkClosed);
		}

		//Empty the list to avoid having missing objects within the list positons
		allVisitors.Clear();
		visitorCount = 0;

		parkEvent.startCountdown(Random.Range(25, 40));
		StartCoroutine(UpdateLightColor(new Color32(255, 255, 255, 1), new Color32(56, 24, 77, 1)));

	}

	void openPark()
	{
		parkClosed = false;
		visitorCount = 30;
		spawnVisitor();
		StartCoroutine(UpdateLightColor(new Color32(56, 24, 77, 1), new Color32(255, 255, 255, 1)));
	}

	public void addPath(EnvironmentTile newPath)
	{
		paths.Add(newPath);
	}

	public List<EnvironmentTile> getPaths()
	{
		return paths;
	}

	public bool getOpeningHour()
	{
		return parkClosed;
	}
}
