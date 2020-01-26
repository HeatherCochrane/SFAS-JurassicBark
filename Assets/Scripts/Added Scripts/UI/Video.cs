using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class Video : MonoBehaviour
{

	Image menu;

    // Start is called before the first frame update
    void Start()
    {
		menu = GameObject.Find("Menu").GetComponent<Image>();

		menu.transform.GetChild(0).transform.gameObject.SetActive(false);
		menu.transform.GetChild(1).transform.gameObject.SetActive(false);
		menu.color = Color.black;
		
		Invoke("intro", 8);
    }

    // Update is called once per frame
    void Update()
    {
    }

	void intro()
	{
		menu.transform.GetChild(0).transform.gameObject.SetActive(true);
		menu.transform.GetChild(1).transform.gameObject.SetActive(true);

		menu.color = new Color32(178, 210, 228, 225);
		Destroy(this.gameObject);
	}
}
