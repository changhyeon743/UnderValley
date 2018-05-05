using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
public class JoyStickController : MonoBehaviour {

    /** 조이스틱 알고리즘, 캐릭터 이동과 연결, Global.dir, Global.speed같은 변수들과 연결됨. **/

   public bool OnClick = false;
   private Vector3 FirstPosition;
   
   public GameObject bigButton;

   Image selfImage;
   Image bigButtonImage;

   float radius;

   void Start() {
      selfImage = GetComponent<Image>();
      bigButtonImage = bigButton.GetComponent<Image>();
	  radius = (transform.parent.GetComponent<RectTransform>().rect.width * transform.parent.GetComponent<RectTransform>().localScale.x) / 2;
   }

   void Update() {
        if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer) {
        if (Input.GetMouseButtonDown(0)) {
            bool IsClickAnything = false;


            var Data_Obj = GameController.CheckRaycast(Input.mousePosition);
            //print(Data_Obj);
            if (Data_Obj != null)
            if (Data_Obj.CompareTag("npc") || Data_Obj.CompareTag("Untouchable")) {
                IsClickAnything = true;
            }
            

            if (!IsClickAnything)
            if (!Global.gameController.global.ShopUIObj.activeSelf && !Global.gameController.global.CommonTalkObj.activeSelf)
            if (Input.mousePosition.x < Screen.width / 2) {
                OnClick = true;
                FirstPosition = Input.mousePosition;
                bigButton.transform.position = FirstPosition;
                return;
            }
            else { //Jump
                
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            OnClick = false;
            Global.gameController.global.dir = Vector3.zero;
            Global.gameController.global.speed = 0;
            bigButton.transform.position = FirstPosition;
            transform.position = bigButton.transform.position;
            return;
        }
    }
    else
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            switch(touch.phase) {
                case TouchPhase.Began:
                    bool IsClickAnything = false;

                    var Data_Obj = GameController.CheckRaycast(touch.position);
                    //print(Data_Obj);
                    if (Data_Obj != null)
                    if (Data_Obj.CompareTag("npc") || Data_Obj.CompareTag("Untouchable")) {
                        IsClickAnything = true;
                    }

                    if (!IsClickAnything)
                    if (!Global.gameController.global.ShopUIObj.activeSelf && !Global.gameController.global.CommonTalkObj.activeSelf)
                    if (touch.position.x < Screen.width / 2) {
                        OnClick = true;
                        FirstPosition = touch.position;
                        bigButton.transform.position = FirstPosition;
                        return;
                    }
                break;

                case TouchPhase.Ended:
                    OnClick = false;
                    Global.gameController.global.dir = Vector3.zero;
                    Global.gameController.global.speed = 0;
                    bigButton.transform.position = FirstPosition;
                    transform.position = bigButton.transform.position;
                return;
            }
        }
            

        if (OnClick) {
            if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
            OnClickCheck(Input.mousePosition);
            else
            OnClickCheck(Input.GetTouch(0).position);
        }

        if (!OnClick) {
            if (selfImage.enabled == true) {
                selfImage.enabled = false;
                bigButtonImage.enabled = false;
            }
        }
   }

   bool OnClickCheck(Vector3 pos) {
       var Data_Obj = GameController.CheckRaycast(pos);
        //print(Data_Obj);
        if (Data_Obj != null)
        if (Data_Obj.name == "JumpBtn" || Global.gameController.global.ShopUIObj.activeSelf || Global.gameController.global.CommonTalkObj.activeSelf) {
            OnClick = false;
            return false;
        }

        float touchArea = Vector3.Distance(FirstPosition,new Vector3(pos.x,pos.y));
        Global.gameController.global.dir = (new Vector3(pos.x,pos.y) - FirstPosition).normalized;
        Vector2 touchPositionFix = pos;

        if (touchArea > radius)
        touchPositionFix = FirstPosition + Global.gameController.global.dir * radius;

        Global.gameController.global.speed = Vector3.Distance(FirstPosition,new Vector3(touchPositionFix.x,touchPositionFix.y));

        transform.position = touchPositionFix; //

        if (selfImage.enabled == false) {
            selfImage.enabled = true;
            bigButtonImage.enabled = true;
        }

        return true;
   }

}