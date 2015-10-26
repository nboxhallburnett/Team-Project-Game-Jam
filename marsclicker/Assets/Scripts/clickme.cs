using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class clickme : MonoBehaviour {

	// Step 3:
	static float profitBuffer;

	float screenShakeTimer = 0.0f;
    public GameObject planetImage;
    public GameObject kawaiiPlanet;
    public GameObject[] planets;
    public ParticleSystem dollarEmitter;
    
	Vector3 defaultPos = new Vector3(0, 0, 0),
			defaultScale = new Vector3(1, 1, 1);

	// Use this for initialisation
	void Start ()
    {
        //initialisation
        profitBuffer = 0.0f;
        FormatWeaponLocations();

        // Make sure the Kawaii face is the only one visible to start
        foreach (GameObject planet in planets)
        {
            planet.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void FormatWeaponLocations()
    {
        bool reloading = false;
        foreach (GameObject oldWep in GameObject.FindGameObjectsWithTag("Satellite"))
        {
            Destroy(oldWep);
            reloading = true;
        }
       
        int totalWepCount = 0, weaponsSpawned = 0;
        float weaponDistance = 0.0f;
        // Get the total weapon count so we can evenly space out the satellites on spawn
        foreach (weapon wep in GameControl.weaponManager.weapons)
        {
            if (wep != null)
            {
                totalWepCount += (wep.count < 10 ? wep.count : 10);
            }
        }
        weaponDistance = 360.0f / (float)totalWepCount;
        foreach (weapon wep in GameControl.weaponManager.weapons)
        {
            int count = 0;
            if (wep != null)
            {
                for (int i = 0; i <= wep.count - 1; i++)
                {
                    if (!reloading)
                    {
                        GameControl.weaponManager.increaseCost(wep.name);
                    }
                    if (++count <= 10)
                    {
                        GameObject newObj = (GameObject)Instantiate(Resources.Load("Prefabs/Weapons/" + wep.name));
                        Vector3 scale = newObj.transform.localScale;
                        newObj.transform.parent = transform.parent.transform.parent.transform;
                        newObj.transform.localScale = scale;
                        newObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, (weaponDistance * weaponsSpawned - 90)));
                        newObj.transform.localPosition = PointOnCircle(weaponDistance * weaponsSpawned++, newObj.GetComponent<satellite>().radius);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetMouseButtonDown(0)){
			RaycastHit hit = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 100.0f)){
                if (hit.collider.tag == "mars") {
                    AddMoney(1.0f);
                    GameControl.achievementManager.UpdateTriggerCurrentValue("MOUSECLICKS", 1);
                    if(dollarEmitter != null)
                    {
                        float multiplier = GameControl.data.multiplier;
                        if(multiplier > 0)
                        {
                            dollarEmitter.Emit(2 * (int)multiplier);
                        }
                        else
                        {
                            dollarEmitter.Emit(2);
                        }
                        
                    }
					screenShakeTimer = 0.15f;

                    // Add a 1 in 10 chance for the planet to change face
                    if (Random.Range(0, 10) > 8) {
                        SwitchPlanet();
                    }
                }
			}
		}
		ScreenShakeUpdate();

	}

	void FixedUpdate () {
        float adjustedProfit = CalculateAdjustedProfit(profitBuffer);
        GameControl.data.cash += adjustedProfit;
        GameControl.data.score += adjustedProfit;
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
	float CalculateAdjustedProfit (float toAdd) {
		return toAdd * GameControl.data.multiplier;
	}

    /// <summary>
    /// Adds a multiplier to the money being collected.
    /// Use a value of 0 to remove the active multiplier.
    /// </summary>
    /// <param name="value">Magnitude of the multiplier to add</param>
	public static void AddMultiplier (float value) {
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
    public static void AddMoney (float amount) {
        if (amount > 0) {
            profitBuffer += amount;
        }
    }

	public void SpawnObject(string objectRoot) {
		GameObject newObj = (GameObject)Instantiate(Resources.Load(objectRoot));
		Vector3 scale = newObj.transform.localScale;
		newObj.transform.parent = transform.parent.transform.parent.transform;
		newObj.transform.localScale = scale;
		newObj.transform.localPosition = new Vector3(0.0f, 1.0f, 0.0f);
	}

	void ScreenShakeUpdate() {
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
    Vector3 PointOnCircle(float angle, float radius) {
        float x = (float)(radius * Mathf.Cos(angle * Mathf.PI / 180.0f));
        float y = (float)(radius * Mathf.Sin(angle * Mathf.PI / 180.0f));

        return new Vector3(x, y, 0.0f);
    }

    /// <summary>
    /// Selects a random planet face to use and sets that one as visible, hiding the rest
    /// </summary>
    void SwitchPlanet() {
        foreach(GameObject planet in planets) {
            planet.GetComponent<SpriteRenderer>().enabled = false;
        }
        kawaiiPlanet.GetComponent<SpriteRenderer>().enabled = false;
        int active = Random.Range(0, planets.Length - 1);
        planets[active].GetComponent<SpriteRenderer>().enabled = true;
    }

}
