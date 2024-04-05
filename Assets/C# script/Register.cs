using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using TMPro;
using System.Text;
using System;

public class Register : MonoBehaviour
{
    public InputField accountField;
    public InputField passwordField;
    public InputField repasswordField;
    public InputField TeamField;
    public Button submitButton;
    public Text WarnMessage;
    public Text TeamWarnMessage;
    public GameObject MainCanvas;
    public GameObject TeamCanvas;

    public class dataReturn{
        public bool success;
        public int situation;
        public string UserName;
        public int UserID;
        public int numOfGame;
        public int numOfPlayer;
    }
    public class TeamReturn{
        public bool success;
        public int situation;
        public int TeamID;
        public string ec;
    }

    // Start is called before the first frame update
    void Start()
    {
        WarnMessage.text = "帳號跟密碼: 限數字英文字母大小寫 長度8-16";
    }

    public void CallRegisterUser(){
        if(string.IsNullOrEmpty(accountField.text))
            WarnMessage.text = "帳號欄不可為空!";   
        else if(accountField.text.Length < 8)
            WarnMessage.text = "帳號長度必須大於8!";
        else if(string.IsNullOrEmpty(passwordField.text))
            WarnMessage.text = "密碼欄不可為空!";
        else if(string.IsNullOrEmpty(repasswordField.text))
            WarnMessage.text = "重複密碼欄位不可為空!";
        else if(passwordField.text.Length < 8)
            WarnMessage.text = "密碼長度需要大於8!";
        else if(!string.Equals(passwordField.text, repasswordField.text, StringComparison.Ordinal))
            WarnMessage.text = "密碼不相符請重新輸入!";
        else{
            Regex regex = new Regex("^[a-zA-Z0-9]+$");
            if(!regex.IsMatch(accountField.text))
                WarnMessage.text = "帳號只能有英文大小寫數字!";
            else if(!regex.IsMatch(passwordField.text))
                WarnMessage.text = "密碼只能有英文大小寫數字!";
            else
                StartCoroutine(RegisterUser());
        }
    }
    public IEnumerator RegisterUser(){

        string hashPwd = CalculateSHA256Hash(passwordField.text);
        Debug.Log(hashPwd);
        WWWForm form = new WWWForm();
        form.AddField("account", accountField.text);
        form.AddField("password", hashPwd);

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/register", form);
        
        dataReturn result = new dataReturn();

        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            print(response);
            result = JsonUtility.FromJson<dataReturn>(response);
            if(result.success == true){
                UserData.Instance.UserName = result.UserName;
                UserData.Instance.UserID = result.UserID;
                UserData.Instance.numOfGame = result.numOfGame;
                UserData.Instance.numOfPlayer = result.numOfPlayer;
            }
        }
        else{
            result.success = false;
            result.situation = -5;
        }
        // 0 成功 -1 使用者已存在 -2 資料庫錯誤 -3 帳號格式不符

        switch (result.situation){
            case 0:
                Debug.Log("成功!");
                WarnMessage.text = "成功!";
                MainCanvas.SetActive(false);
                TeamCanvas.SetActive(true);
                break;
            case -1:
                Debug.Log("傳遞參數錯誤!");
                WarnMessage.text = "系統錯誤!";
                break;
            case -2:
                Debug.Log("資料庫錯誤");
                WarnMessage.text = "資料庫錯誤!";
                break;
            case -3:
                Debug.Log("帳號格式不符");
                WarnMessage.text = "帳號格式不符!";
                break;
            case -4:
                Debug.Log("帳號已存在");
                WarnMessage.text = "帳號已存在!";
                break;
            case -5:
                Debug.Log("request未成功");
                WarnMessage.text = "request未成功!";
                break;
        }

        

    }
    private string CalculateSHA256Hash(string input)
    {
        using (SHA256 sha256 = new SHA256Managed())
        {
            // 将输入字符串转换为字节数组
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            // 计算散列
            byte[] hashBytes = sha256.ComputeHash(inputBytes);

            // 将字节数组转换为十六进制字符串
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }

    public void GoToMain(){
        GameObject[] dontDestroyObjects = GameObject.FindGameObjectsWithTag("DontDestroy");
        foreach (GameObject obj in dontDestroyObjects)
            Destroy(obj);
        SceneManager.LoadScene("StartMenu");
        
    }

    public void CallSetTeam(){
        if(string.IsNullOrEmpty(TeamField.text))
            TeamWarnMessage.text = "名稱不可為空!";  
        else{
            string pattern = @"^[a-zA-Z0-9\u4e00-\u9fa5]*$"; // 匹配字母、数字和中文
            if(TeamField.text.Length > 10)
                TeamWarnMessage.text = "隊伍名稱長度需要小於10!";
            else if (!Regex.IsMatch(TeamField.text, pattern))
                TeamWarnMessage.text = "隊伍名稱只能輸入中文字 英文字母 數字!";
            else{
                TeamWarnMessage.text = "";
                StartCoroutine(SetTeam());
            }
        } 
    }

    public IEnumerator SetTeam(){
        WWWForm form = new WWWForm();
        form.AddField("UserID", UserData.Instance.UserID);
        form.AddField("TeamName", TeamField.text);

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/SetTeam", form);

        TeamReturn result = new TeamReturn();

        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            print(response);
            result = JsonUtility.FromJson<TeamReturn>(response);
            if(result.success == true)
                UserData.Instance.TeamID = result.TeamID;
        }
        else{
            result.success = false;
            result.situation = -6;
        }

        switch (result.situation){
            case 0:
                Debug.Log("成功!");
                TeamWarnMessage.text = "成功!";
                SceneManager.LoadScene("GameSelect");
                break;
            case -2:
                Debug.Log("帳號錯誤!");
                TeamWarnMessage.text = "帳號錯誤!";
                break;
            case -3:
                Debug.Log("隊伍名稱重複!");
                TeamWarnMessage.text = "隊伍名稱重複!";
                break;
            case -4:
                Debug.Log("資料庫錯誤!");
                TeamWarnMessage.text = "資料庫錯誤!";
                break;
            case -5:
                Debug.Log(result.ec);
                TeamWarnMessage.text = result.ec;
                break;
            case -6:
                Debug.Log("request未成功");
                TeamWarnMessage.text = "連線失敗!";
                break;
        }
    }

}
