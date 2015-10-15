using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class spawnButtonInit : MonoBehaviour {

	public string weaponName;
	int index;
	weapon data;

	// Use this for initialization
	void Start () {
		foreach (weapon wep in GameControl.weaponManager.weapons) {
			if (wep != null && wep.name == weaponName) {
				data = wep;
				Debug.Log (wep.name);
			}
		}

		GetComponentInChildren<Text>().text = weaponName;
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Button>().interactable = data.cost <= GameControl.data.cash;
	}

	public void clickEvent () {
		GameControl.weaponManager.purchaseWeapon (weaponName);
	}
}
