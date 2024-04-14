using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using TMPro;
using System.Text;

public class Login : MonoBehaviour
{
    public InputField accountField;
    public InputField passwordField;
    public Button submitButton;
    public Text WarnMessage;

    public class dataReturn{
        public bool success;
        public int situation;
        public string UserName;
        public int UserID;
        public int TeamID;
        public string TeamName;
        public int numOfGame;
        public int numOfPlayer;
        public string ec;
    }


    // Start is called before the first frame update
    void Start(){
        WarnMessage.text = "";
    }
    public void CallLoginUser(){
        if(string.IsNullOrEmpty(accountField.text))
            WarnMessage.text = "Account can't be empty!";   
        else if(string.IsNullOrEmpty(passwordField.text))
            WarnMessage.text = "Password can't be empty!";
        else
            StartCoroutine(LoginUser());
    }

    public IEnumerator LoginUser(){

        string hashPwd = CalculateSHA256Hash(passwordField.text);

        WWWForm form = new WWWForm();
        form.AddField("account", accountField.text);
        form.AddField("password", hashPwd);

        UnityWebRequest www = UnityWebRequest.Post("https://volley.csie.ntnu.edu.tw/login", form);
        
        dataReturn result = new dataReturn();

        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            Debug.Log(response);
            result = JsonUtility.FromJson<dataReturn>(response);
            if(result.success == true){
                UserData.Instance.UserName = result.UserName;
                UserData.Instance.UserID = result.UserID;
                UserData.Instance.TeamID = result.TeamID;
                UserData.Instance.numOfGame = result.numOfGame;
                UserData.Instance.numOfPlayer = result.numOfPlayer;
                UserData.Instance.UserTeamName = result.TeamName;
            }
        }
        else{
            result.success = false;
            result.situation = -7;
        }
        // 0 成功 -1 資料庫錯誤 -2 密碼錯誤 -3 request未成功 

        switch (result.situation){
            case 0:
                Debug.Log("success!");
                WarnMessage.text = "Success!";
                SceneManager.LoadScene("GameSelect");
                break;
            case -1:
                Debug.Log(result.ec);
                WarnMessage.text = "資料庫錯誤(參數)!";
                break;
            case -2:
                Debug.Log(result.ec);
                WarnMessage.text = "資料庫錯誤(users)!";
                break;
            case -3:
                Debug.Log(result.ec);
                WarnMessage.text = "資料庫錯誤(Team)!";
                break;
            case -4:
                Debug.Log(result.ec);
                WarnMessage.text = "密碼錯誤!";
                break;
            case -5:
                Debug.Log(result.ec);
                WarnMessage.text = "資料庫錯誤(exception)!";
                break;
            case -6:
                Debug.Log(result.ec);
                WarnMessage.text = "帳號密碼錯誤!";
                break;
            case -7:
                Debug.Log("連線錯誤!");
                WarnMessage.text = "連線錯誤!";
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
    public void Test(){
        StartCoroutine(LoadSceneAsync("MainScene"));
    }
    IEnumerator LoadSceneAsync(string sceneName)
{
    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
    while (!asyncLoad.isDone)
    {
        yield return null;
    }
}
}
