using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum HumanType { Head, Body, LeftArm, RightArm, LeftLeg, RightLeg }
public class PlayerController : MonoBehaviour {
	/** PlayerController는 조이스틱에서 전송된 Global.dir이나 Global.speed 같은 변수를 이용해 이동 **/
	public float jumpPower = 5f;
	public float rotateSpeed = 10f;
	Rigidbody rigidbodyComponent;
	Vector3 movement;
	public List<GameObject> humanObj;
	public List<Animator> animator;
	public bool isGround;
	private bool isRunning;
	

	void Start () {
		rigidbodyComponent = GetComponent<Rigidbody>();
		//DontDestroyOnLoad(this);
		Global.gameController.global.PlayerObj.GetComponent<PlayerController>().RefreshSkin();
	}

	void Update() {
		if(Input.GetButtonDown("Jump")) {
			rigidbodyComponent.AddForce(Vector3.up * jumpPower,ForceMode.Impulse);
		}
		if (Input.GetKeyDown(KeyCode.R)) {
			RefreshSkin();
		}
		
	}

	void OnCollisionStay(Collision col) {
		if (col.gameObject.CompareTag("Ground"))
			isGround = true;
	}
	void OnCollisionExit(Collision col) {
		if (col.gameObject.CompareTag("Ground"))
			isGround = false;
	}

	public void Jump() {
		if (isGround == true)
		rigidbodyComponent.AddForce(Vector3.up * jumpPower,ForceMode.Impulse);
	}

	public void RefreshCollider (GameObject parentObject)
	{
		BoxCollider bc = parentObject.GetComponent<BoxCollider>();
		Bounds bounds = new Bounds (Vector3.zero, Vector3.zero);
		bool hasBounds = false;
		
		Renderer[] renderers =  parentObject.GetComponentsInChildren<Renderer>();
		
		foreach (Renderer r in renderers) {
			if (r.tag == "Pickax")
			continue;

			if (!hasBounds) {
				bounds = r.bounds;
				hasBounds = true;
			}
			else if (hasBounds) {
				bounds.Encapsulate(r.bounds);
			}
		}

		bc.center = bounds.center - parentObject.transform.position;
		bc.size = bounds.size;
	}

	public void RefreshSkin() {
		int Data_HeadType = (int)DataController.Instance.saveable.head;

		XYZClass Data_XYZ;

		humanObj[(int)HumanType.Head].GetComponent<MeshRenderer>().material.mainTexture = DataController.Instance.playerhead[Data_HeadType].playerHeadInfo.texture;
		humanObj[(int)HumanType.Head].GetComponent<MeshFilter>().mesh = DataController.Instance.playerhead[Data_HeadType].playerHeadInfo.mesh;
		Data_XYZ = DataController.Instance.playerhead[Data_HeadType].playerHeadInfo.xyz;
		humanObj[(int)HumanType.Head].transform.localPosition = new Vector3(Data_XYZ.x, Data_XYZ.y, Data_XYZ.z);
		
		ChangeBodySkin(HumanType.Body);
		ChangeBodySkin(HumanType.LeftArm);
		ChangeBodySkin(HumanType.RightArm);
		ChangeBodySkin(HumanType.LeftLeg);
		ChangeBodySkin(HumanType.RightLeg);

		RefreshCollider(this.gameObject);
	}

	void ChangeBodySkin(HumanType type) {
		int Data_BodyType = (int)DataController.Instance.saveable.body;
		humanObj[(int)type].GetComponent<MeshRenderer>().material.mainTexture = DataController.Instance.playerbody[Data_BodyType].playerBodyInfo[(int)type-1].texture;
		humanObj[(int)type].GetComponent<MeshFilter>().mesh = DataController.Instance.playerbody[Data_BodyType].playerBodyInfo[(int)type-1].mesh;
		var Data_XYZ = DataController.Instance.playerbody[Data_BodyType].playerBodyInfo[(int)type-1].xyz;
		humanObj[(int)type].transform.localPosition = new Vector3(Data_XYZ.x,Data_XYZ.y,Data_XYZ.z);
		humanObj[(int)type].gameObject.SetActive(DataController.Instance.playerbody[Data_BodyType].playerBodyInfo[(int)type-1].isExists);
	}

	void Turn() {
		if (Global.gameController.global.speed == 0)
		return;

		Quaternion newRotation = Quaternion.LookRotation(-movement);
		rigidbodyComponent.rotation = Quaternion.Slerp(rigidbodyComponent.rotation, newRotation, rotateSpeed * Time.deltaTime);
	}

	void FixedUpdate () {
		Run();
		Turn();
	}

	void Run() {
		if (Global.gameController.global.speed == 0) {
			if (isRunning == false)
				foreach(var Data_animator in animator) {
					if (Data_animator.transform.gameObject.activeSelf == true)
						Data_animator.SetBool("isRunning",false);
				}
			
			isRunning = true;

			return;
		}

		if (isRunning == true)
			foreach(var Data_animator in animator)
				if (Data_animator.transform.gameObject.activeSelf == true)
					Data_animator.SetBool("isRunning",true);
		
		isRunning = false;
		
		movement = normalizeDir();
		movement = RotateWithView();
		// 5 * (Global.gameController.global.speed / 112.5f)
		movement = movement.normalized * (float)(5 * (Global.gameController.global.speed / 112.5f) * (1 - ((100 - DataController.Instance.saveable.hungry) *0.01f * 0.25f) - (DataController.Instance.saveable.tired * 0.01f *0.35f))) * Time.deltaTime;

		transform.position = transform.position + movement;
	}

	private Vector3 RotateWithView() {
		Vector3 dir = Camera.main.transform.TransformDirection (movement);
		dir.Set(dir.x,0,dir.z);
		return dir.normalized * movement.magnitude;
	}

	private Vector3 normalizeDir() {
		Vector3 dir = Vector3.zero;

		dir.x = Global.gameController.global.dir.x;
		dir.z = Global.gameController.global.dir.y;

		if (dir.magnitude > 1)
		dir.Normalize();

		return dir;
	}

}