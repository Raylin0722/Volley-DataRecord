using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data;
using Mono.Data.Sqlite;
using System;


public class dealDB : MonoBehaviour
{
    [SerializeField] public TextMeshPro[] WPlayer;
    [SerializeField] public TextMeshPro[] BPlayer;
    
    public string formation;

    public const int CATCH = 0;
    public const int SERVE = 1;
    public const int ATTACK = 2;
    public const int BLOCK = 3;
    
    public string gameName;
    public string account;
    public string hashPasswd;
    public struct Data{
        public string formation;
        public int index;
        public int round;
        public string role;
        public string attackblock, catchblock;
        public int score, situation;

        public Data(string formation, int index, int round, string role, string attackblock,
                    string catchblock, int situation, int score){
            this.formation = formation;
            this.index = index;
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
    }
    public Data[] saveData; // 儲存資料用
    public int[] saveIndex; // 儲存資料用
    public Data[] showData; // 顯示資料用

    // Start is called before the first frame update
    void Awake(){
        saveData = new Data[300]; // 儲存資料用
        saveIndex = new int[1]; // 儲存資料用
    }

    public void CallinitDB(string gameName){
        
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

    public void displayData(){
        WWWForm form = new WWWForm();
        form.AddField("gameName", gameName);
        form.AddField("account", account);

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/displayData", form);
        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            showData = JsonUtility.FromJson<Data[]>(response);
        }
    }

    public void insertData(){
        
    }

}



