using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectWeaponScript : MonoBehaviour {

	public string weaponName;
	public bool selected = false;

	void Start () {
		GameControl.weaponManager.weaponButtons.Add(this);
	}

	public void SetSelected() {
		selected = true;
		GetComponent<Image>().sprite = GameControl.weaponManager.selectedButton;
	}

	public void SetDeselected() {
		selected = false;
		GetComponent<Image>().sprite = GameControl.weaponManager.deselectedButton;
	}

	public void clickEvent () {
		GameControl.weaponManager.setSelectedWeapon (weaponName);
	}
}
