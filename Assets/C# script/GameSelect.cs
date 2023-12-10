using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;


public class GameSelect : MonoBehaviour
{
    public GameObject Game; // 你的Prefab
    public GameObject Player;
    public RectTransform GameContent; // ScrollView的Content Transform
    public RectTransform PlayerContent;
    public float prefabHeight = 100f; // 調整Prefab的高度
    public GameObject GameScrollview;
    public GameObject PlayerScrollview;
    public Text User;

    public GameObject Main;
    public GameObject AddPlayerObj;
    public GameObject AddGameObj; 

    public class PlayerData{
        public int PlayerID;
        public int PlayerNumber;
        public string PlyaerName;
    }
    public class GameData{
        public int GameID;
        public string GameDate;
        public string GameName;
    }

    public class ServerToUser{
        public List<PlayerData> PlayerList;
        public List<GameData> GameList;
    }

    public class Return{
        public bool success;
        public int situation;
    }

    public int numOfGame;
    public int numOfPlayer;
    public int UserID;
    public string UserName;
    public List<GameData> GameList;
    public List<PlayerData> PlayerList;

    private void Awake(){
        /*UserName = UserData.Instance.UserName; //後面要連伺服器
        numOfGame = UserData.Instance.numOfGame; // 後面要連伺服器
        numOfPlayer = UserData.Instance.numOfPlayer;
        UserID = UserData.Instance.UserID;
        User.text = "User: " + UserName;
        CallUpdateUserData();*/
    }
    private void Start()
    {
        Main.SetActive(true);
        AddPlayerObj.SetActive(false);
        AddPlayerObj.SetActive(false);
        // 初始化ScrollView的滾動位置
        GameScrollview.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
        PlayerScrollview.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;

        // 這裡演示創建3個Prefab的例子，實際上你可以根據需求生成不同數量的Prefab
        for (int i = 0; i < numOfGame; i++)
        {
            // 透過Instantiate生成Prefab
            GameObject newGame = Instantiate(Game, GameContent);
            GameObject newPlayer = Instantiate(Player, PlayerContent);

            Text[] GameTexts = newGame.GetComponentsInChildren<Text>();

            GameTexts[0].text = "2023/12/" + i;
            GameTexts[1].text = "Game" + i;

            Text[] PlayerTexts = newPlayer.GetComponentsInChildren<Text>();

            PlayerTexts[0].text =  (i + 1).ToString();
            PlayerTexts[1].text = "Player" + (i+1);

            // 設定Prefab的位置
            RectTransform rectTransform = newGame.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, -i * prefabHeight);
        }
        GameContent.sizeDelta = new Vector2(GameContent.sizeDelta.x, 0 * prefabHeight);
        PlayerContent.sizeDelta = new Vector2(PlayerContent.sizeDelta.x, 0 * prefabHeight);



    }

    public InputField PName;
    public InputField PNum;
    public InputField GName;
    public InputField GDate;
    public Text PWarning;
    public Text GWarning;
    public void CallAddPlayer(){

        Regex regexNum = new Regex("^[0-9]+$");
        Regex regexName = new Regex("^[a-zA-Z0-9_\\u4e00-\\u9fa5]+$");
        if(!regexNum.IsMatch(PNum.text)){

        }
        else{

        }
        int PNumIN = int.Parse(PNum.text);
        string PNameIn = PName.text;
        StartCoroutine(AddPlayer(PNumIN, PNameIn));
    }
    public void CallAddGame(){
        StartCoroutine(AddGame());
    }

    public void CallUpdateUserData(){
        StartCoroutine(UpdateUserData());
    }

    public void LogOut(){
        SceneManager.LoadScene("StartMenu");
    }

    public void ClickGame(){

    }

    public IEnumerator UpdateUserData(){

        WWWForm form = new WWWForm();
        form.AddField("account", UserName);
        form.AddField("id", UserID);

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/UpdateUserData", form);

        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            
            GameList = new List<GameData>();
            PlayerList = new List<PlayerData>();

            ServerToUser userRetuen = JsonUtility.FromJson<ServerToUser>(response);

            foreach(GameData game in userRetuen.GameList)
                GameList.Add(game);   
                    
            foreach(PlayerData player in userRetuen.PlayerList)
                PlayerList.Add(player);
        }
        
    }
    public IEnumerator AddPlayer(int PlayerNumber, string PlayerName){
        WWWForm form = new WWWForm();
        form.AddField("account", UserName);
        form.AddField("UserID", UserID);
        form.AddField("PlayerNumber", PlayerNumber);
        form.AddField("PlayerName", PlayerName);

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/AddPlayer", form);
        yield return www.SendWebRequest();

        Return result = new Return();
        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            
            result = JsonUtility.FromJson<Return>(response);
            if(result.success == false){
                switch (result.situation){
                    case 0:
                        Debug.Log("Success!");
                        CallUpdateUserData();
                        break;
                }
            }
                
            
        }

    }
    public IEnumerator AddGame(){
        yield return null;
    }

    public void CallAdd(){
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        if(obj.tag == "AddPlayer"){
            Main.SetActive(false);
            AddPlayerObj.SetActive(true);
            AddGameObj.SetActive(false);
        }
        else if(obj.tag == "AddGame"){
            Main.SetActive(false);
            AddPlayerObj.SetActive(false);
            AddGameObj.SetActive(true);
        }
        else if(obj.tag == "GameSelect"){
            Main.SetActive(true);
            AddPlayerObj.SetActive(false);
            AddGameObj.SetActive(false);
        }
    }

}
