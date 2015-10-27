using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class spawnButtonInit : MonoBehaviour {

	int index;
	public GameObject Spawner;
    public GameObject[] oneOffSpawners;

	void Update () {
        if (GameControl.weaponManager.selectedWeapon != null) {
            bool enable = false;
            if (GameControl.weaponManager.selectedWeapon.type == WeaponType.AutoClicker ||
                (GameControl.weaponManager.selectedWeapon.type == WeaponType.PlayerWeapon &&
                 GameControl.weaponManager.selectedWeapon.count == 0)) {
                enable = GameControl.data.cash >= GameControl.weaponManager.selectedWeapon.cost;
            } else {
                enable = true;
            }

            GetComponent<Button>().interactable = enable;
        }
	}

	public void Clickything () {
        if (GameControl.weaponManager.selectedWeapon.type == WeaponType.AutoClicker ||
            (GameControl.weaponManager.selectedWeapon.type == WeaponType.PlayerWeapon &&
             GameControl.weaponManager.selectedWeapon.count == 0)) {
            GameControl.weaponManager.purchaseWeapon(GameControl.weaponManager.selectedWeapon.name);
        } else {
            foreach(GameObject spawner in oneOffSpawners) {
                if (spawner.GetComponent<OneOffWeaponScript>().weaponName == GameControl.weaponManager.selectedWeapon.name) {
                    spawner.GetComponent<OneOffWeaponScript>().ClickEvent();
                    clickme.profitBuffer += GameControl.weaponManager.selectedWeapon.damage;
                }
            }
        }
        if (GameControl.weaponManager.selectedWeapon.type == WeaponType.AutoClicker &&
            GameControl.weaponManager.selectedWeapon.count <= 10) {
            Spawner.GetComponent<clickme>().FormatWeaponLocations();
        }
    }
}
