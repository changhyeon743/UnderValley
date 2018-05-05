using UnityEngine;

public class MakeToFade : MonoBehaviour {

	/** 쉐이더 변수들은 인스펙터에서 지정, 이 스크립트는 건물때문에 카메라에 캐릭터가 안보이기 때문에 만들어짐. **/

	public GameObject target;

	private bool isAlpha = false;

	public Shader nonAlphaShader;
	public Shader AlphaShader;
	
	void OnTriggerEnter(Collider other) {
		SetAlpha(other.gameObject);
	}
	void OnTriggerExit(Collider other) {
		SetAlpha(other.gameObject);
	}

	void SetAlpha(GameObject other) {
		if (other.CompareTag("Player")) {
			MeshRenderer mesh = target.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();

			if (isAlpha) {
				mesh.material.shader = nonAlphaShader;
			}
			else {
				mesh.material.shader = AlphaShader;
				
				mesh.material.color = new Color(mesh.material.color.r,mesh.material.color.g,mesh.material.color.b,0.5f);
			}
			isAlpha=!isAlpha;
		}
	}

}
