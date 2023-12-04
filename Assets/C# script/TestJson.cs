using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections;



public class MyScript : MonoBehaviour
{

    public struct testData
    {
        public string formation;
        public int index;
        public int round;
        public string role;
        public string attackblock, catchblock;
        public int score, situation;

        public testData(string formation, int index, int round, string role, string attackblock,
                    string catchblock, int situation, int score)
        {
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
    void Start()
    {
        // 创建 Data 数组
        testData[] dataArray = new testData[]
        {
            new testData("Formation1", 1, 2, "Role1", "Attack1", "Catch1", 3, 100),
            new testData("Formation2", 2, 3, "Role2", "Attack2", "Catch2", 4, 150)
            // 可以添加更多 Data 对象
        };

        // 将 Data 数组序列化为 JSON 字符串
        string jsonData = JsonConvert.SerializeObject(dataArray);

        // 发送 POST 请求
        StartCoroutine(SendPostRequest(jsonData));
    }

    IEnumerator SendPostRequest(string jsonData)
    {
        // 指定 Flask 服务器的 URL
        string serverUrl = "http://127.0.0.1:5000/insertData";

        // 创建 UnityWebRequest 对象
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(serverUrl, ""))
        {
            // 设置请求头部为 application/json
            www.SetRequestHeader("Content-Type", "application/json");

            // 将 JSON 数据放入请求的数据体
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
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
    }
}
