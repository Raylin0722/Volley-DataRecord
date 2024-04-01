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



    // Start is called before the first frame update
    void Awake(){
        saveData = new List<Data>(); // 儲存資料用
        UserName = UserData.Instance.UserName; //後面要連伺服器
        UserID = UserData.Instance.UserID;
        GameID = UserData.Instance.GameID;
        CallinitDB();
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
        //StartCoroutine(insertData());
        foreach(Data data in saveData){
            print(string.Format("formation: {0}\nround: {1}\nteamNum: {2}\nrole1: {3} role2: {4} role3: {5}\nsituation: {6}\n startPos: ({7}, {8}) endPos: ({9}, {10})\nscore: {11}\n", 
                                 data.formation, data.round, data.teamNum, data.role1, data.role2, data.role3, data.situation, data.startx, data.starty, data.endx, data.endy, data.score));
        }
        print(saveData.Count);
    }

    public IEnumerator insertData()
    {
        string data = JsonConvert.SerializeObject(saveData);
        string serverUrl = "https://volley.csie.ntnu.edu.tw/insertData";
        

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

}



