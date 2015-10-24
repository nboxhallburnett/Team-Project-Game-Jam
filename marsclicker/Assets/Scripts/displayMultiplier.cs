using UnityEngine;
using UnityEngine.UI;

public class displayMultiplier : MonoBehaviour {

	Text multiplierDisplay;
    Image boxImage;
    public GameObject box;
	
	// Use this for initialization
	void Start () {
        boxImage = box.GetComponent<Image>();
        boxImage.enabled = false;
        multiplierDisplay = GetComponent<Text> ();
		multiplierDisplay.text = GameControl.data.multiplier + "x: " + GameControl.data.multiplierTimer.ToString("0.00") + "s";
	}
	
	// Update is called once per frame
	void Update () {
		if (GameControl.data.multiplier != 1.0f) {
            boxImage.enabled = true;
            multiplierDisplay.text = GameControl.data.multiplier + "x: " + GameControl.data.multiplierTimer.ToString("0.00") + "s";
		} else {
            boxImage.enabled = false;
			multiplierDisplay.text = "";
		}
	}
}
