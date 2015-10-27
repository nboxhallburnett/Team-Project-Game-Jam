using UnityEngine;
using System.Collections;

public class OneOffWeaponScript : MonoBehaviour {

	public string weaponName;
    public float timer;

    void Start () {
        timer = 0.0f;
    }

	public void ClickEvent () {
		GetComponent<ParticleSystem>().Emit(1);
	}
}
