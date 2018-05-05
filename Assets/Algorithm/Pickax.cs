using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickax : MonoBehaviour {
	bool isAnimatonDone;
	
	Animator animator;
	

	void Start() {
		animator = GetComponent<Animator>();
	}


	void OnGUI() {
		//GUILayout.TextField(animator.GetCurrentAnimatorStateInfo(0).normalizedTime.ToString());
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Block")) {
			Block blockComponent = other.GetComponent<Block>();

			float hp = (float)blockComponent.hp;


			if (hp - 1.3f <= 0) {
				int blockType = (int)blockComponent.blockType;

				if(DataController.Instance.saveable.item.ContainsKey(blockType) == false)
				DataController.Instance.saveable.item.Add(blockType,0);

				DataController.Instance.saveable.item[blockType]+=1;
				
				blockComponent.destroyEvent();
			}
			else {
				blockComponent.hp-=1.3f;
				blockComponent.executeEvent();
			}
		}
	}
	
	public void Mining(bool isMining) {
		animator.SetBool("isMining",isMining);
	}
}
