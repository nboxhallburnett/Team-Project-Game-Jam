using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OneOffWeaponScript : MonoBehaviour {

	public string weaponName;
    public float timer;
    bool purchased;

    public GameObject[] TimerDisplay;

    void Start () {
        timer = 0.0f;
        foreach(weapon wep in GameControl.weaponManager.weapons) {
            if (wep != null && wep.name == weaponName) {
                if (wep.count > 0) {
                    purchased = true;
                }
            }
        }
    }

    void FixedUpdate () {
        if (timer > 0.0f) {
            timer -= Time.fixedDeltaTime;
            if (GameControl.weaponManager.selectedWeapon != null && GameControl.weaponManager.selectedWeapon.name == weaponName && purchased) {
                TimerDisplay[0].GetComponent<Text>().text = "Cooldown: " + (Mathf.Floor(timer) + 1).ToString();
                TimerDisplay[1].GetComponent<Text>().text = "Cooldown: " + (Mathf.Floor(timer) + 1).ToString();
            } else if (GameControl.weaponManager.selectedWeapon == null || 
                GameControl.weaponManager.selectedWeapon.type == WeaponType.AutoClicker || 
                (GameControl.weaponManager.selectedWeapon.name == weaponName && !purchased)) {
                TimerDisplay[0].GetComponent<Text>().text = "";
                TimerDisplay[1].GetComponent<Text>().text = "";
            }
        } else {
            timer = 0.0f;
            if (GameControl.weaponManager.selectedWeapon != null && GameControl.weaponManager.selectedWeapon.name == weaponName && purchased) {
                TimerDisplay[0].GetComponent<Text>().text = "Ready to fire!";
                TimerDisplay[1].GetComponent<Text>().text = "Ready to fire!";
            } else if (GameControl.weaponManager.selectedWeapon == null || 
                GameControl.weaponManager.selectedWeapon.type == WeaponType.AutoClicker ||
                (GameControl.weaponManager.selectedWeapon.name == weaponName && !purchased)) {
                TimerDisplay[0].GetComponent<Text>().text = "";
                TimerDisplay[1].GetComponent<Text>().text = "";
            }
        }
    }

	public void ClickEvent () {
        if (timer == 0.0f) {
            timer = 5.0f;
            GetComponent<ParticleSystem>().Emit(1);
            purchased = true;
            clickme.profitBuffer += GameControl.weaponManager.selectedWeapon.damage;
        }
    }
}
