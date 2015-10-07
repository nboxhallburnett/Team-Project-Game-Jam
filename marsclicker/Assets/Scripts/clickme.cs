using UnityEngine;
using System.Collections;

public class clickme : MonoBehaviour {

	// Step 3:
	float profitBuffer;

	// Use this for initialization
	void Start () {
		//initialisation
		profitBuffer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)){
			RaycastHit hit = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//Transform select = GameObject.FindWithTag("mars").transform;
			if (Physics.Raycast (ray, out hit, 100.0f)){
                if(hit.collider.tag == "mars")
                {
                    profitBuffer += 1.0f;
                    GameControl.manager.UpdateTriggerCurrentValue("MOUSECLICKS", 1);
                }               
			}
		}
	}

	void FixedUpdate () {
        float adjustedProfit = calculateAdjustedProfit(profitBuffer);
        GameControl.data.cash += adjustedProfit;
        GameControl.data.score += adjustedProfit;
        GameControl.manager.UpdateTriggerCurrentValue("TOTALMONEYEARNED", adjustedProfit);
        profitBuffer = 0.0f;

		if (GameControl.data.multiplierTimer - Time.fixedDeltaTime < 0) {
			GameControl.data.multiplierTimer = 0.0f;
			GameControl.data.multiplier = 1.0f;
		} else {
			GameControl.data.multiplierTimer -= Time.fixedDeltaTime;
		}
	}

	float calculateAdjustedProfit (float toAdd) {
		return toAdd * GameControl.data.multiplier;
	}

	public void addMultiplier (float value) {
		if (value == 0.0f) {
			GameControl.data.multiplierTimer = 0.0f;
		} else if (GameControl.data.multiplierTimer == 0.0f) {
			GameControl.data.multiplierTimer = 10.0f;
		}

		if (value == 0.0f) {
			GameControl.data.multiplier = 1.0f;
		} else if (GameControl.data.multiplier == 1.0f) {
			GameControl.data.multiplier += (value - 1);
		} else {
			GameControl.data.multiplier += value;
		}
	}
}
