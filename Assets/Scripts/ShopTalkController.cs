using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum CompanyRank {
	///인턴
	intern,
	///사원
	employee,
	///대리
	assistantManager,
	///과장
	Manager,
	///부장
	GeneralManager
}


public enum Contents {
	Bank, Company, School, FurnitureStore, Store, FlowerStore, Casino, ClothingStore, Salon, Train, Cashshop, Hospital, Bed, Spa, MagmaSlime
}

//public enum ShopUIEffectImage { None, Smart , Hungry, Tired }

[System.Serializable]
public class EffectImageAndText {
	public Sprite EffectImage;
	public string effect;
}

[System.Serializable]
public struct ShopUI {
	public Contents contents;
    public string name;
    public string explain;
    public int time;
    public List<EffectImageAndText> effect;
    public List<string> buttonName;
}

[System.SerializableAttribute]
public struct EffectClass {
	public Image effectImageComponent;
	public Text effecttxtComponent;
}

[System.SerializableAttribute]
public class Buttons {
	public Image itemImageComponent;
	public Text nametxtComponent;
	public Text explaintxtComponent;
	public List<EffectClass> effectComponent;
	public Text timetxtComponent;
	public List<GameObject> buttonObj = new List<GameObject>();
}

public class ShopTalkController : MonoBehaviour {

	struct shopButtonInfoClass {
		public int Bigbtnnum;
		public int clickedbutton;
		public int i;
		public string name;
	}

	/** ShopTalkController는 상업적 대화를 컨트롤함. **/

	[SerializeField]
	private int page = 0;
	[SerializeField]
	private int maxPage = 0;

	public List<ShopUI> ShopUIInfo = new List<ShopUI>();

	public List<Buttons> ButtonsComponent = new List<Buttons>();

	public GameObject PreviousButtonObj;
	public GameObject NextButtonObj;

	private bool isFade = false;

	private shopButtonInfoClass shopButtonInfo = new shopButtonInfoClass();
	public List<Sprite> shopUIEffectImage;

	
	void Update() {
		if (isFade) {
			if (Global.gameController.global.fadeInOutObj.GetComponent<Image>().color.a >= 1f) {
				Global.gameController.StartCoroutine("FadeOut");
				
				RealEvent(shopButtonInfo.name,shopButtonInfo.i,shopButtonInfo.clickedbutton,true);

				isFade = false;
			}
		}
	}

	public void gotoNextPage() {
		if (page < maxPage) {
			page++;

			setAllText();
		}

		RefreshMoveButtons();
	}


	public void gotoPreviousPage() {
		if (page > 0) {
			page--;

			setAllText();
		}
		
		RefreshMoveButtons();
	}

	
	void SetText(int btnnum) {
		///첫번째페이지에서의 0,1 두번째 페이지에서의 3,4 반환
		int i = btnnum+page*2;
		
		if (i >= ShopUIInfo.Count) {
			FindButton(btnnum).SetActive(false);
			return;
		}
		else
		FindButton(btnnum).SetActive(true);

		ButtonsComponent[btnnum].nametxtComponent.text = ShopUIInfo[i].name;
		ButtonsComponent[btnnum].explaintxtComponent.text = ShopUIInfo[i].explain;

		for(var b=0;b<ButtonsComponent[btnnum].effectComponent.Count;b++) {
			ButtonsComponent[btnnum].effectComponent[b].effecttxtComponent.enabled = false;
			ButtonsComponent[btnnum].effectComponent[b].effectImageComponent.enabled = false;
		}
		for(int a=0;a<ShopUIInfo[i].effect.Count;a++) {
			ButtonsComponent[btnnum].effectComponent[a].effecttxtComponent.enabled = true;
			if (ShopUIInfo[i].effect[a].EffectImage != null) {
				ButtonsComponent[btnnum].effectComponent[a].effectImageComponent.enabled = true;
				ButtonsComponent[btnnum].effectComponent[a].effectImageComponent.sprite = ShopUIInfo[i].effect[a].EffectImage;
			}
			ButtonsComponent[btnnum].effectComponent[a].effecttxtComponent.text = ShopUIInfo[i].effect[a].effect;
		}

		ButtonsComponent[btnnum].timetxtComponent.text = ShopUIInfo[i].time+"시간";

		if (ShopUIInfo[i].buttonName.Count < 4) {
			for(int g=ShopUIInfo[i].buttonName.Count;g<4;g++) {
				ButtonsComponent[btnnum].buttonObj[g].SetActive(false);
			}
		}
		else {
			for(int g=0;g<4;g++) {
				ButtonsComponent[btnnum].buttonObj[g].SetActive(true);
			}
		}

		for(int l=0;l<ShopUIInfo[i].buttonName.Count;l++) {
			ButtonsComponent[btnnum].buttonObj[l].transform.GetChild(0).GetComponent<Text>().text = ShopUIInfo[i].buttonName[l];
		}


		switch(ShopUIInfo[i].contents) {
			case Contents.Bank: ButtonsComponent[btnnum].explaintxtComponent.text = "현재잔고: "+DataController.Instance.saveable.bankMoney; break;
			case Contents.Company:
				if (ShopUIInfo[i].name == "근무") {
					int money = 0;
					int tired = 0;
					int hungry = 0;
					switch(DataController.Instance.saveable.companyRank) {
						case CompanyRank.intern: money = 30; tired = 28; hungry = 12; break;
						case CompanyRank.employee: money = 37; tired = 32; hungry = 12; break;
						case CompanyRank.assistantManager: money = 45; tired = 36; hungry = 12; break;
						case CompanyRank.Manager: money = 55; tired = 40; hungry = 12; break;
						case CompanyRank.GeneralManager: money = 66; tired = 44; hungry = 12; break;
					}
					ButtonsComponent[btnnum].effectComponent[1].effecttxtComponent.text = "-"+hungry;
					ButtonsComponent[btnnum].effectComponent[0].effecttxtComponent.text = "+"+ money;
					ButtonsComponent[btnnum].effectComponent[2].effecttxtComponent.text = "+"+ tired;
				}
				if (ShopUIInfo[i].name == "승진") {
					ButtonsComponent[btnnum].explaintxtComponent.text = "승진 지능 : " + DataController.Instance.saveable.smart+" / "+ getCompanyneedSmart() + "\n현재 직급: "+ getCompanyRankName();
				}
			break;

			case Contents.MagmaSlime:
			ButtonsComponent[btnnum].explaintxtComponent.text = "몸무게 : "+ (0.01f+DataController.Instance.saveable.SlimeWeight * 100f - 0.01f*0.01f).ToString();
			break;
		}
		
		
	}
	void setAllText() {
		SetText(0);
		
		SetText(1);
		//SetText(2);
	}

	void RefreshMoveButtons() {
		if (page == 0) {
			PreviousButtonObj.SetActive(false);
		}
		else
		PreviousButtonObj.SetActive(true);

		if (page == maxPage) {
			NextButtonObj.SetActive(false);
		}
		else
		NextButtonObj.SetActive(true);
	}


	public void RefreshPage() {
		page = -1;

		float count = ShopUIInfo.Count;
		maxPage = (int)Mathf.Ceil(count/2)-1;

		if (count < 2) {
			for(int i=(int)count;i < 2;i++) {
				FindButton(i).SetActive(false);
			}
		}

		gotoNextPage();
		RefreshMoveButtons();
	}


	GameObject FindButton(int num) {
		return ButtonsComponent[num].itemImageComponent.transform.parent.gameObject;
	}


	public void TurnOffShopUI() {
		Global.gameController.global.JumpBtnObj.SetActive(true);
		this.gameObject.SetActive(false);
	}

	public void ExectueEvent(GameObject Obj) {
		string temp;

		temp = Obj.transform.parent.parent.gameObject.name;
		temp = temp.Substring(temp.Length-1);
		
		int Bigbtnnum = int.Parse(temp) - 1;

		temp = Obj.name;
		temp = temp.Substring(temp.Length-1);

		int clickedbutton = int.Parse(temp) - 1;

		int i = Bigbtnnum+page*2;
		string name = ShopUIInfo[i].name;

		//체킹했을때 돈이부족하는등 이벤트가 발생했을경우
		if (!RealEvent(name,i,clickedbutton,false)) {
			return;
		}

		Global.gameController.global.fadeInOutObj.SetActive(true);
		Global.gameController.StartCoroutine("FadeIn");
		isFade = true;

		shopButtonInfo.i = i;
		shopButtonInfo.Bigbtnnum = Bigbtnnum;
		shopButtonInfo.clickedbutton = clickedbutton;
		shopButtonInfo.name = name;
	}

	bool RealEvent(string name,int i,int clickedbutton,bool DoEvent) {
		int tired = 0;
		int hungry = 0;
		int smart = 0;
		int money = 0;

		switch(ShopUIInfo[i].contents) {
			case Contents.Bank:
				int howmuch = int.Parse(ShopUIInfo[i].buttonName[clickedbutton]);
				if (name == "입금")
				if (DataController.Instance.saveable.money < howmuch) {
					GameController.MakeCloudMessage("돈이 부족합니다");
					return false;
				}
				if (name == "출금")
				if (howmuch > DataController.Instance.saveable.bankMoney) {
					GameController.MakeCloudMessage("돈이 부족합니다");
					return false;
				}
				if (DoEvent) {
					///입금
					if (name == "입금" && DataController.Instance.saveable.money >= howmuch) {
						DataController.Instance.saveable.money-=howmuch;
						DataController.Instance.saveable.bankMoney+=howmuch;
					}

					///출금
					if (name == "출금" && howmuch <= DataController.Instance.saveable.bankMoney) {
						DataController.Instance.saveable.bankMoney-=howmuch;
						DataController.Instance.saveable.money+=howmuch;
					}
				}
				break;


			case Contents.School:
				switch(name) {
					case "일반 교육": tired = 3; smart = 3; hungry = -12; break;
					case "특별 교육": tired = 10; smart = 7; money = -10; hungry = -16; break;
					case "참교육": tired = 15; smart = 11; money = -30; hungry = -12; break;
				}
			break;


			case Contents.Company:
				if (name == "근무") {
					
					switch(DataController.Instance.saveable.companyRank) {
						case CompanyRank.intern: money = 30; tired = 28; hungry = -12; smart = 1; break;
						case CompanyRank.employee: money = 37; tired = 32; hungry = -12; smart = 1; break;
						case CompanyRank.assistantManager: money = 45; tired = 36; hungry = -12; smart = 1; break;
						case CompanyRank.Manager: money = 55; tired = 40; hungry = -12; smart = 1; break;
						case CompanyRank.GeneralManager: money = 66; tired = 44; hungry = -12; smart = 1; break;
					}
				}

				if (name == "승진") {
					if (DataController.Instance.saveable.smart >= getCompanyneedSmart()) {
						if (DoEvent) {
							DataController.Instance.saveable.companyRank++;
						}
					}
					else {
						GameController.MakeCloudMessage("지능이 부족합니다");
						return false;
					}
				}
			break;


			case Contents.Store:
				switch(name) {
					case "감자튀김": money = -4; hungry = 8; break;
					case "핫도그": money = -6; hungry = 12; break;
					case "햄버거": money = -8; hungry = 17; break;
					case "치킨": money = -11; hungry = 23; break;
					case "피자": money = -14; hungry = 30; break;
					case "음료수": money = -2; hungry = 3; break;
				}
			break;


			case Contents.Bed:
				if (DoEvent)
				if (name == "잠자기") {
					DataController.Instance.saveable.tired-=50;
					DataController.Instance.saveable.hungry-=20;
					DataController.Instance.saveable.bankMoney += Mathf.FloorToInt(DataController.Instance.saveable.bankMoney*0.03f);

					DataController.SaveDatas(DataController.Instance.saveable,Global.filePath);

					return true;
				}
			break;

			case Contents.Casino:
				int howmuch2 = int.Parse(ShopUIInfo[i].buttonName[clickedbutton]);
				if (DataController.Instance.saveable.money < howmuch2) {
					GameController.MakeCloudMessage("돈이 부족합니다");
					return false;
				}
				if (DoEvent) {
					switch(name) {
						case "초급 동전 던지기": 
							if (DataController.Instance.saveable.money >= howmuch2) {
								DataController.Instance.casino.casinoType = CasinoType.EasyCoin;
								DataController.Instance.casino.betMoney = howmuch2;
								DataController.Instance.saveable.money-=howmuch2;
								SceneManager.LoadScene("Casino");
							}
						break;
						case "중급 동전 던지기":
							if (DataController.Instance.saveable.money >= howmuch2) {
								DataController.Instance.casino.casinoType = CasinoType.MiddleCoin;
								DataController.Instance.casino.betMoney = howmuch2;
								DataController.Instance.saveable.money-=howmuch2;
								SceneManager.LoadScene("Casino");
							}
						break;
						case "키또": break;
					}
					
				}
			
			break;

			case Contents.Salon:
				switch(name) {
					case "기본 머리": money = -70; tired = -14; hungry = -4; break;
					case "안드로이드 머리": money = -70; tired = -14; hungry = -4; break;
					case "더듬이 머리": money = -90; tired = -18; hungry = -4; break;
					case "나무 머리": money = -110; tired = -22; hungry = -4; break;
					case "닭 머리": money = -260; tired = -52; hungry = -4; break;
					case "지구 머리": money = -290; tired = -58; hungry = -4; break;
					case "외계인 머리": money = -340; tired = -68; hungry = -4; break;
					case "콜라 머리": money = -400; tired = -80; hungry = -4; break;
					case "말 머리": money = -710; tired = -142; hungry = -4; break;
					case "작은 머리": money = -740; tired = -148; hungry = -4; break;
					case "해적왕 머리": money = -780; tired = -156; hungry = -4; break;
					case "알파카 머리": money = -810; tired = -162; hungry = -4; break;
				}

				if (DoEvent) {
					DataController.Instance.saveable.head = (HeadType)i;
					Global.gameController.global.PlayerObj.GetComponent<PlayerController>().RefreshSkin();
				}
				
			break;

			case Contents.ClothingStore:
				switch(name) {
					case "기본 옷": money = -210; tired = -21; hungry = -2; break;
					case "안드로이드 옷": money = -220; tired = -22; hungry = -2; break;
					case "개미 옷": money = -270; tired = -27; hungry = -2; break;
					case "꿀벌 옷": money = -330; tired = -33; hungry = -2; break;
					case "눈사람 옷": money = -780; tired = -78; hungry = -2; break;
					case "펭귄 옷": money = -870; tired = -87; hungry = -2; break;
					case "콜라 옷": money = -1010; tired = -101; hungry = -2; break;
					case "막대기 옷": money = -1190; tired = -119; hungry = -2; break;
					case "달팽이 옷": money = -2120; tired = -212; hungry = -2; break;
					case "애벌레 옷": money = -2220; tired = -222; hungry = -2; break;
					case "탱크 옷": money = -2330; tired = -233; hungry = -2; break;
					case "알파카 옷": money = -2440; tired = -244; hungry = -2; break;
				}

				if (DoEvent) {
					DataController.Instance.saveable.body = (BodyType)i;
					Global.gameController.global.PlayerObj.GetComponent<PlayerController>().RefreshSkin();
				}
			break;

			case Contents.Train:
				if (DoEvent) {
					switch(i) {
						case 0: DataController.Instance.saveable.DirectionalLight = true; hungry = -8; break;
					}
					Global.gameController.global.PlayerObj.transform.position = Global.gameController.global.FPos.position;
					TurnOffShopUI();
				}
			break;

			case Contents.Spa:
				money = -25;
				tired = -30;
				hungry = -4;
			break;

			case Contents.MagmaSlime:
			int howmuch3 = int.Parse(ShopUIInfo[i].buttonName[clickedbutton]);
			money = -howmuch3;
			hungry = -2;

			if (DoEvent) {

			if (DataController.Instance.saveable.SlimeWeight < 200)
			DataController.Instance.saveable.SlimeWeight += 0.01f + (float)howmuch3 * 0.01f;
			else {
				GameController.MakeCloudMessage("슬라임을 더이상 키울 수 없습니다");
				return false;
			}

			}
			
			break;

		}

		//if (DataController.Instance.saveable.hungry - hungry > 100) {
		//	GameController.MakeCloudMessage("배가 고픕니다");
		//	return false;
		//}

		if (DataController.Instance.saveable.tired + tired > 100) {
			GameController.MakeCloudMessage("피곤합니다");
			return false;
		}

		if (DataController.Instance.saveable.money + money < 0) {
			GameController.MakeCloudMessage("돈이 부족합니다");
			return false;
		}

		if (DoEvent) {
			DataController.Instance.saveable.hungry+=hungry;
			DataController.Instance.saveable.tired+=tired;
			DataController.Instance.saveable.smart+=smart;
			DataController.Instance.saveable.money+=money;

			setAllText();
		}

		return true;
	}


	int getCompanyneedSmart() {
		int needSmart = 0;
		switch(DataController.Instance.saveable.companyRank) {
			case CompanyRank.intern: needSmart = 60; break;
			case CompanyRank.employee: needSmart = 330; break;
			case CompanyRank.assistantManager: needSmart = 1361; break;
			case CompanyRank.Manager: needSmart = 4083; break;
			case CompanyRank.GeneralManager: needSmart = 0; break;
		}
		return needSmart;
	}


	string getCompanyRankName() {
		string name = "";
		switch(DataController.Instance.saveable.companyRank) {
			case CompanyRank.intern: name = "인턴"; break;
			case CompanyRank.employee: name = "사원"; break;
			case CompanyRank.assistantManager: name = "대리"; break;
			case CompanyRank.Manager: name = "과장"; break;
			case CompanyRank.GeneralManager: name = "부장"; break;
		}
		return name;
	}
	
}
