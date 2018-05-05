using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isCoinTrigger : MonoBehaviour {
	public bool isUp;
	public Coin coin;
	void OnTriggerEnter(Collider col) {
		Coin.isUp = isUp;
		if (!coin.isGround && coin.isGaming) {
			coin.isGaming = false;
			coin.system.GameIsEnd();
		}
		coin.isGround = true;
	}
}
