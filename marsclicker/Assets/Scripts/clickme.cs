using UnityEngine;
using System.Collections;

public class clickme : MonoBehaviour {

	// Step 3:
	static float profitBuffer;

	float screenShakeTimer = 0.0f;
	public GameObject planetImage;
	Vector3 defaultPos,
			defaultScale;

	// Use this for initialisation
	void Start () {
		//initialisation
		profitBuffer = 0.0f;

        int totalWepCount = 0,
            weaponsSpawned = 0;
        float weaponDistance = 0.0f;
        // Get the total weapon count so we can evenly space out the satellites on spawn
        foreach(weapon wep in GameControl.weaponManager.weapons) {
            if (wep != null) {
                totalWepCount += wep.count;
            }
        }
        weaponDistance = 360.0f / (float)totalWepCount;
        foreach(weapon wep in GameControl.weaponManager.weapons) {
            if (wep != null) {
                for (int i = 0; i <= wep.count - 1; i++) {
                    GameObject newObj = (GameObject)Instantiate(Resources.Load("Prefabs/Weapons/" + wep.name));
                    Vector3 scale = newObj.transform.localScale;
                    newObj.transform.parent = transform.parent.transform.parent.transform;
                    newObj.transform.localScale = scale;
                    newObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, (weaponDistance * weaponsSpawned - 90)));
                    newObj.transform.localPosition = pointOnCircle(weaponDistance * weaponsSpawned++, newObj.GetComponent<satellite>().radius);
                }
            }
        }
		
		defaultPos = planetImage.transform.localPosition;
		defaultScale = planetImage.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)){
			RaycastHit hit = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 100.0f)){
                if (hit.collider.tag == "mars") {
                    addMoney(1.0f);
                    GameControl.achievementManager.UpdateTriggerCurrentValue("MOUSECLICKS", 1);
					screenShakeTimer = 0.15f;
                }
			}
		}
		screenShakeUpdate();

	}

	void FixedUpdate () {
        float adjustedProfit = calculateAdjustedProfit(profitBuffer);
        GameControl.data.cash += adjustedProfit;
        GameControl.data.score += adjustedProfit;
        GameControl.achievementManager.UpdateTriggerCurrentValue("TOTALMONEYEARNED", adjustedProfit);
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
	public static void addMultiplier (float value) {
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

	public void spawnObject(string objectRoot) {
		GameObject newObj = (GameObject)Instantiate(Resources.Load(objectRoot));
		Vector3 scale = newObj.transform.localScale;
		newObj.transform.parent = transform.parent.transform.parent.transform;
		newObj.transform.localScale = scale;
		newObj.transform.localPosition = new Vector3(0.0f, 1.0f, 0.0f);
	}

	void screenShakeUpdate() {
		if (screenShakeTimer <= 0.0f) {
			screenShakeTimer = 0.0f;
			planetImage.transform.localPosition = defaultPos;
			planetImage.transform.localScale = defaultScale;
			return;
		} else {
			screenShakeTimer -= Time.deltaTime;
			planetImage.transform.localPosition = new Vector3 (defaultPos.x + Random.Range (-0.01f, 0.01f), defaultPos.y + Random.Range (-0.01f, 0.01f), defaultPos.z);
			planetImage.transform.localScale = new Vector3 (defaultScale.x + Random.Range (-0.1f, 0.0f), defaultScale.y + Random.Range (-0.1f, 0.0f), defaultScale.z);
		}
	}

    /// <summary>
    /// Returns the position vector for a satellite to be spawned at on load
    /// </summary>
    /// <param name="angle">Angle in Degrees that the satellite should be positioned at (360 / total count)</param>
    /// <param name="radius">Radius of the circle the satellites are orbiting at</param>
    /// <returns>Position Vector3 for the satellite</returns>
    Vector3 pointOnCircle(float angle, float radius) {
        float x = (float)(radius * Mathf.Cos(angle * Mathf.PI / 180.0f));
        float y = (float)(radius * Mathf.Sin(angle * Mathf.PI / 180.0f));

        return new Vector3(x, y, 0.0f);
    }

}
