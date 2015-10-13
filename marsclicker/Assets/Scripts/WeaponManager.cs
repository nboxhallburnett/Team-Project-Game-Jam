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

	weapon[] weapons { get; private set; }

	public void Initialise (int[] count) {
		weapons = new weapon[30];
		if (count == null) {
			count = new int[weapons.Length];
		}
		int index = 0;
		#region autoClickers
		//							Weapon Type				Name						Damage		Cost		Index
		weapons[index] = new weapon(WeaponType.AutoClicker, "Drone Bomber", 			2.0f, 		50.0f, 		count[index++]);
		weapons[index] = new weapon(WeaponType.AutoClicker, "Orbital Cannon", 			10.0f, 		300.0f, 	count[index++]);
		weapons[index] = new weapon(WeaponType.AutoClicker, "Solar Array Strike", 		40.0f, 		1200.0f, 	count[index++]);
		weapons[index] = new weapon(WeaponType.AutoClicker, "Low Mars Orbital Cannon", 	260.0f, 	10000.0f, 	count[index++]);
		weapons[index] = new weapon(WeaponType.AutoClicker, "Elon Star", 				2048.0f, 	900000.0f, 	count[index++]);
		#endregion
		#region playerWeapons
		weapons[index] = new weapon(WeaponType.PlayerWeapon, "Falcon X Parts", 			1.0f, 		0.0f, 		count[index++]);
		weapons[index] = new weapon(WeaponType.PlayerWeapon, "Tesla Cars", 				8.0f, 		70.0f, 		count[index++]);
		weapons[index] = new weapon(WeaponType.PlayerWeapon, "PowerWall Battery Bomb", 	16.0f, 		200.0f, 	count[index++]);
		weapons[index] = new weapon(WeaponType.PlayerWeapon, "SpaceX Rockets", 			32.0f, 		1000.0f, 	count[index++]);
		weapons[index] = new weapon(WeaponType.PlayerWeapon, "Nuclear Bombs", 			64.0f,		5000.0f, 	count[index++]);
		weapons[index] = new weapon(WeaponType.PlayerWeapon, "Fusion Bombs", 			128.0f, 	15000.0f, 	count[index++]);
		weapons[index] = new weapon(WeaponType.PlayerWeapon, "Asteroid Mining Strike", 	256.0f, 	25000.0f, 	count[index++]);
		weapons[index] = new weapon(WeaponType.PlayerWeapon, "Space Ripper", 			512.0f, 	40000.0f, 	count[index++]);
		weapons[index] = new weapon(WeaponType.PlayerWeapon, "Black Hole Detonator", 	1024.0f, 	300000.0f, 	count[index++]);
		#endregion

	}
	
	// Update is called on a fixed interval
	void FixedUpdate () {
		float cashBuffer = 0;
		foreach (weapon wep in weapons) {
			if (wep != null && wep.type == WeaponType.AutoClicker) {
				cashBuffer += (wep.damage * wep.count) * Time.fixedDeltaTime;
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
		if (type == WeaponType.AutoClicker) {
			return GameControl.data.cash - cost >= 0;
		} else {
			return count == 0
		}
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
