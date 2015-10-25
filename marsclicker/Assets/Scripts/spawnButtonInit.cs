using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class spawnButtonInit : MonoBehaviour {

	int index;
	public GameObject Spawner;

	void Update () {
        if (GameControl.weaponManager.selectedWeapon != null) {
            GetComponent<Button>().interactable = GameControl.data.cash >= GameControl.weaponManager.selectedWeapon.cost;
        }
	}

	public void Clickything () {
		GameControl.weaponManager.purchaseWeapon(GameControl.weaponManager.selectedWeapon.name);
        Spawner.GetComponent<clickme>().FormatWeaponLocations();
    }
}
