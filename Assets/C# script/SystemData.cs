using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SystemData : MonoBehaviour
{
    public string formation; // 紀錄當前陣容
    [SerializeField] public GameObject[] leftPlayers; // 左方球員
    [SerializeField] public GameObject[] rightPlayers; // 右方球員
    [SerializeField] public int[] initLeftPlayers;
    [SerializeField] public int[] initRightPlayers;
    
    public Vector2[] leftPLayersPos;
    public Vector2[] rightPlayersPos;
    public int[] point; // 小分
    public int[] score; // 大分
    public Text leftScoreText; // 左方分數文字
    public Text rightScoreText; // 右方分數文字
    public Text leftPointText;
    public Text rightPointText;
    public Text leftTeamName;
    public Text rightTeamName;
    public int whoWin;
    public int setServe;
    public List<dealDB.Data> saveData; // 資料儲存
    public bool changePosition; //更換位子變數判斷

    void Awake(){
        score = new int[2];
        leftScoreText.text = "0";
        rightScoreText.text = "0";
        leftPointText.text = "00";
        rightPointText.text = "00";
        saveData = new List<dealDB.Data>();
        changePosition = false;
        if(true){ // 使用者 left 0 right -1
            leftTeamName.text = UserData.Instance.UserTeamName;
            rightTeamName.text = UserData.Instance.EnemyTeamName;
            for(int i = 0 ; i < 6; i++){
                leftPlayers[i].GetComponent<dragPlayer>().playerName = UserData.Instance.UserPlayerName[i];
                rightPlayers[i].GetComponent<dragPlayer>().playerName = UserData.Instance.EnemyPlayerName[i];
                leftPlayers[i].GetComponent<dragPlayer>().playerNum = UserData.Instance.UserPlayerNumber[i].ToString();
                rightPlayers[i].GetComponent<dragPlayer>().playerNum = UserData.Instance.EnemyPlayerNumber[i].ToString();
                initLeftPlayers[i] = UserData.Instance.UserPlayerNumber[i];
                initRightPlayers[i] = UserData.Instance.EnemyPlayerNumber[i];
            }
        }
        else{
            rightTeamName.text = UserData.Instance.UserTeamName;
            leftTeamName.text = UserData.Instance.EnemyTeamName;
            for(int i = 0 ; i < 6; i++){
                rightPlayers[i].GetComponent<dragPlayer>().playerName = UserData.Instance.UserPlayerName[i];
                leftPlayers[i].GetComponent<dragPlayer>().playerName = UserData.Instance.EnemyPlayerName[i];
                rightPlayers[i].GetComponent<dragPlayer>().playerNum = UserData.Instance.UserPlayerNumber[i].ToString();
                leftPlayers[i].GetComponent<dragPlayer>().playerNum = UserData.Instance.EnemyPlayerNumber[i].ToString();
                initRightPlayers[i] = UserData.Instance.UserPlayerNumber[i];
                initLeftPlayers[i] = UserData.Instance.EnemyPlayerNumber[i];
            }
        }
        
    }

}
