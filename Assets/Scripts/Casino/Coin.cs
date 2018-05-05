using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
	private Rigidbody rigidbodyComponent;
	public float ForceSpeed;
	public float TorqueSpeed;
	public float plusminus;

	public bool isGround = true;

	public static bool isUp;

	public CasinoController system;

	public bool isGaming = false;

	void Start() {
		rigidbodyComponent = GetComponent<Rigidbody>();
		int[] randomZ = new int[]{180,0};
		transform.rotation = Quaternion.Euler(0,0,randomZ[Random.Range(0,randomZ.Length)]);
	}
	
	

	void OnCollisionExit(Collision col) {
		isGround = false;
	}

	public void Roll() {
		if (isGround) {
			isGaming = true;
			rigidbodyComponent.AddForce(Vector3.up * Random.Range(ForceSpeed-plusminus,ForceSpeed+plusminus),ForceMode.VelocityChange);
			rigidbodyComponent.AddTorque(Random.onUnitSphere * TorqueSpeed,ForceMode.VelocityChange);
		}
	}
	
}
