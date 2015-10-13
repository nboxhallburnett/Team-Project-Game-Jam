using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class displayMultiplier : MonoBehaviour {

	Text multiplierDisplay;
	
	// Use this for initialization
	void Start () {
		multiplierDisplay = GetComponent<Text> ();
		multiplierDisplay.text = GameControl.data.multiplier + "x: " + GameControl.data.multiplierTimer.ToString("0.00") + "s";
	}
	
	
	// Update is called once per frame
	void Update () {
		if (GameControl.data.multiplier != 1.0f) {
			multiplierDisplay.text = GameControl.data.multiplier + "x: " + GameControl.data.multiplierTimer.ToString("0.00") + "s";
		} else {
			multiplierDisplay.text = "";
		}
	}
}
