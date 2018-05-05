using UnityEngine;
public class MenuController : MonoBehaviour{
    /** Num변수는 인스펙터에서 지정, 자신이 몇번째 버튼인지를 뜻함. 이 변수는 포탈에 닿아서 플레이어가 버튼을 눌렀을때 몇번째 버튼을 눌렀는지를 전송하기 위해 사용 **/
    public int Num;

    public void MouseClick() {
        Global.gameController.global.currentPortal.GetComponent<PortalController>().MoveTransform(Num);
    }
}