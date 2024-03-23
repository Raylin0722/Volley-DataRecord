using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class RefreshPoint : MonoBehaviour
{
    /*const int LEFT = 0, RIGHT = 1;
    [SerializeField] public GameObject serverData;
    [SerializeField] public GameObject[] Self_Player;
    [SerializeField] public GameObject[] Enemy_Player;
    [SerializeField] GameObject leftServe;
    [SerializeField] GameObject rightServe;
    [SerializeField] public static int whoWin = 0; //每回合勝方 -1,0,1
    [SerializeField] public static int setServe = 0; //每回合發球方 0,1
    [SerializeField] public static Vector3[] Self_Player_Default_Position = new Vector3[6];
    [SerializeField] public static Vector3[] Enemy_Player_Default_Position = new Vector3[6];
    SystemData allData;
    // Start is called before the first frame update
    void Start() {
        getDefaultPosition();
        changeServe(0);
        changeSideServe();
        FirstTimeOpen = true;
        allData = serverData.GetComponent<SystemData>();
    }
    void Update() {
        if(Setting.serve_change == 1) {
            changeSideServe();
            Setting.serve_change = 0;
        }
    }
    public void getDefaultPosition() {
        if(!FirstTimeOpen) {
            for(int i = 0;i < 6;i++) {
                Self_Player_Default_Position[i] = Self_Player[i].transform.position;
                Enemy_Player_Default_Position[i] = Enemy_Player[i].transform.position;
            }
        }
    }
    public void add_point() {
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        if(obj.tag == "LeftPoint"){
            (allData.point[LEFT])++;
            if((allData.point[LEFT]) < 10){
                allData.leftPointText.text = "0" + allData.point[LEFT].ToString();
            }
            else{
                allData.leftPointText.text = allData.point[LEFT].ToString();
            }
            changeServe(LEFT); //-1=left win
            whoWin = LEFT;
            add_score();
        }
        else{
            (allData.point[RIGHT])++;
            if(allData.point[RIGHT] < 10){
                allData.rightPointText.text = "0" + allData.point[RIGHT].ToString();
            }
            else{
                allData.rightPointText.text = allData.point[RIGHT].ToString();
            }
            changeServe(RIGHT); //1=right win
            whoWin = RIGHT;
            add_score();   
        }
    }
    public void add_score() {
        int winPoints = 25;
        if(allData.score[LEFT] + allData.score[RIGHT] >= 4)
            winPoints = 15;
        if(allData.point[LEFT] >= winPoints && ((allData.point[LEFT] - allData.point[RIGHT]) >= 2)){
            allData.score[LEFT]++;
            allData.leftScoreText.text = allData.score[LEFT].ToString();
            allData.leftScoreText.text = "00";
            allData.rightPointText.text = "00";
            allData.point[LEFT] = 0;
            allData.point[RIGHT] = 0;
            changeAll();
            changeSideServe();
            backToDefaultPosition(0);
        }
        else if(allData.point[RIGHT] >= winPoints && ((allData.point[RIGHT] - allData.point[LEFT]) >= 2)){
            allData.score[RIGHT]++;
            allData.rightScoreText.text = allData.score[RIGHT].ToString();
            allData.leftPointText.text = "00";
            allData.rightPointText.text = "00";
            allData.point[RIGHT] = 0;
            allData.point[LEFT] = 0;
            changeAll();
            changeSideServe();
            backToDefaultPosition(0);
        }
    }
    //whoWin -1,0,1
    //setServe 0,1
    public void changeServe(int whoWin) {
        if(whoWin == 0) {
            if(setServe == RIGHT)
                leftServe.SetActive(false);
            else if(setServe == LEFT)
                rightServe.SetActive(false);
        }
        if(whoWin == -1 && setServe == 1) { //self win and enemy serve
            leftServe.SetActive(true);
            rightServe.SetActive(false);
            setServe = 0;
            selfRotation();
        }
        if(whoWin == 1 && setServe == 0) { //enemy win and self serve
            leftServe.SetActive(false);
            rightServe.SetActive(true);
            setServe = 1;
            enemyRotation();
        }
    }
    public void changeSideServe() {
        if(Self_Point != 0 && Enemy_Point != 0)
            return;
        if(Setting.whoServe == 0){
            leftServe.SetActive(true);
            rightServe.SetActive(false);
            setServe = 0;
        }
        else if(Setting.whoServe == 1){
            leftServe.SetActive(false);
            rightServe.SetActive(true);
            setServe = 1;
        }
    }
    public void selfRotation() {
        backToDefaultPosition(-1);
        string text_tmp = Self_Player[0].gameObject.GetComponentInChildren<TextMeshPro>().text;
        for(int i = 0;i < 5;i++) {
            Self_Player[i].gameObject.GetComponentInChildren<TextMeshPro>().text = Self_Player[i+1].gameObject.GetComponentInChildren<TextMeshPro>().text;
        }
        Self_Player[5].gameObject.GetComponentInChildren<TextMeshPro>().text = text_tmp;

        string Num_tmp = Self_Player[0].gameObject.GetComponent<dragPlayer>().playerNum;
        string Name_tmp = Self_Player[0].gameObject.GetComponent<dragPlayer>().playerName;
        for(int i = 0;i < 5;i++) {
            Self_Player[i].gameObject.GetComponent<dragPlayer>().playerNum = Self_Player[i+1].gameObject.GetComponent<dragPlayer>().playerNum;
            Self_Player[i].gameObject.GetComponent<dragPlayer>().playerName = Self_Player[i+1].gameObject.GetComponent<dragPlayer>().playerName;
        }
        Self_Player[5].gameObject.GetComponent<dragPlayer>().playerNum = Num_tmp;
        Self_Player[5].gameObject.GetComponent<dragPlayer>().playerName = Name_tmp;
    }
    public void enemyRotation() {
        backToDefaultPosition(1);
        string text_tmp = Enemy_Player[0].gameObject.GetComponentInChildren<TextMeshPro>().text;
        for(int i = 0;i < 5;i++) {
            Enemy_Player[i].gameObject.GetComponentInChildren<TextMeshPro>().text = Enemy_Player[i+1].gameObject.GetComponentInChildren<TextMeshPro>().text;
        }
        Enemy_Player[5].gameObject.GetComponentInChildren<TextMeshPro>().text = text_tmp;

        string Num_tmp = Enemy_Player[0].gameObject.GetComponent<dragPlayer>().playerNum;
        string Name_tmp = Enemy_Player[0].gameObject.GetComponent<dragPlayer>().playerName;
        for(int i = 0;i < 5;i++) {
            Enemy_Player[i].gameObject.GetComponent<dragPlayer>().playerNum = Enemy_Player[i+1].gameObject.GetComponent<dragPlayer>().playerNum;
            Enemy_Player[i].gameObject.GetComponent<dragPlayer>().playerName = Enemy_Player[i+1].gameObject.GetComponent<dragPlayer>().playerName;
        }
        Enemy_Player[5].gameObject.GetComponent<dragPlayer>().playerNum = Num_tmp;
        Enemy_Player[5].gameObject.GetComponent<dragPlayer>().playerName = Name_tmp;
    }
    public void backToDefaultPosition(int side) { //-1,0,1
        if(side == -1 || side == 0) {
            for(int i = 0;i < 6;i++)
                Self_Player[i].transform.position = Self_Player_Default_Position[i];
        }
        if(side == 1 || side == 0) {
            for(int i = 0;i < 6;i++)
                Enemy_Player[i].transform.position = Enemy_Player_Default_Position[i];
        }
    }
    public static int change_all = 0;
    public void changeAll() {
        string teamName = SaveAndLoadName.TeamName[0];
        SaveAndLoadName.TeamName[0] = SaveAndLoadName.TeamName[1];
        SaveAndLoadName.TeamName[1] = teamName;

        for(int i = 0;i < 12;i++) {
            string num_tmp = SaveAndLoadName.SelfPlayerInfo[i,0];
            SaveAndLoadName.SelfPlayerInfo[i,0] = SaveAndLoadName.EnemyPlayerInfo[i,0];
            SaveAndLoadName.EnemyPlayerInfo[i,0] = num_tmp;

            string name_tmp = SaveAndLoadName.SelfPlayerInfo[i,1];
            SaveAndLoadName.SelfPlayerInfo[i,1] = SaveAndLoadName.EnemyPlayerInfo[i,1];
            SaveAndLoadName.EnemyPlayerInfo[i,1] = name_tmp;
        }

        for(int i = 0;i < 6;i++) {
            string playerText_tmp = Self_Player[i].gameObject.GetComponentInChildren<TextMeshPro>().text;
            Self_Player[i].gameObject.GetComponentInChildren<TextMeshPro>().text = Enemy_Player[i].gameObject.GetComponentInChildren<TextMeshPro>().text;
            Enemy_Player[i].gameObject.GetComponentInChildren<TextMeshPro>().text = playerText_tmp;

            string playerNum_tmp = Self_Player[i].gameObject.GetComponent<dragPlayer>().playerNum;
            Self_Player[i].gameObject.GetComponent<dragPlayer>().playerNum = Enemy_Player[i].gameObject.GetComponent<dragPlayer>().playerNum;
            Enemy_Player[i].gameObject.GetComponent<dragPlayer>().playerNum = playerNum_tmp;

            string playerName_tmp = Self_Player[i].gameObject.GetComponent<dragPlayer>().playerName;
            Self_Player[i].gameObject.GetComponent<dragPlayer>().playerName = Enemy_Player[i].gameObject.GetComponent<dragPlayer>().playerName;
            Enemy_Player[i].gameObject.GetComponent<dragPlayer>().playerName = playerName_tmp;
        }

        string teamName_tmp = Self_Team_Name.text;
        Self_Team_Name.text = Enemy_Team_Name.text;
        Enemy_Team_Name.text = teamName_tmp;

        int teamScore = allData.score[LEFT];
        allData.score[LEFT] = Enemy_Score;
        Enemy_Score = teamScore;

        string teamScore_tmp = allData.leftScoreText.text;
        allData.leftScoreText.text = Enemy_Score_Text.text;
        Enemy_Score_Text.text = teamScore_tmp;

        change_all = 1;
    }*/

    const int LEFT = 0, RIGHT = 1;
    public int whoServe, startServe, reWhoServe,  reClick;
    [SerializeField] GameObject serverData;
    [SerializeField] GameObject leftServeTag;
    [SerializeField] GameObject rightServeTag;
    private SystemData serverDataScript;

    void Start(){
        serverDataScript = serverData.GetComponent<SystemData>();
        whoServe = UserData.Instance.whoServe; // 跟伺服器拿資料
        startServe = whoServe; // 跟伺服器拿資料
    }

    void addPoint(){ // 小比分加分
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        if(obj.tag == "LeftPoint"){
            serverDataScript.point[LEFT]++;
            reClick = LEFT;
            reWhoServe = whoServe;
            if(whoServe == RIGHT){
                rightServeTag.SetActive(false);
                leftServeTag.SetActive(true);
                whoServe = LEFT;
                rotate(LEFT);
            }
            serverDataScript.leftPointText.text = serverDataScript.point[LEFT] > 10 
                ? serverDataScript.point[LEFT].ToString() : "0" + serverDataScript.point[LEFT].ToString();
        }
        else if(obj.tag == "RightPoint"){
            serverDataScript.point[RIGHT]++;
            reClick = RIGHT;
            reWhoServe = whoServe;
            if(whoServe == LEFT){
                leftServeTag.SetActive(false);
                rightServeTag.SetActive(true);
                whoServe = RIGHT;
                rotate(RIGHT);
            }
        }
        serverDataScript.rightPointText.text = serverDataScript.point[RIGHT] > 10 
                ? serverDataScript.point[RIGHT].ToString() : "0" + serverDataScript.point[RIGHT].ToString();

    }
    void addScore(){ // 大比分加分 插入資料再判斷
        int endPoint = 25;
        bool clear = false;
        if(serverDataScript.score[LEFT] + serverDataScript.score[RIGHT] >= 4)
            endPoint = 15;
        
        if(serverDataScript.point[LEFT] >= endPoint && serverDataScript.point[LEFT] - serverDataScript.point[RIGHT] >= 2){
            serverDataScript.score[LEFT]++;
            clear = true;
        }
        else if(serverDataScript.point[RIGHT] >= endPoint && serverDataScript.point[RIGHT] - serverDataScript.point[LEFT] >= 2){
            serverDataScript.score[RIGHT]++;
            clear = true;
        }
        if(clear){
            serverDataScript.point[LEFT] = 0;
            serverDataScript.point[RIGHT] = 0;
            serverDataScript.leftPointText.text = "00";
            serverDataScript.rightPointText.text = "00";
            serverDataScript.leftScoreText.text = serverDataScript.score[LEFT].ToString();
            serverDataScript.rightScoreText.text = serverDataScript.score[RIGHT].ToString();
        }
        
    }
    void rotate(int side){ // 輪轉
        GameObject tmp;
        Vector2 tmpPos;
        if(side == LEFT){
            tmp = serverDataScript.leftPlayers[0];
            tmpPos = serverDataScript.leftPLayersPos[0];
            for(int i = 0; i < 5; i++){
                serverDataScript.leftPlayers[i] = serverDataScript.leftPlayers[i + 1];
                serverDataScript.leftPLayersPos[i] = serverDataScript.leftPLayersPos[i + 1];
            }
            serverDataScript.leftPlayers[5] = tmp;
            serverDataScript.leftPLayersPos[5] = tmpPos;
        }
        else if(side == RIGHT){
            tmp = serverDataScript.rightPlayers[0];
            tmpPos = serverDataScript.rightPlayersPos[0];
            for(int i = 0; i < 5; i++){
                serverDataScript.rightPlayers[i] = serverDataScript.rightPlayers[i + 1];
                serverDataScript.rightPlayersPos[i] = serverDataScript.rightPlayersPos[i + 1];
            }
            serverDataScript.rightPlayers[5] = tmp;
            serverDataScript.rightPlayersPos[5] = tmpPos;
        }
    }
    void changeSideServe(){ // 局換發
        if((serverDataScript.score[LEFT] + serverDataScript.score[RIGHT]) % 2 != 0)
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
    void back(){ // 回復比分
        if(reClick == LEFT){
            serverDataScript.point[LEFT]--;
            whoServe = reWhoServe;
        }
        else if(reClick == RIGHT){
            serverDataScript.point[RIGHT]--;
            whoServe = reWhoServe;
        }
    }
    void LRChange(){
        
    }     
}
