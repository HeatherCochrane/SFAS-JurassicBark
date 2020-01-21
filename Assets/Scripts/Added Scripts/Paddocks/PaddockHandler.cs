using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddockHandler : MonoBehaviour
{

	Environment environment;


	//Paddock prefabs

	public GameObject smallPrefab;
	GameObject[] newPaddock;
	public GameObject paddockParentPrefab;
	GameObject paddockParent;

	int x = 0;
	int z = 0;

	//Fences
	public GameObject smallPaddock;
	public GameObject mediumPaddock;
	public GameObject bigPaddock;


	List<GameObject> paddocksPlaced = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
		newPaddock = new GameObject[9];
    }

    // Update is called once per frame
    void Update()
    {
		
    }

	public GameObject createPaddock(Vector3 location, int size)
	{
		paddockParent = Instantiate(paddockParentPrefab);
		paddockParent.transform.position = location;

		for (int j = 0; j < size; j++)
		{
			for (int i = 0; i < size; i++)
			{
				newPaddock[i] = Instantiate(smallPrefab);
				newPaddock[i].transform.position = new Vector3(location.x + x, location.y + 1, location.z + z);
				newPaddock[i].transform.parent = paddockParent.transform;

				x += 10;
			}
			z += 10;
			x = 0;
		}

		//Reset variables back to 0
		x = 0;
		z = 0;

		return paddockParent;
	}

	public GameObject getFence(int size)
	{
		//Get the correct fence size
		switch(size)
		{
			case 3: return smallPaddock;

			case 4: return mediumPaddock;

			case 5: return bigPaddock;

			default: return smallPaddock;
		}
	}

	public void addPaddock(GameObject newPad)
	{
		paddocksPlaced.Add(newPad);
	}
	public void deletePaddock(GameObject pad)
	{
		paddocksPlaced.Remove(pad);
	}

	public List<GameObject> getPaddocks()
	{
		return paddocksPlaced;
	}
}
