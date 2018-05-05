using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class menu {
	public string name;
	public Transform transform;
	public bool isDirectionalLight;

	[RangeAttribute(0f,1f)]
	public float Intensity;
}

public class PortalController : MonoBehaviour {
	/** menu 클래스안에 담겨있는 name, transform 변수는 인스펙터에서 지정, 이동될땐 햇빛을 끄고, isFade변수를 이용해 불필요한 연산 안함. clickedButton변수엔 몇번째 버튼을 눌렀는지 담김. **/
	/** 체크가 끝나면 변수안에 있는 값 초기화, menuList의 크기가 2냐 3이냐에 따라서 코드가 두방향으로 갈림. **/

	private bool isFade;


	private GameObject Player;

	public menu[] menuList;

	private int clickedButton;

	void Start() {
		Player = GameObject.FindWithTag("Player");
	}
	
	void Update() {
		if (isFade) {
			if (Global.gameController.global.fadeInOutObj.GetComponent<Image>().color.a >= 1f) {

				Global.gameController.StartCoroutine("FadeOut");

				switch(menuList[clickedButton].name) {
					case "채굴 회사": Global.gameController.global.Pickax.SetActive(true); break;
					case "나가기":
						if (DataController.Instance.saveable.currentContent == "채굴 회사") 
						Global.gameController.global.Pickax.SetActive(false);
					break;
				}

				DataController.Instance.saveable.currentContent = menuList[clickedButton].name;

				Player.transform.position = menuList[clickedButton].transform.position;
				DataController.Instance.saveable.LightIntensity = menuList[clickedButton].Intensity;
				DataController.Instance.saveable.DirectionalLight = menuList[clickedButton].isDirectionalLight;
				
				isFade = false;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {

			int menuLength = menuList.Length;
			Global.gameController.global.MenuButtonsObj[menuLength-1].SetActive(true);

			for(int i=0;i < menuLength;i++) {
				Global.gameController.global.MenuButtonsObj[menuLength-1].transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = menuList[i].name;
			}
			
			Global.gameController.global.currentPortal = this.gameObject;
			//SceneManager.LoadScene("Buildings");

		}
	}



	void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			int menuLength = menuList.Length;
			Global.gameController.global.MenuButtonsObj[menuLength-1].SetActive(false);
			Global.gameController.global.currentPortal = null;
		}
	}



	//MenuController 에서 사용
	public void MoveTransform(int Num) {
		Global.gameController.global.fadeInOutObj.SetActive(true);
		Global.gameController.StartCoroutine("FadeIn");
		isFade = true;
		clickedButton = Num;
		for(int i=0;i<Global.gameController.global.ButtonSize;i++) {
			if (Global.gameController.global.MenuButtonsObj[i].activeSelf) {
				Global.gameController.global.MenuButtonsObj[i].SetActive(false);
				return;
			}
		}
	}


	
}
