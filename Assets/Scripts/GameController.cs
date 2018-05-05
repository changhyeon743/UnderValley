using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class Global{
    public static string filePath;
    public static GameController gameController;
    public static ShopTalkController shopTalkController;
    public Vector3 dir;
    public float speed;
    public GameObject CommonTalkObj;
    public GameObject fadeInOutObj;
    public Light DirectionalLight;
    public Light ContentsLight;
    public int ButtonSize = 3;
    public GameObject[] MenuButtonsObj;
    public GameObject currentPortal;
    public GameObject ShopUIObj;
    public GameObject msgObj;
    public Shader illuminShader;
    public GameObject PlayerObj;
    public Transform FPos;
    public GameObject JumpBtnObj;
    public GameObject MineBtnObj;
    public GameObject Pickax;
}

public class GameController : MonoBehaviour {

    /** 레이캐스트로 npc와 대화가능, Start에서 여러가지 오브젝트를 다룸. 해상도설정. statusupdate()을 통해 statusUI에 담기는 값 실시간 업데이트 **/
    public Text[] statustext;

    public Text Timetxt;

    public Global global;

	private string dotString;

    void Awake() {
        Global.gameController = this.GetComponent<GameController>();
        Global.filePath = Application.dataPath + "/data.bin";
        StartCoroutine(AutoSave());
    }

    IEnumerator AutoSave() {
        while(true) {
            yield return new WaitForSeconds(2f);
            DataController.SaveDatas(DataController.Instance.saveable,Global.filePath);
        }
    }

    void Start() {
        Screen.SetResolution(1280, 720, true);

        //global.DirectionalLight.gameObject.GetComponent<sunController>().ChangeTransform();

        
        global.PlayerObj.transform.position = new Vector3(DataController.Instance.saveable.playerXYZ.x,DataController.Instance.saveable.playerXYZ.y,DataController.Instance.saveable.playerXYZ.z);
    }

    void Update() {
        DataController.Instance.saveable.tired = Mathf.Clamp(DataController.Instance.saveable.tired,0,100);
        DataController.Instance.saveable.hungry = Mathf.Clamp(DataController.Instance.saveable.hungry,0,100);

        DataController.Instance.saveable.playerXYZ.x = Global.gameController.global.PlayerObj.transform.position.x;
        DataController.Instance.saveable.playerXYZ.y = Global.gameController.global.PlayerObj.transform.position.y;
        DataController.Instance.saveable.playerXYZ.z = Global.gameController.global.PlayerObj.transform.position.z;

        statustext[0].text = DataController.Instance.saveable.money.ToString();
        
        statustext[1].text = DataController.Instance.saveable.crystal.ToString();
        
        statustext[2].text = DataController.Instance.saveable.hungry.ToString();
        statustext[2].transform.parent.GetChild(1).localScale = new Vector2((float)DataController.Instance.saveable.hungry / 100, statustext[2].transform.parent.GetChild(0).localScale.y);     

        if (Input.GetKeyDown(KeyCode.W)) {

        }

        if(Input.GetMouseButtonDown(0)) {
            var Data_Obj = CheckRaycast(Input.mousePosition);
            if (Data_Obj != null)
            if (isNotUIRunning())
            if (Data_Obj.CompareTag("npc")) {
                Data_Obj.GetComponent<GiveTextToTalk>().SendTextToTalkController();
            }

            /*Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
 
            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0) {
                if (results[0].gameObject.tag == ("Untouchable")) {
                    return;
                }
            }

            //int layermask = (1<<LayerMask.NameToLayer("UI") | (1<<LayerMask.NameToLayer("Character"))); 
            if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1 << 9 )) {
                if (hitInfo.transform.CompareTag("npc"))
                hitInfo.transform.GetComponent<GiveTextToTalk>().SendTextToTalkController();
            }*/
        }

        if (Global.gameController.global.DirectionalLight.gameObject.transform.position.y < 0) {
            Global.gameController.global.DirectionalLight.enabled = false;
            Global.gameController.global.ContentsLight.enabled = !DataController.Instance.saveable.DirectionalLight;
        }
        else {
                Global.gameController.global.DirectionalLight.enabled = true;
                Global.gameController.global.DirectionalLight.GetComponent<Light>().intensity = DataController.Instance.saveable.LightIntensity;
                Global.gameController.global.ContentsLight.GetComponent<Light>().intensity = DataController.Instance.saveable.LightIntensity;
                Global.gameController.global.ContentsLight.enabled = !DataController.Instance.saveable.DirectionalLight;
                Global.gameController.global.DirectionalLight.enabled = DataController.Instance.saveable.DirectionalLight;
        }

        
        
    }  
    
    public static GameObject CheckRaycast(Vector3 pos) {
        Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        PointerEventData pointerData = new PointerEventData(EventSystem.current);

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0) {
            if (results[0].gameObject.CompareTag("Untouchable")) {
                return results[0].gameObject;
            }
        }

        if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1 << 9 )) {
            return hitInfo.transform.gameObject;
        }

        return null;
    }

    public static bool isNotUIRunning() {
        return (!Global.gameController.global.MenuButtonsObj[0].activeSelf && !Global.gameController.global.MenuButtonsObj[1].activeSelf && !Global.gameController.global.MenuButtonsObj[2].activeSelf && !Global.gameController.global.ShopUIObj.activeSelf && !Global.gameController.global.CommonTalkObj.activeSelf);
    }

    public IEnumerator FadeIn() {
        for(float i = 0f; i <= 1.1f; i += 0.1f) {
            Color color = new Color(0,0,0,i);
            Global.gameController.global.fadeInOutObj.GetComponent<Image>().color = color;
            yield return 0;
        }
    }

    public IEnumerator FadeOut() {
        for(float i = 1f; i >= -0.1f; i -= 0.1f) {
            Color color = new Color(0,0,0,i);
            Global.gameController.global.fadeInOutObj.GetComponent<Image>().color = color;
            yield return 0;
        }
    }

    public static void MakeCloudMessage(string strText) {
        //GameObject RealMsgObj = Instantiate(Global.gameController.global.msgObj) as GameObject;
        //RealMsgObj.transform.SetParent(GameObject.Find("Canvas").transform,false);
        //RealMsgObj.GetComponent<CloudMessageController>().textComponent.text = strText;
    }

}