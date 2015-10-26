using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum WeaponType {
    AutoClicker,
    PlayerWeapon
}

/// <summary>
/// Handles weapon damage / cash generation
/// </summary>
public class WeaponManager : MonoBehaviour {

    public weapon[] weapons { get; private set; }
    public List<SelectWeaponScript> weaponButtons;
    public weapon selectedWeapon;
    public Sprite deselectedButton;
    public Sprite selectedButton;

    public GameObject[] OneOffWeapons;

    public void Initialise (int[] count) {
        weapons = new weapon[30];
        if (count == null) {
            count = new int[weapons.Length];
        }
        int index = 0;
        #region autoClickers
        //                          Weapon Type             Name                Damage      Cost        Index
        weapons[index] = new weapon(WeaponType.AutoClicker, "Drone Bomber",     2.0f,       50.0f,      count[index++]);
        weapons[index] = new weapon(WeaponType.AutoClicker, "Orbital Cannon",   16.0f,      300.0f,     count[index++]);
        weapons[index] = new weapon(WeaponType.AutoClicker, "Solar Array",      128.0f,     1500.0f,    count[index++]);
        weapons[index] = new weapon(WeaponType.AutoClicker, "Elon Star",        2048.0f,    900000.0f,  count[index++]);
        #endregion
        #region oneOffWeapons
        weapons[index] = new weapon(WeaponType.PlayerWeapon, "Edison Car",      8.0f,       50.0f,      count[index++]);
        weapons[index] = new weapon(WeaponType.PlayerWeapon, "SpaceY Rocket",   64.0f,      200.0f,     count[index++]);
        weapons[index] = new weapon(WeaponType.PlayerWeapon, "Asteroid Strike", 128.0f,     1000.0f,    count[index++]);
        weapons[index] = new weapon(WeaponType.PlayerWeapon, "Nuclear Bomb",    256.0f,     5000.0f,    count[index++]);
        weapons[index] = new weapon(WeaponType.PlayerWeapon, "Fusion Bomb",     1024.0f,    25000.0f,   count[index++]);
        weapons[index] = new weapon(WeaponType.PlayerWeapon, "Space Ripper",    2048.0f,    100000.0f,  count[index++]);
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
        clickme.AddMoney(cashBuffer);

        foreach (GameObject laser in GameObject.FindGameObjectsWithTag("laser")) {
            satellite l = laser.transform.parent.gameObject.GetComponent<satellite>();
            if (l.laserTimer < 0.8f) {
                laser.GetComponent<SpriteRenderer>().enabled = true;
            } else {
                laser.GetComponent<SpriteRenderer>().enabled = false;
            }

            if (l.laserTimer > 0.3f) {
                l.laserTimer -= Time.fixedDeltaTime;
            } else {
                l.laserTimer = Random.Range(0.3f, 3.0f);
            }
        }
    }

    public void setSelectedWeapon (string name) {
        foreach (weapon wep in weapons) {
            if (wep != null && wep.name == name) {
                if (selectedWeapon == wep) {
                    selectedWeapon = null;
                } else {
                    selectedWeapon = wep;
                }
            }
        }
        foreach (SelectWeaponScript button in weaponButtons) {
            if (button.weaponName == name && selectedWeapon != null) {
                button.SetSelected();
            } else {
                button.SetDeselected();
            }
        }
    }

    public void increaseCost (string name) {
        foreach (weapon wep in weapons) {
            if (wep != null && wep.name == name) {
                wep.updateCost(wep.cost + (wep.cost * 0.1f));
            }
        }
    }

    public int[] GetWeaponCounts () {
        int[] wepCount = new int[weapons.Length];
        int index = 0;
        foreach (weapon wep in weapons) {
            if (wep != null) {
                wepCount[index++] = wep.count;
            }
        }
        return wepCount;
    }

    public void purchaseWeapon (string name) {
        foreach (weapon wep in weapons) {
            if (wep != null && wep.name == name) {
                if (wep.purchase()) {
                    increaseCost(name);
                }
            }
        }
    }
}

/// <summary>
/// Weapon.
/// </summary>
public class weapon {
    public WeaponType type { get; private set; }
    public string name { get; private set; }
    public float damage { get; private set; }
    public float cost { get; private set; }
    public int count { get; private set; }

    public weapon (WeaponType _type, string _name, float _damage, float _cost, int _count) {
        type = _type;
        name = _name;
        damage = _damage;
        cost = _cost;
        count = _count;
    }

    public void updateCost (float newCost) {
        cost = newCost;
    }

    public bool purchase () {
        if (GameControl.data.cash - cost >= 0) {
            GameControl.data.cash -= cost;
            count++;
            GameControl.data.Save();
            return true;
        } else {
            return false;
        }
    }
}
