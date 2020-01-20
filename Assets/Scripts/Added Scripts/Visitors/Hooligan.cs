using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hooligan : MonoBehaviour
{
	Events parkEvent;
	// Start is called before the first frame update
	void Start()
	{
		parkEvent = GameObject.Find("Event").GetComponent<Events>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Dog")
		{
			parkEvent.hooliganCaught(true);
		}
	}
}
