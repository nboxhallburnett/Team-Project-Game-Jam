using UnityEngine;
using System.Collections;

public class clickme : MonoBehaviour {
	float money;

	// Use this for initialization
	void Start () {
		//initialisation
		money = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetMouseButtonDown(0)){
			RaycastHit hit = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//Transform select = GameObject.FindWithTag("mars").transform;
			if (Physics.Raycast (ray, out hit, 100.0f)){
                if(hit.collider.tag == "mars")
                money += 1.2f;
			}
		}
	}

	void OnGUI() {
		GUI.Label(new Rect(10, 10, 100, 20), "$" + Mathf.Floor(money).ToString());
	}
}
