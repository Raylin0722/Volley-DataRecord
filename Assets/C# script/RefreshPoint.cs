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

    
    void Start(){
        systemDataScript = serverData.GetComponent<SystemData>();
        whoServe = UserData.Instance.whoServe; // 跟伺服器拿資料
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
            print(systemDataScript.point[LEFT]);
            systemDataScript.point[LEFT]++;
            print(systemDataScript.point[LEFT]);
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
        bool clear = false;
        if(systemDataScript.score[LEFT] + systemDataScript.score[RIGHT] >= 4)
            endPoint = 15;
        
        if(systemDataScript.point[LEFT] >= endPoint && systemDataScript.point[LEFT] - systemDataScript.point[RIGHT] >= 2){
            systemDataScript.score[LEFT]++;
            clear = true;
        }
        else if(systemDataScript.point[RIGHT] >= endPoint && systemDataScript.point[RIGHT] - systemDataScript.point[LEFT] >= 2){
            systemDataScript.score[RIGHT]++;
            clear = true;
        }
        if(clear){
            systemDataScript.point[LEFT] = 0;
            systemDataScript.point[RIGHT] = 0;
            systemDataScript.leftPointText.text = "00";
            systemDataScript.rightPointText.text = "00";
            systemDataScript.leftScoreText.text = systemDataScript.score[LEFT].ToString();
            systemDataScript.rightScoreText.text = systemDataScript.score[RIGHT].ToString();
            LRChange();
        }
        
    }
    public void rotate(){ // 輪轉
        string tmpName, tmpNum;
        Vector2 tmpNamePos;
        if(reClick == LEFT && reWhoServe == RIGHT){
            tmpName = systemDataScript.leftPlayers[0].GetComponent<dragPlayer>().playerName;
            tmpNum = systemDataScript.leftPlayers[0].GetComponent<dragPlayer>().playerNum;
            //tmpNamePos = systemDataScript.leftPLayersPos[0];
            for(int i = 0; i < 5; i++){
                systemDataScript.leftPlayers[i].GetComponent<dragPlayer>().playerName = systemDataScript.leftPlayers[i + 1].GetComponent<dragPlayer>().playerName;
                systemDataScript.leftPlayers[i].GetComponent<dragPlayer>().playerNum = systemDataScript.leftPlayers[i + 1].GetComponent<dragPlayer>().playerNum;
                //systemDataScript.leftPLayersPos[i] = systemDataScript.leftPLayersPos[i + 1];
            }
            systemDataScript.leftPlayers[5].GetComponent<dragPlayer>().playerName = tmpName;
            systemDataScript.leftPlayers[5].GetComponent<dragPlayer>().playerNum = tmpNum;
            //systemDataScript.leftPLayersPos[5] = tmpNamePos;
        }
        else if(reClick == RIGHT && reWhoServe == LEFT){
            tmpName = systemDataScript.rightPlayers[0].GetComponent<dragPlayer>().playerName;
            tmpNum = systemDataScript.rightPlayers[0].GetComponent<dragPlayer>().playerNum;
            //tmpNamePos = systemDataScript.rightPlayersPos[0];
            for(int i = 0; i < 5; i++){
                systemDataScript.rightPlayers[i].GetComponent<dragPlayer>().playerName = systemDataScript.rightPlayers[i + 1].GetComponent<dragPlayer>().playerName;
                systemDataScript.rightPlayers[i].GetComponent<dragPlayer>().playerNum = systemDataScript.rightPlayers[i + 1].GetComponent<dragPlayer>().playerNum;
                //systemDataScript.rightPlayersPos[i] = systemDataScript.rightPlayersPos[i + 1];
            }
            systemDataScript.rightPlayers[5].GetComponent<dragPlayer>().playerName = tmpName;
            systemDataScript.rightPlayers[5].GetComponent<dragPlayer>().playerNum = tmpNum;
            //systemDataScript.rightPlayersPos[5] = tmpPos;
        }
        for(int i = 0; i < 6; i++){
            systemDataScript.leftPlayers[i].GetComponent<dragPlayer>().updata[0] = false;
            systemDataScript.rightPlayers[i].GetComponent<dragPlayer>().updata[0] = false;
            systemDataScript.leftPlayers[i].SetActive(true);
            systemDataScript.rightPlayers[i].SetActive(true);
        }
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

        }
        else if(reClick == RIGHT){
            systemDataScript.point[RIGHT]--;
            whoServe = reWhoServe;
            if(reWhoServe == LEFT){
                leftServeTag.SetActive(true);
                rightServeTag.SetActive(false);
            }
        }
    }
    void LRChange(){

        Text tmpText;
        tmpText = systemDataScript.leftTeamName;
        systemDataScript.leftTeamName = systemDataScript.rightTeamName;
        systemDataScript.rightTeamName = tmpText;

        int tmpPoint;
        tmpPoint = systemDataScript.point[0];
        systemDataScript.point[0] = systemDataScript.point[1];
        systemDataScript.point[1] = tmpPoint;

        tmpPoint = systemDataScript.score[0];
        systemDataScript.score[0] = systemDataScript.score[1];
        systemDataScript.score[1] = tmpPoint;

        tmpText = systemDataScript.leftScoreText;
        systemDataScript.leftScoreText = systemDataScript.rightScoreText;
        systemDataScript.rightScoreText = tmpText;

        tmpText = systemDataScript.leftPointText;
        systemDataScript.leftPointText = systemDataScript.rightPointText;
        systemDataScript.rightPointText = tmpText;

        whoServe = startServe;

        GameObject tmpPlayer;

        for(int i = 0; i < 5; i++){
            
        }


    }     
}
