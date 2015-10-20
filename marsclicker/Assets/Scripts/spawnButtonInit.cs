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
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Button>().interactable = data.cost <= GameControl.data.cash;
        GetComponentInChildren<Text>().text = "$" + data.cost + " \t " + data.damage + "DPS\n" + weaponName + "\n(" + data.count + "x)";
    }

	public void ClickEvent () {
        GameControl.weaponManager.purchaseWeapon(weaponName);
	}
}
