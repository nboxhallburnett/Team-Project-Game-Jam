using UnityEngine;
using System.Collections;

public class OneOffWeaponScript : MonoBehaviour {

	public string name;

	public void ClickEvent () {
		GetComponent<ParticleSystem>().Emit(1);
	}
}
