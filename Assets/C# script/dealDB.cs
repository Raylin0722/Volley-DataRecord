using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data;
using Mono.Data.Sqlite;
using System;
using UnityEngine.Networking;
using Newtonsoft.Json;


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
        public string role;
        public string attackblock, catchblock;
        public int score, situation;

        public Data(string formation,  int round, string role, string attackblock,
                    string catchblock, int situation, int score){
            this.formation = formation;
            this.round = round;
            this.role = role;
            this.attackblock = attackblock;
            this.catchblock = catchblock;
            this.situation = situation;
            this.score = score;
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
        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/initDB", form);
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

        
        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/displayData", form);
        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            showData = JsonConvert.DeserializeObject<List<Data>>(response);
        }
    }

    public void CallInsertData(){
        StartCoroutine(insertData());
    }

    public IEnumerator insertData()
    {
        string data = JsonConvert.SerializeObject(saveData);
        string serverUrl = "http://127.0.0.1:5000/insertData";
        

        UnityWebRequest www = UnityWebRequest.Post($"http://127.0.0.1:5000/insertData?GameID={GameID}&UserID={UserID}", new WWWForm());
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
    
    public void initPlayer(){
        
    }
}



