using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CasinoController : MonoBehaviour {

    public Text BettingText;
    public Button RollBtn;
    public Button ExitBtn;
    public int RollNum = 0;
    
    public bool isGameEnd = false;

    public Coin coin;

    void Start() {
        BettingText.text = "건 돈 : "+DataController.Instance.casino.betMoney;
    }

	public void GoBackToMain() {
        //DataController.SaveDatas(DataController.Instance.saveable,Global.filePath);
        SceneManager.LoadScene("Main");
    }

    public void GameIsEnd() {
        if (DataController.Instance.casino.casinoType == CasinoType.EasyCoin) {
            if (Coin.isUp) {
                BettingText.text = "성공했습니다!";
                ExitBtn.transform.GetChild(0).GetComponent<Text>().text = "받고 나가기";
            }
            
            if (!Coin.isUp) {
                BettingText.text = "실패했습니다";
                ExitBtn.transform.GetChild(0).GetComponent<Text>().text = "나가기";
            }
            RollBtn.gameObject.SetActive(false);
            isGameEnd = true;
        }
        if (DataController.Instance.casino.casinoType == CasinoType.MiddleCoin) {
            if (RollNum < 3) {
                RollNum++;
            }

            if (Coin.isUp) {
                BettingText.text = "성공했습니다!";

                if (RollNum >= 3) {
                    RollBtn.gameObject.SetActive(false);
                }
                else
                RollBtn.transform.GetChild(0).GetComponent<Text>().text = "한번 더 던지기";

                isGameEnd = false;

                ExitBtn.transform.GetChild(0).GetComponent<Text>().text = "받고 나가기";
            }
            
            if (!Coin.isUp) {
                BettingText.text = "실패했습니다";
                RollBtn.gameObject.SetActive(false);
                ExitBtn.transform.GetChild(0).GetComponent<Text>().text = "나가기";
            }
            
            isGameEnd = true;
        }
    }

    public void ExitButtonClick() {
        DataController.SaveDatas(DataController.Instance.saveable,Global.filePath);
        if(!isGameEnd) {
            GoBackToMain();
        }
        if (isGameEnd) {
            if (DataController.Instance.casino.casinoType == CasinoType.EasyCoin) {
                if (Coin.isUp) {
                    DataController.Instance.saveable.money += DataController.Instance.casino.betMoney * 2;
                }
                GoBackToMain();
            }
            if (DataController.Instance.casino.casinoType == CasinoType.MiddleCoin) {
                if (Coin.isUp) {
                    DataController.Instance.saveable.money += DataController.Instance.casino.betMoney * (RollNum+1);
                }
                GoBackToMain();
            }
        }
    }

    public void RollButtonClick() {
        if (DataController.Instance.casino.casinoType == CasinoType.EasyCoin) {
            coin.Roll();
        }
        if (DataController.Instance.casino.casinoType == CasinoType.MiddleCoin) {
            coin.Roll();
        }
    }
    

}
