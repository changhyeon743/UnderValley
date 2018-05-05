using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	public Items blockType;
	private MeshRenderer meshRenderer;
	private MeshFilter meshFilter;
	private BoxCollider boxCollider;

	public List<BlockCostume> costume;

	public float maxHP;
	public float hp;
	public float reward;

	private GameObject canvasObject;

	void Start() {
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();
		boxCollider = GetComponent<BoxCollider>();
		canvasObject = transform.GetChild(0).gameObject;
		StartCoroutine(Refresh());
	}

	IEnumerator Refresh() {
		yield return new WaitForSeconds(0.2f);
		foreach(var a in costume) {
			if (blockType == Items.none) {
				boxCollider.enabled = false;
			}

			if (blockType == a.itemType) {
				meshFilter.mesh = a.mesh;
				meshRenderer.material.mainTexture = a.texture;
				reward = a.reward;
				hp = a.hp;
				maxHP = hp;
			}

		}
	}

	public void executeEvent() {
		canvasObject.SetActive(true);
		canvasObject.transform.GetChild(1).GetComponent<BlockHP>().RefreshBlockHP();
	}

	public void destroyEvent() {
		DataController.Instance.saveable.money+=(int)reward;
		Destroy(this.gameObject);
	}

}
