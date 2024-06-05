using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class EndRecord : MonoBehaviour
{
    public GameObject Setting;
    public GameObject EndRecordScene;
    
    public void GoToEndRecord(){
        Setting.SetActive(false);
        EndRecordScene.SetActive(true);
    }

    public void CallStopRecord(){
        int UserID = UserData.Instance.UserID;
        int GameID = UserData.Instance.GameID;
        StartCoroutine(StopRecord(UserID, GameID));
        return;
    }

    public class ReqReturn{
        public string ec;
        public bool success;
    }

    public IEnumerator StopRecord(int UserID, int GameID){
        WWWForm form = new WWWForm();
        form.AddField("UserID", UserID);
        form.AddField("GameID", GameID);

        
        UnityWebRequest www = UnityWebRequest.Post("https://volley.csie.ntnu.edu.tw/EndRecord", form);
        yield return www.SendWebRequest();

        ReqReturn result = new ReqReturn();
        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            result = JsonUtility.FromJson<ReqReturn>(response);
            if(result.success){
                UserData tmp = new UserData();
                tmp.UserName = UserData.Instance.UserName;
                tmp.UserID = UserData.Instance.UserID;
                tmp.TeamID = UserData.Instance.TeamID;
                tmp.numOfGame = UserData.Instance.numOfGame;
                tmp.numOfPlayer = UserData.Instance.numOfPlayer;
                UserData.Instance = tmp;
                
                SceneManager.LoadScene("GameSelect");
            }
            else{
                print("Error!");
            }
        }
        else{
            print("Error!");
        }
    }
}
