using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections;



public class MyScript : MonoBehaviour
{
    [SerializeField] GameObject text1;
    [SerializeField] GameObject text2;
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
        RectTransform text1Pos = text1.GetComponent<RectTransform>();
        RectTransform text2Pos = text2.GetComponent<RectTransform>();
        text2Pos.anchoredPosition = new Vector2(text1Pos.anchoredPosition.x + 168f, text1Pos.anchoredPosition.y);
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
