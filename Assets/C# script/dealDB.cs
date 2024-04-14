using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data;
using Mono.Data.Sqlite;
using System;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;


public class dealDB : MonoBehaviour
{
    [SerializeField] public TextMeshPro[] WPlayer;
    [SerializeField] public TextMeshPro[] BPlayer;
    
    public class ServerToUserData{
        public bool success;
        public int situation;
        public List<int> UserPlayerID;
        public List<string> UserPlayerName;
        public List<int> UserPlayerNumber;
        public List<int> UserGameID;
        public List<string> UserGameDate;
        public List<string> UserGameName;
    }
    public struct Data{
        public string formation;
        public int round;
        public int teamNum;
        public int role1, role2, role3;
        public int score, situation;
        public int startx, starty;
        public int endx, endy;
        public int side;

        public Data(string formation, int round, List<GameObject> role, int teamNum,
                    int startx, int starty, int endx, int endy,
                    int situation, int score, int side){
            this.formation = formation;
            this.round = round;
            this.startx = startx;
            this.starty = starty;
            this.endx= endx;
            this.endy = endy;
            this.situation = situation;
            this.score = score;
            this.teamNum = teamNum;
            this.side = side;

            this.role1 = Int32.Parse(role[0].GetComponent<dragPlayer>().playerNum);
            if(role.Count == 2)
                this.role2 = Int32.Parse(role[1].GetComponent<dragPlayer>().playerNum);
            else
                this.role2 = -1;
            if(role.Count == 3)
                this.role3 = Int32.Parse(role[2].GetComponent<dragPlayer>().playerNum);
            else
                this.role3 = -1;
        }

    }
    public class Return{
        public bool success;
        public int situation;
    }

    public string formation;

    public const int CATCH = 0;
    public const int SERVE = 1;
    public const int ATTACK = 2;
    public const int BLOCK = 3;
    public const int SCORE = 4;
    
    public string gameName;
    public string account;
    public string hashPasswd;
    public List<Data> saveData; // 儲存資料用 
    public List<Data> showData; // 顯示資料用

    public int UserID;
    public int GameID;
    public string UserName;

    [SerializeField] GameObject insertBtn;
    private SystemData SystemScript;


    // Start is called before the first frame update
    void Awake(){
        saveData = new List<Data>(); // 儲存資料用
        UserName = UserData.Instance.UserName; //後面要連伺服器
        UserID = UserData.Instance.UserID;
        GameID = UserData.Instance.GameID;
        SystemScript = this.gameObject.GetComponent<SystemData>();
    }



    public void CallinitDB(){
        StartCoroutine(initDB());
    }

    public IEnumerator initDB(){
        WWWForm form = new WWWForm();
        form.AddField("UserID", UserID);
        form.AddField("GameID", GameID);
        UnityWebRequest www = UnityWebRequest.Post("https://volley.csie.ntnu.edu.tw/initDB", form);
        yield return www.SendWebRequest();

        Return result = new Return();
        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            result = JsonUtility.FromJson<Return>(response);
            if(result.success == false){
                switch (result.situation){
                    case -1:
                        Debug.Log("參數傳送錯誤!"); 
                        break;
                    case -2:
                        Debug.Log("資料庫錯誤!"); 
                        break;
                    case -3:
                        Debug.Log("比賽不存在!"); 
                        break;
                    case -4:
                        Debug.Log("帳號不存在!"); 
                        break;
                }
            }
            else{
                Debug.Log("Success!");
            }
        }
        else{
            Debug.Log("未連接到伺服器!");
        }

    }

    public void CallDisplayData(){
        StartCoroutine(displayData());
    }

    public IEnumerator displayData(){
        WWWForm form = new WWWForm();
        form.AddField("gameName", gameName);
        form.AddField("account", account);

        
        UnityWebRequest www = UnityWebRequest.Post("https://volley.csie.ntnu.edu.tw/displayData", form);
        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            showData = JsonConvert.DeserializeObject<List<Data>>(response);
        }
    }
    [SerializeField] GameObject content;
    public void CallInsertData(){
        //content.GetComponent<Text>().text = "";
        StartCoroutine(insertData());
        foreach(Data data in saveData){
            print(string.Format("formation: {0}\nround: {1}\nteamNum: {2}\nrole1: {3} role2: {4} role3: {5}\nsituation: {6}\n startPos: ({7}, {8}) endPos: ({9}, {10})\nscore: {11}\n", 
                                 data.formation, data.round, data.teamNum, data.role1, data.role2, data.role3, data.situation, data.startx, data.starty, data.endx, data.endy, data.score));
        }
        print(saveData.Count);
        saveData.Clear();
        insertBtn.GetComponent<Button>().interactable = false;
    }

    public IEnumerator insertData()
    {
        string data = JsonConvert.SerializeObject(saveData);
        
        UnityWebRequest www = UnityWebRequest.Post($"https://volley.csie.ntnu.edu.tw/insertData?GameID={GameID}&UserID={UserID}", new WWWForm());
        www.SetRequestHeader("Content-Type", "application/json");

        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(data);
        www.uploadHandler = new UploadHandlerRaw(jsonBytes);
        yield return www.SendWebRequest();

        Return result = new Return();
        if (www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            result = JsonUtility.FromJson<Return>(response);

            if(result.success == false){
                switch (result.situation){
                    case -1:
                        Debug.Log("參數傳送錯誤!"); 
                        break;
                    case -2:
                        Debug.Log("資料庫錯誤!"); 
                        break;
                    case -3:
                        Debug.Log("資料表不存在!");
                        break;
                    case -4:
                        Debug.Log("比賽不存在!"); 
                        break;
                    case -5:
                        Debug.Log("帳號不存在!"); 
                        break;
                }
            }
            else{
                Debug.Log("Success!");
                
            }
        }
        else{
            Debug.Log("未連接到伺服器!");
        }


        saveData.Clear();
    }

    public void CallGetPlayerCatchPos(){
        StartCoroutine(GetPlayerCatchPos());
        return;     
    }

    public class PosReturn {
        public bool success;
        public int situation;
        public string ec;
        public int PL1X, PL1Y;
        public int PL2X, PL2Y;
        public int PL3X, PL3Y;
        public int PL4X, PL4Y;
        public int PL5X, PL5Y;
        public int PL6X, PL6Y;
        public int PR1X, PR1Y;
        public int PR2X, PR2Y;
        public int PR3X, PR3Y;
        public int PR4X, PR4Y;
        public int PR5X, PR5Y;
        public int PR6X, PR6Y;
        
    }
    public IEnumerator GetPlayerCatchPos(){
        string sendFormation = "", sendL = "", sendR = "";
        for(int i = 0; i < 6; i++){
            sendL += ("L" + SystemScript.leftPlayers[i].GetComponent<dragPlayer>().playerNum + " ");
            sendR += ("R" + SystemScript.rightPlayers[i].GetComponent<dragPlayer>().playerNum + " ");
        }
        sendFormation = sendL + sendR;

        WWWForm form = new WWWForm();
        form.AddField("UserID", UserID);
        form.AddField("GameID", GameID);
        form.AddField("TeamL", SystemScript.leftTeamNum[0]);
        form.AddField("TeamR", SystemScript.rightTeamNum[0]);
        form.AddField("formation", sendFormation);
        
        UnityWebRequest www = UnityWebRequest.Post("https://volley.csie.ntnu.edu.tw/GetPlayerCatchPos", form);
        yield return www.SendWebRequest();

        PosReturn result = new PosReturn();
        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            result = JsonUtility.FromJson<PosReturn>(response);
            if(result.success == false){
                switch (result.situation){
                    case -1:
                    case -2:
                    case -3:
                    case -4:
                    case -5:
                    case -6:
                        print(result.ec);
                        break;
                }
            }
            else{
                ClickRecord tmp = this.gameObject.GetComponent<ClickRecord>();
                print("Success!");
                SystemScript.leftGamePos[0] = new Vector3(result.PL1X, result.PL1Y, 0);
                SystemScript.leftGamePos[1] = new Vector3(result.PL2X, result.PL2Y, 0);
                SystemScript.leftGamePos[2] = new Vector3(result.PL3X, result.PL3Y, 0);
                SystemScript.leftGamePos[3] = new Vector3(result.PL4X, result.PL4Y, 0);
                SystemScript.leftGamePos[4] = new Vector3(result.PL5X, result.PL5Y, 0);
                SystemScript.leftGamePos[5] = new Vector3(result.PL6X, result.PL6Y, 0);

                SystemScript.rightGamePos[0] = new Vector3(result.PR1X, result.PR1Y, 0);
                SystemScript.rightGamePos[1] = new Vector3(result.PR2X, result.PR2Y, 0);
                SystemScript.rightGamePos[2] = new Vector3(result.PR3X, result.PR3Y, 0);
                SystemScript.rightGamePos[3] = new Vector3(result.PR4X, result.PR4Y, 0);
                SystemScript.rightGamePos[4] = new Vector3(result.PR5X, result.PR5Y, 0);
                SystemScript.rightGamePos[5] = new Vector3(result.PR6X, result.PR6Y, 0);   
                for(int i = 0; i < 6; i++){
                    print(string.Format("PL{0} X: {1} Y: {2}", i + 1, SystemScript.leftGamePos[i].x, SystemScript.leftGamePos[i].y));
                    print(string.Format("PR{0} X: {1} Y: {2}", i + 1, SystemScript.rightGamePos[i].x, SystemScript.rightGamePos[i].y));
                }
                for(int i = 0; i < 6; i++){
                    Vector3 LTmp = new Vector3(), RTmp = new Vector3();
                    Vector3[] NetXY = tmp.NetLocateXY;
                    if(SystemScript.leftGamePos[i].x != -1 && SystemScript.leftGamePos[i].y != -1){
                        LTmp.x = (SystemScript.leftGamePos[i].x) * ((NetXY[3].x - NetXY[2].x) / 500) + (NetXY[2].x);
                        LTmp.y = -((SystemScript.leftGamePos[i].y) * ((NetXY[2].y - NetXY[4].y) / 800) - (NetXY[2].y));
                        LTmp.z = NetXY[2].z;
                        SystemScript.leftGamePos[i] = new Vector3(LTmp.x, LTmp.y, LTmp.z);
                    }
                    if(SystemScript.rightGamePos[i].x != -1 && SystemScript.rightGamePos[i].y != -1){
                        RTmp.x = -((SystemScript.rightGamePos[i].x) * ((NetXY[3].x - NetXY[2].x) / 500) - (NetXY[5].x));
                        RTmp.y = (SystemScript.rightGamePos[i].y) * ((NetXY[2].y - NetXY[4].y) / 800) + (NetXY[5].y);
                        RTmp.z = NetXY[2].z;
                        SystemScript.rightGamePos[i] = new Vector3(RTmp.x, RTmp.y, RTmp.z);
                    }

                    
                    
                }
            }
        }
        else{
            Debug.Log("未連接到伺服器!");
        }

    }

}




