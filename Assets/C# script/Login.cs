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
    public TMP_InputField accountField;
    public TMP_InputField passwordField;
    public Button submitButton;
    public TMP_Text WarnMessage;

    public class dataReturn{
        public bool success;
        public int situation;
        public string UserName;
        public int UserID;
        public int numOfGame;
        public int numOfPlayer;
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

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/login", form);
        
        dataReturn result = new dataReturn();

        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            Debug.Log(response);
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
            result.situation = -3;
        }
        // 0 成功 -1 資料庫錯誤 -2 密碼錯誤 -3 request未成功 

        switch (result.situation){
            case 0:
                Debug.Log("success!");
                WarnMessage.text = "Success!";
                SceneManager.LoadScene("GameSelect");
                break;
            case -1:
                Debug.Log("帳號不存在");
                WarnMessage.text = "Account doesn't exist!";
                break;
            case -2:
                Debug.Log("密碼錯誤");
                WarnMessage.text = "Password error!";
                break;
            case -3:
                Debug.Log("request未成功");
                WarnMessage.text = "Server down!";
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
        SceneManager.LoadScene("StartMenu");
    }

}
