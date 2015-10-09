﻿using UnityEngine;
using System.Collections;

public class clickme : MonoBehaviour {

	// Step 3:
	static float profitBuffer;

	// Use this for initialisation
	void Start () {
		//initialisation
		profitBuffer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)){
			RaycastHit hit = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 100.0f)){
                if (hit.collider.tag == "mars") {
                    addMoney(1.0f);
                }
			}
		}
	}

	void FixedUpdate () {
		GameControl.data.cash += calculateAdjustedProfit(profitBuffer);
		GameControl.data.score += calculateAdjustedProfit(profitBuffer);
		profitBuffer = 0.0f;

		if (GameControl.data.multiplierTimer - Time.fixedDeltaTime < 0) {
			GameControl.data.multiplierTimer = 0.0f;
			GameControl.data.multiplier = 1.0f;
		} else {
			GameControl.data.multiplierTimer -= Time.fixedDeltaTime;
		}
	}

    /// <summary>
    /// Adjusts the input value by the current value of the multiplier
    /// </summary>
    /// <param name="toAdd">Value to adjust by the multiplier</param>
    /// <returns></returns>
	float calculateAdjustedProfit (float toAdd) {
		return toAdd * GameControl.data.multiplier;
	}

    /// <summary>
    /// Adds a multiplier to the money being collected.
    /// Use a value of 0 to remove the active multiplier.
    /// </summary>
    /// <param name="value">Magnitude of the multiplier to add</param>
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

    /// <summary>
    /// Add money to the players balance, pre-multiplier
    /// </summary>
    /// <param name="amount">Base value of money to add to the players balance</param>
    public static void addMoney (float amount) {
        if (amount > 0) {
            profitBuffer += amount;
        }
    }
}
