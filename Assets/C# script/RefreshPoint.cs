using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class RefreshPoint : MonoBehaviour
{
    const int LEFT = 0, RIGHT = 1;
    public int whoServe, startServe, reWhoServe,  reClick;
    [SerializeField] GameObject serverData;
    [SerializeField] GameObject leftServeTag;
    [SerializeField] GameObject rightServeTag;
    
    
    private SystemData systemDataScript;

    bool lastRound = false, stop = false;
    
    
    void Start(){
        systemDataScript = serverData.GetComponent<SystemData>();
        whoServe = UserData.Instance.whoServe; // 跟伺服器拿資料
        //whoServe = 1;
        startServe = whoServe; // 跟伺服器拿資料
        if(whoServe == 0){
            leftServeTag.SetActive(true);
            rightServeTag.SetActive(false);
        }
        else{
            leftServeTag.SetActive(false);
            rightServeTag.SetActive(true);
        }
        reClick = -1;
    }

    public void addPoint(){ // 小比分加分
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        int endPoint = 25;
        if(systemDataScript.score[LEFT] + systemDataScript.score[RIGHT] >= 4)
            endPoint = 15;
        
        if((systemDataScript.point[LEFT] == endPoint && systemDataScript.point[RIGHT] <= endPoint - 2) || 
                systemDataScript.point[LEFT] > endPoint &&  systemDataScript.point[LEFT] - systemDataScript.point[RIGHT] > 2)
                    return;

        if((systemDataScript.point[RIGHT] == endPoint && systemDataScript.point[LEFT] <= endPoint - 2) || 
                systemDataScript.point[RIGHT] > endPoint &&  systemDataScript.point[RIGHT] - systemDataScript.point[LEFT] > 2)
                    return;

        if(obj.tag == "LeftPoint" ){
            systemDataScript.point[LEFT]++;
            reClick = LEFT;
            reWhoServe = whoServe;
            if(whoServe == RIGHT){
                rightServeTag.SetActive(false);
                leftServeTag.SetActive(true);
                whoServe = LEFT;
            }
            systemDataScript.leftPointText.text = systemDataScript.point[LEFT] >= 10 
                ? systemDataScript.point[LEFT].ToString() : "0" + systemDataScript.point[LEFT].ToString();
        }
        else if(obj.tag == "RightPoint"){
            systemDataScript.point[RIGHT]++;
            reClick = RIGHT;
            reWhoServe = whoServe;
            if(whoServe == LEFT){
                leftServeTag.SetActive(false);
                rightServeTag.SetActive(true);
                whoServe = RIGHT;
            }
            systemDataScript.rightPointText.text = systemDataScript.point[RIGHT] >= 10 
                ? systemDataScript.point[RIGHT].ToString() : "0" + systemDataScript.point[RIGHT].ToString();
        }
        

    }
    public void addScore(){ // 大比分加分 插入資料再判斷
        int endPoint = 25;
        bool clear = false ;
        if(systemDataScript.score[LEFT] + systemDataScript.score[RIGHT] >= 4){
            endPoint = 15;
            lastRound = true;
        }
        
        if(systemDataScript.point[LEFT] >= endPoint && systemDataScript.point[LEFT] - systemDataScript.point[RIGHT] >= 2){
            systemDataScript.score[LEFT]++;
            clear = true;
        }
        else if(systemDataScript.point[RIGHT] >= endPoint && systemDataScript.point[RIGHT] - systemDataScript.point[LEFT] >= 2){
            systemDataScript.score[RIGHT]++;
            clear = true;
        }
        if(clear && !lastRound){
            systemDataScript.point[LEFT] = 0;
            systemDataScript.point[RIGHT] = 0;
            systemDataScript.leftPointText.text = "00";
            systemDataScript.rightPointText.text = "00";
            systemDataScript.leftScoreText.text = systemDataScript.score[LEFT].ToString();
            systemDataScript.rightScoreText.text = systemDataScript.score[RIGHT].ToString();
            LRChange();
        }
        else if(clear && lastRound)
            stop = true;
        
        
    }
    public void rotate(){ // 輪轉
        string tmpName, tmpNum;
        int tmpPos;
        if(reClick == LEFT && reWhoServe == RIGHT){
            tmpName = systemDataScript.leftPlayers[0].GetComponent<dragPlayer>().playerName;
            tmpNum = systemDataScript.leftPlayers[0].GetComponent<dragPlayer>().playerNum;
            tmpPos = systemDataScript.leftPlayers[0].GetComponent<dragPlayer>().playerPlayPos;
            for(int i = 0; i < 5; i++){
                systemDataScript.leftPlayers[i].GetComponent<dragPlayer>().playerName = systemDataScript.leftPlayers[i + 1].GetComponent<dragPlayer>().playerName;
                systemDataScript.leftPlayers[i].GetComponent<dragPlayer>().playerNum = systemDataScript.leftPlayers[i + 1].GetComponent<dragPlayer>().playerNum;
                systemDataScript.leftPlayers[i].GetComponent<dragPlayer>().playerPlayPos = systemDataScript.leftPlayers[i + 1].GetComponent<dragPlayer>().playerPlayPos;
            }
            systemDataScript.leftPlayers[5].GetComponent<dragPlayer>().playerName = tmpName;
            systemDataScript.leftPlayers[5].GetComponent<dragPlayer>().playerNum = tmpNum;
            systemDataScript.leftPlayers[5].GetComponent<dragPlayer>().playerPlayPos = tmpPos;


        }
        else if(reClick == RIGHT && reWhoServe == LEFT){
            tmpName = systemDataScript.rightPlayers[0].GetComponent<dragPlayer>().playerName;
            tmpNum = systemDataScript.rightPlayers[0].GetComponent<dragPlayer>().playerNum;
            tmpPos = systemDataScript.rightPlayers[0].GetComponent<dragPlayer>().playerPlayPos;
            for(int i = 0; i < 5; i++){
                systemDataScript.rightPlayers[i].GetComponent<dragPlayer>().playerName = systemDataScript.rightPlayers[i + 1].GetComponent<dragPlayer>().playerName;
                systemDataScript.rightPlayers[i].GetComponent<dragPlayer>().playerNum = systemDataScript.rightPlayers[i + 1].GetComponent<dragPlayer>().playerNum;
                systemDataScript.rightPlayers[i].GetComponent<dragPlayer>().playerPlayPos = systemDataScript.rightPlayers[i + 1].GetComponent<dragPlayer>().playerPlayPos;
            }
            systemDataScript.rightPlayers[5].GetComponent<dragPlayer>().playerName = tmpName;
            systemDataScript.rightPlayers[5].GetComponent<dragPlayer>().playerNum = tmpNum;
            systemDataScript.rightPlayers[5].GetComponent<dragPlayer>().playerPlayPos = tmpPos;
        }
        for(int i = 0; i < 6; i++){
            systemDataScript.leftPlayers[i].GetComponent<dragPlayer>().updata[0] = false;
            systemDataScript.rightPlayers[i].GetComponent<dragPlayer>().updata[0] = false;
            systemDataScript.leftPlayers[i].SetActive(true);
            systemDataScript.rightPlayers[i].SetActive(true);
        }
        int lindex = -1, rindex = -1;
        for(int i = 0; i < 6; i++){
            if(systemDataScript.leftPlayers[i].GetComponent<dragPlayer>().playerPlayPos == 2)
                lindex = i;
            if(systemDataScript.rightPlayers[i].GetComponent<dragPlayer>().playerPlayPos == 2)
                rindex = i;
        }
        for(int i = 0; i < 6; i++){
            if(whoServe == LEFT){ // 左發球 右接球
                systemDataScript.rightPlayers[i].transform.position = (systemDataScript.rightStartCatchPos[rindex, i]);
                systemDataScript.leftPlayers[i].transform.position = (systemDataScript.leftStartServePos[i]);
            }
            else{
                systemDataScript.leftPlayers[i].transform.position = (systemDataScript.leftStartCatchPos[lindex, i]);
                systemDataScript.rightPlayers[i].transform.position = (systemDataScript.rightStartServePos[i]);
            }
        }
        reClick = -1;
    }
    public void changeSideServe(){ // 局換發
        if((systemDataScript.score[LEFT] + systemDataScript.score[RIGHT]) % 2 != 0)
            whoServe = 1 - whoServe;
        if(whoServe == LEFT){
            leftServeTag.SetActive(true);
            rightServeTag.SetActive(false);
        }
        else{
            leftServeTag.SetActive(false);
            rightServeTag.SetActive(true);
        }
    }
    public void back(){ // 回復比分
        if(reClick == -1)
            return;
        if(reClick == LEFT){
            systemDataScript.point[LEFT]--;
            whoServe = reWhoServe;
            if(reWhoServe == RIGHT){
                leftServeTag.SetActive(false);
                rightServeTag.SetActive(true);
            }
            systemDataScript.leftPointText.text = systemDataScript.point[LEFT] >= 10 
                ? systemDataScript.point[LEFT].ToString() : "0" + systemDataScript.point[LEFT].ToString();
        }
        else if(reClick == RIGHT){
            systemDataScript.point[RIGHT]--;
            whoServe = reWhoServe;
            if(reWhoServe == LEFT){
                leftServeTag.SetActive(true);
                rightServeTag.SetActive(false);
            }
            systemDataScript.rightPointText.text = systemDataScript.point[RIGHT] >= 10 
                ? systemDataScript.point[RIGHT].ToString() : "0" + systemDataScript.point[RIGHT].ToString();
        }
        reClick = -1;
    }
    void LRChange(){
        string tmpText;
        tmpText = systemDataScript.leftTeamName.text;
        systemDataScript.leftTeamName.text = systemDataScript.rightTeamName.text;
        systemDataScript.rightTeamName.text = tmpText;

        int tmpPoint;
        tmpPoint = systemDataScript.point[0];
        systemDataScript.point[0] = systemDataScript.point[1];
        systemDataScript.point[1] = tmpPoint;

        tmpPoint = systemDataScript.score[0];
        systemDataScript.score[0] = systemDataScript.score[1];
        systemDataScript.score[1] = tmpPoint;


        tmpText = systemDataScript.leftScoreText.text;
        systemDataScript.leftScoreText.text = systemDataScript.rightScoreText.text;
        systemDataScript.rightScoreText.text = tmpText;

        tmpText = systemDataScript.leftPointText.text;
        systemDataScript.leftPointText.text = systemDataScript.rightPointText.text;
        systemDataScript.rightPointText.text = tmpText;

        whoServe = startServe;

        string tmpName, tmpNum;
        int tmpPos;

        for(int i = 0; i < 6; i++){
            tmpName = systemDataScript.leftPlayers[i].GetComponent<dragPlayer>().playerName;
            tmpNum = systemDataScript.leftPlayers[i].GetComponent<dragPlayer>().playerNum;
            tmpPos = systemDataScript.leftPlayers[i].GetComponent<dragPlayer>().playerPlayPos;
            systemDataScript.leftPlayers[i].GetComponent<dragPlayer>().playerName = systemDataScript.rightPlayers[i].GetComponent<dragPlayer>().playerName;
            systemDataScript.leftPlayers[i].GetComponent<dragPlayer>().playerNum = systemDataScript.rightPlayers[i].GetComponent<dragPlayer>().playerNum;
            systemDataScript.leftPlayers[i].GetComponent<dragPlayer>().playerPlayPos = systemDataScript.rightPlayers[i].GetComponent<dragPlayer>().playerPlayPos;
            systemDataScript.rightPlayers[i].GetComponent<dragPlayer>().playerName = tmpName;
            systemDataScript.rightPlayers[i].GetComponent<dragPlayer>().playerNum = tmpNum;
            systemDataScript.rightPlayers[i].GetComponent<dragPlayer>().playerPlayPos = tmpPos;
            systemDataScript.rightPlayers[i].GetComponent<dragPlayer>().updata[0] = false;
            systemDataScript.leftPlayers[i].GetComponent<dragPlayer>().updata[0] = false;
        }
        int tmpID;
        tmpID =  systemDataScript.leftTeamNum[0];
        systemDataScript.leftTeamNum[0] = systemDataScript.rightTeamNum[0];
        systemDataScript.rightTeamNum[0] = tmpID;
    }     

    void setFormation(){
        int SetterPos = 0;
        if(whoServe == LEFT){ // 左方發球 左方正常 右方接球陣

            for(int i = 0; i < 6; i++){
                if(systemDataScript.leftPlayers[i].GetComponent<dragPlayer>().playerPlayPos == 2){
                    SetterPos = i;
                    break;
                }
            }
        }
        else{ // 右方發球 左方接球陣 右方正常
            for(int i = 0; i < 6; i++){
                if(systemDataScript.rightPlayers[i].GetComponent<dragPlayer>().playerPlayPos == 2){
                    SetterPos = i;
                    break;
                }
            }
        }
    }

}
