using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decorations : MonoBehaviour
{
	public List<GameObject> decorations = new List<GameObject>();
	GameObject deco;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public GameObject pickDecoration(int num)
	{
		deco = Instantiate(decorations[num]);
		return deco;
	}
}
