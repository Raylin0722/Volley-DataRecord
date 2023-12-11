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
    public List<int> UserPlayerID;
    public List<string> UserPlayerName;
    public List<int> UserPlayerNumber;


    // Start is called before the first frame update
    void Awake(){
        saveData = new List<Data>(); // 儲存資料用
        UserName = UserData.Instance.UserName; //後面要連伺服器
        UserID = UserData.Instance.UserID;
        CallUpdateUserData();
    }

    public void CallinitDB(string gameName){
        StartCoroutine(initDB());
    }

    IEnumerator initDB(){
        WWWForm form = new WWWForm();
        form.AddField("gameName", gameName);
        form.AddField("account", account);

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/initDB", form);
        yield return www.SendWebRequest();

        Return result = new Return();

        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            result = JsonUtility.FromJson<Return>(response);
        }
        else
            result.success = false;

        yield return result;

    }

    public void CallDisplayData(){
        StartCoroutine(displayData());
    }

    IEnumerator displayData(){
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

    IEnumerator insertData()
    {
        string data = JsonConvert.SerializeObject(saveData);
        string serverUrl = "http://127.0.0.1:5000/insertData";

        // 创建 UnityWebRequest 对象
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(serverUrl, ""))
        {
            // 设置请求头部为 application/json
            www.SetRequestHeader("Content-Type", "application/json");

            // 将 JSON 数据放入请求的数据体
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(data);
            www.uploadHandler = new UploadHandlerRaw(jsonBytes);

            // 发送网络请求
            yield return www.SendWebRequest();

            // 处理响应
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("POST request successful");
            }
            else
            {
                Debug.LogError($"Error: {www.error}");
            }
        }

        saveData.Clear();
    }
    
    public void CallUpdateUserData(){
        StartCoroutine(UpdateUserData());
    }

    public IEnumerator UpdateUserData(){

        WWWForm form = new WWWForm();
        form.AddField("account", UserName);
        form.AddField("UserID", UserID);

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/UpdateUserData", form);

        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            print(response);
            UserPlayerID = new List<int>();
            UserPlayerName = new List<string>();
            UserPlayerNumber = new List<int>();

            ServerToUserData userRetuen = JsonUtility.FromJson<ServerToUserData>(response);
            Debug.Log(response);
            if(userRetuen.success == true){
                for(int i = 0; i < userRetuen.UserPlayerID.Count; i++){
                    UserPlayerID.Add(userRetuen.UserPlayerID[i]);
                    UserPlayerNumber.Add(userRetuen.UserPlayerNumber[i]);
                    UserPlayerName.Add(userRetuen.UserPlayerName[i]);
                }
                Debug.Log("Success!");
            }
            else if(userRetuen.success == false){
                switch (userRetuen.situation){
                    case -1:
                        Debug.Log("參數錯誤");
                        break;
                    case -2:
                        Debug.Log("資料庫錯誤");
                        break;
                    case -3:
                        Debug.Log("帳號不存在");
                        break;
                }
            }
        }
        
    }


}



