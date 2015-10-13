using UnityEngine;
using System.Collections;

enum WeaponType {
	AutoClicker,
	PlayerWeapon
}

/// <summary>
/// Handles weapon damage / cash generation
/// </summary>
public class WeaponManager : MonoBehaviour {

	weapon[] weapons = new weapon[30];

	public void Initialise (int[] count) {
		weapons = new weapon[weapons.Length];
		if (count == null) {
			count = new int[weapons.Length];
		}
		int index = 0;
		#region autoClickers
		weapons[index] = new weapon (WeaponType.AutoClicker, "Drone Bomber", 2.0f, 50.0f, count[index++]);
		weapons[index] = new weapon (WeaponType.AutoClicker, "Orbital Station", 10.0f, 300.0f, count[index++]);
		weapons[index] = new weapon (WeaponType.AutoClicker, "Solar Array Strike", 40.0f, 1200.0f, count[index++]);
		weapons[index] = new weapon (WeaponType.AutoClicker, "Low Mars Orbital Cannon", 260.0f, 10000.0f, count[index++]);
		weapons[index] = new weapon (WeaponType.AutoClicker, "Elon Star", 2048.0f, 900000.0f, count[index++]);
		#endregion

	}
	
	// Update is called on a fixed interval
	void FixedUpdate () {
		float cashBuffer = 0;
		foreach (weapon wep in weapons) {
			if (wep != null && wep.type == WeaponType.AutoClicker) {
				cashBuffer += wep.damage * wep.count;
			}
		}
		clickme.addMoney(cashBuffer);
	}

	public int[] GetWeaponCounts() {
		int[] wepCount = new int[weapons.Length];
		int index = 0;
		foreach (weapon wep in weapons) {
			if (wep != null) {
				wepCount[index++] = wep.count;
			}
		}
		return wepCount;
	}

}

/// <summary>
/// Weapon.
/// </summary>
class weapon {
	public WeaponType 	type	{ get; private set; }
	public string 		name 	{ get; private set; }
	public float 		damage 	{ get; private set; }
	public float 		cost 	{ get; private set; }
	public int			count	{ get; private set; }

	public weapon (WeaponType _type, string _name, float _damage, float _cost, int _count) {
		type 	= _type;
		name 	= _name;
		damage 	= _damage;
		cost 	= _cost;
		count 	= _count;
	}

	public bool purchasable () {
		return GameControl.data.cash - cost >= 0;
	}

	public bool purchase () {
		if (GameControl.data.cash - cost >= 0) {
			GameControl.data.cash -= cost;
			count++;
			return true;
		} else {
			return false;
		}
	}
}
