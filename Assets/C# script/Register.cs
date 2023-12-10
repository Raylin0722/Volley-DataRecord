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
    public TMP_InputField accountField;
    public TMP_InputField passwordField;
    public TMP_InputField repasswordField;
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
    void Start()
    {
        WarnMessage.text = "Account and passwords: English letters, numbers only, 8-16 characters";
    }

    public void CallRegisterUser(){
        if(string.IsNullOrEmpty(accountField.text))
            WarnMessage.text = "Account can't be empty!";   
        else if(accountField.text.Length < 8)
            WarnMessage.text = "Account length must be greater than 8!";
        else if(string.IsNullOrEmpty(passwordField.text))
            WarnMessage.text = "Password can't be empty!";
        else if(string.IsNullOrEmpty(repasswordField.text))
            WarnMessage.text = "Retype Password can't be empty!";
        else if(passwordField.text.Length < 8)
            WarnMessage.text = "Password length must be greater than 8!";
        else if(!string.Equals(passwordField.text, repasswordField.text, StringComparison.Ordinal))
            WarnMessage.text = "Please enter the same password in both fields!";
        else{
            Regex regex = new Regex("^[a-zA-Z0-9]+$");
            if(!regex.IsMatch(accountField.text))
                WarnMessage.text = "Account can only consist of uppercase and lowercase letters as well as numbers!";
            else if(!regex.IsMatch(passwordField.text))
                WarnMessage.text = "Password can only consist of uppercase and lowercase letters as well as numbers!";
            else
                StartCoroutine(RegisterUser());
        }
    }
    public IEnumerator  RegisterUser(){

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
            result.situation = -4;
        }
        // 0 成功 -1 使用者已存在 -2 資料庫錯誤 -3 帳號格式不符

        switch (result.situation){
            case 0:
                Debug.Log("success!");
                WarnMessage.text = "Success!";
                SceneManager.LoadScene("GameSelect");
                break;
            case -1:
                Debug.Log("帳號已存在");
                WarnMessage.text = "Account already exists!";
                break;
            case -2:
                Debug.Log("資料庫錯誤");
                WarnMessage.text = "Database error!";
                break;
            case -3:
                Debug.Log("帳號格式不符");
                WarnMessage.text = "Invalid account format";
                break;
            case -4:
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
