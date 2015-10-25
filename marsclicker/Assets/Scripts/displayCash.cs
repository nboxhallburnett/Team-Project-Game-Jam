using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class displayCash : MonoBehaviour {

	Text cashDisplay;

	// Use this for initialization
	void Start () {
		cashDisplay = GetComponent<Text> ();
		cashDisplay.text = GameControl.data.cash.ToString("$0.00");
	}
	
	
	// Update is called once per frame
	void Update () {
		cashDisplay.text = GameControl.data.cash.ToString("$0.00");
	}
}
