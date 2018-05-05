using UnityEngine;
using System.Collections.Generic;
public enum SendTextType {
    CommonTalk,
    ShopTalk
}

public class GiveTextToTalk : MonoBehaviour{

    /** npc오브젝트 안에 들어가는 스크립트로, 인스펙터에서 npc의 이름, 대사등을 지정할 수 있음. GameController의 레이캐스트와 연결. **/

    public SendTextType Type;
    public List<string> Text;
    public string Name;

    public List<ShopUI> ShopInfo;
    
    
    public void SetTalkobjActive(GameObject obj, bool active) {
        obj.SetActive(active);
    }

    public void SendTextToTalkController() {
        if (Type == SendTextType.CommonTalk) {
            Global.gameController.global.JumpBtnObj.SetActive(false);
            Global.gameController.global.MineBtnObj.SetActive(false);

            var component = Global.gameController.global.CommonTalkObj.GetComponent<CommonTalkController>();

            SetTalkobjActive(Global.gameController.global.CommonTalkObj, true);
            component.TextList = Text;
            component.stringName = Name;
            component.GetComponent<CommonTalkController>().ResetPage();
        }
        if (Type == SendTextType.ShopTalk) {
            Global.gameController.global.JumpBtnObj.SetActive(false);
            SetTalkobjActive(Global.gameController.global.ShopUIObj, true);
            Global.gameController.global.ShopUIObj.GetComponent<ShopTalkController>().ShopUIInfo = ShopInfo;
            Global.gameController.global.ShopUIObj.GetComponent<ShopTalkController>().RefreshPage();
        }
    }
}
