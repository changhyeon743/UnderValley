using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CloudMessageController : MonoBehaviour {
	public Text textComponent;
	
	void Start() { StartCoroutine(MinusAlpha()); }

	IEnumerator MinusAlpha() {
		for(float a = 1; a > -0.1f;a-=0.05f) {
			transform.Translate(Vector2.up);
			textComponent.color = new Color(1,1,1,a);
			if (a <= 0) {
				Destroy(this.gameObject);
			}
			yield return 0;
		}
	}
}
