using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class openExtendMenu : MonoBehaviour {
	[SerializeField]
	private bool isOpenMenu = false;

	public List<GameObject> buttons;
	public List<Sprite> OpenCloseImg;

	public void openCloseMenu() {
		float fixedY = 0;
		for(int i=0;i<5;i++) {
			fixedY += buttons[i].GetComponent<RectTransform>().sizeDelta.y;
		}
		for (int i=0;i<buttons.Count;i++) {
			buttons[i].SetActive(!isOpenMenu);
		}
		
		if (isOpenMenu) { //처음상태로 돌아감
			//GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, 0));
			//transform.localPosition = transform.localPosition + new Vector3(0,fixedY,0);
			GetComponent<Image>().sprite = OpenCloseImg[0];
		}

		if (!isOpenMenu) { //펼친상태로감
			//GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, 180));
			//transform.localPosition = transform.localPosition + new Vector3(0,-fixedY,0);
			GetComponent<Image>().sprite = OpenCloseImg[1];
		}

		isOpenMenu = !isOpenMenu;
		//transform.rotation = vector;
		
	}
}
