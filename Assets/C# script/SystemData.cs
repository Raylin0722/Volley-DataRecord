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
    [SerializeField] public GameObject[] changeLeftPlayers;
    [SerializeField] public GameObject[] changeRightPlayers;
    public Vector2[] leftPlayersPos;
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
    public int leftRight;
    public int[] leftTeamNum, rightTeamNum;
    public bool changePlayer;

    void Start(){
        score = new int[2];
        point = new int[2];
        leftScoreText.text = "0";
        rightScoreText.text = "0";
        leftPointText.text = "00";
        rightPointText.text = "00";
        saveData = new List<dealDB.Data>();
        leftRight = UserData.Instance.leftRight;
        leftTeamNum = new int[1];
        leftTeamNum[0] = UserData.Instance.leftTeamNum;
        rightTeamNum = new int[1];
        rightTeamNum[0] = UserData.Instance.rightTeamNum;
        leftPlayersPos = new Vector2[6];
        rightPlayersPos = new Vector2[6];
        changePosition = false;
        changePlayer = false;
        if(leftRight == 0){ // 使用者 left 0 right 1
            leftTeamName.text = UserData.Instance.UserTeamName;
            rightTeamName.text = UserData.Instance.EnemyTeamName;
            for(int i = 0 ; i < 6; i++){
                leftPlayers[i].GetComponent<dragPlayer>().playerName = UserData.Instance.UserPlayerName[i];
                rightPlayers[i].GetComponent<dragPlayer>().playerName = UserData.Instance.EnemyPlayerName[i];
                leftPlayers[i].GetComponent<dragPlayer>().playerNum = UserData.Instance.UserPlayerNumber[i].ToString();
                rightPlayers[i].GetComponent<dragPlayer>().playerNum = UserData.Instance.EnemyPlayerNumber[i].ToString();
                leftPlayers[i].GetComponent<dragPlayer>().playerPlayPos = UserData.Instance.UserPlayerPlayPos[i];
                rightPlayers[i].GetComponent<dragPlayer>().playerPlayPos = UserData.Instance.EnemyPlayerPlayPos[i];

                changeLeftPlayers[i].GetComponent<dragPlayerToChange>().playerName = UserData.Instance.UserPlayerName[i + 6];
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().playerName = UserData.Instance.EnemyPlayerName[i + 6];
                changeLeftPlayers[i].GetComponent<dragPlayerToChange>().playerNum = UserData.Instance.UserPlayerNumber[i + 6].ToString();
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().playerNum = UserData.Instance.EnemyPlayerNumber[i + 6].ToString();
                changeLeftPlayers[i].GetComponent<dragPlayerToChange>().playerPlayPos = UserData.Instance.UserPlayerPlayPos[i + 6];
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().playerPlayPos = UserData.Instance.EnemyPlayerPlayPos[i + 6];


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
                rightPlayers[i].GetComponent<dragPlayer>().playerPlayPos = UserData.Instance.UserPlayerPlayPos[i];
                leftPlayers[i].GetComponent<dragPlayer>().playerPlayPos = UserData.Instance.EnemyPlayerPlayPos[i];

                changeLeftPlayers[i].GetComponent<dragPlayerToChange>().playerName = UserData.Instance.EnemyPlayerName[i + 6];
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().playerName = UserData.Instance.UserPlayerName[i + 6];
                changeLeftPlayers[i].GetComponent<dragPlayerToChange>().playerNum = UserData.Instance.EnemyPlayerNumber[i + 6].ToString();
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().playerNum = UserData.Instance.UserPlayerNumber[i + 6].ToString();
                changeLeftPlayers[i].GetComponent<dragPlayerToChange>().playerPlayPos = UserData.Instance.EnemyPlayerPlayPos[i + 6];
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().playerPlayPos = UserData.Instance.UserPlayerPlayPos[i + 6];
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().FirstPlayer = false;
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().FirstPlayer = false;
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().timeOfChange = 1;
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().timeOfChange = 1;
            }
            
        }
        
    }

    public void changePlayerPos(){
        if(!changePosition)
            changePosition = true;
        else
            changePosition = false;
    }

    public void CplayerPos(){
        if(!changePlayer)
            changePlayer = true;
        else
            changePlayer = false;
    }

}
