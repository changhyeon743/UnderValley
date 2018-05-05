using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHP : MonoBehaviour {

	public void RefreshBlockHP() {
		Block blockComponent = transform.parent.parent.GetComponent<Block>();
		GetComponent<RectTransform>().localScale = new Vector3(blockComponent.hp / blockComponent.maxHP,1,1);
	}
}
