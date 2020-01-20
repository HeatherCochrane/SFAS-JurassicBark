using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DogHandler : MonoBehaviour
{
	Environment mMap;

	//Dog Prefab
	GameObject dog;
	GameObject staticDog;


	public List<GameObject> dogBreeds = new List<GameObject>();
	public List<GameObject> dogsStatic = new List<GameObject>();

	int selectedDog = 0;

	List<GameObject> allDogs = new List<GameObject>();

	Challenges challenge;

	// Start is called before the first frame update
	void Start()
    {
		challenge = GameObject.Find("Challenges").GetComponent<Challenges>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public GameObject spawnDog()
	{
		dog = Instantiate(dogBreeds[selectedDog]);
		dog.transform.rotation = Quaternion.identity;

		allDogs.Add(dog);
		challenge.dogsBought(1);

		return dog;
	}

	public GameObject selectDog(int dog)
	{
		staticDog = Instantiate(dogsStatic[dog]);
		selectedDog = dog;
		return staticDog;
	}

	public GameObject getDog()
	{
		return dog;
	}

	public List<GameObject> getDogList()
	{
		return allDogs;
	}
}
