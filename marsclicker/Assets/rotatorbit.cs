using UnityEngine;
using System.Collections;

public class rotatorbit : MonoBehaviour {
	public float rotY = 0f;
	public float rotX = 0f;
	public float rotZ = 0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.rotation = Quaternion.AngleAxis(rotY--, new Vector3(-0.4f, 1, 0));

	}
}
