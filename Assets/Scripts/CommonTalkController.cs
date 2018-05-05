
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonTalkController : MonoBehaviour {

	/** CommonTalkController는 가장 일반적인 대화를 컨트롤함. **/
	/** CommonTalkController는 대화창에 들어가는 스크립트로, 인스펙터에서 한번 지정해주고나서 별다른 수정은 없을예정. **/

	[SerializeField]
	private int page = 0;
	[SerializeField]
	private int maxPage = 0;

	public string stringName;

	public List<string> TextList = new List<string>();

	public Text txtComponent;
	public Text nametxtComponent;
	public GameObject nameObj;
	
	public void gotoNextPage() {
		if (page < maxPage) {
			page++;
			txtComponent.text = TextList[page];
			return;
		}

		if (page >= maxPage) {
			TextList = new List<string>();
			page = 0;
			maxPage = 0;
			Global.gameController.global.JumpBtnObj.SetActive(true);
			Global.gameController.global.MineBtnObj.SetActive(true);
			this.gameObject.SetActive(false);
		}
	}

	public void ResetPage() {
		page = -1;
		maxPage = TextList.Count-1;
		gotoNextPage();
		nametxtComponent.text = stringName;
		nameObj.GetComponent<RectTransform>().sizeDelta = new Vector2(nametxtComponent.preferredWidth+48,64);
	}

	
}
