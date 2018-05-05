using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour {
	Rigidbody rigidbodyComponent;
	float scale;

	void Start () {
		rigidbodyComponent = GetComponent<Rigidbody>();
		StartCoroutine(Jump());
	}

	void Update() {
		scale = 0.25f+(0.75f/9999)*DataController.Instance.saveable.SlimeWeight * 100f - 0.01f;
		transform.localScale = new Vector3(scale,scale,scale);
	}
	
	IEnumerator Jump() {
		while(true) {
			yield return new WaitForSeconds(5f);
			float jumpPower = Random.Range(3f-scale/100,5f-scale/100);
			int r = Random.Range(0,5+1);

			if (r<=3)
			rigidbodyComponent.AddForce(Vector3.up * jumpPower,ForceMode.Impulse);
		}
	}
}