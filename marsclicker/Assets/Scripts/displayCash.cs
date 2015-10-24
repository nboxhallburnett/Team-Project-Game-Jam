using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class displayCash : MonoBehaviour {

	Text cashDisplay;

	// Use this for initialization
	void Start () {
		cashDisplay = GetComponent<Text> ();
		cashDisplay.text = "$" + Mathf.Floor(GameControl.data.cash).ToString();
	}
	
	
	// Update is called once per frame
	void Update () {
        cashDisplay.text = System.String.Format("{0:C}", Mathf.Floor(GameControl.data.cash));
	}
}
