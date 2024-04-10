using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;
using TMPro;
using System;


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

    public GameObject MainCanvas;
    public GameObject AddPlayerCanvas;
    public GameObject AddGameCanvas; 
    public GameObject CorrPlayerCanvas;
    public GameObject AssignCanvas;
    public GameObject WarnCanvas;
    public GameObject AddOtherCanvas;
    public GameObject AddOtherPlayerCanvas;
    public GameObject AssignOthTeamCanvas;
    public GameObject AddTeamCanvas;

    public int numOfGame;
    public int numOfPlayer;
    public int UserID;
    public int UserTeamID;
    public string UserName;
    public List<int> UserPlayerID;
    public List<string> UserPlayerName;
    public List<int> UserPlayerNumber;
    public List<int> UserPlayerPos;
    public List<int> UserGameID;
    public List<string> UserGameDate;
    public List<string> UserGameName;
    public List<int> GameServe;
    public List<int> TeamL;
    public List<int> TeamR;
    public List<int> OtherTeamID;
    public List<string> OtherTeamName;
    public List<int> OtherPlayerID;
    public List<int> OtherPlayerNumber;
    public List<string> OtherPlayerName;
    public List<int> OtherPlayerPos;
    public List<int> OtherPlayerTeamID;
    public InputField PName;
    public InputField PNum;
    public Dropdown PPos;
    public InputField GName;
    public TMP_InputField GDate;
    public Text PWarning;
    public Text GWarning;
    public GameObject CorrTarget;
    public Text TeamWarning;
    public Text OtherTeamWarning;
    public Text WarningMsg;
    public InputField PCorrName;
    public InputField PCorrNum;
    public Dropdown PCorrPos;
    public Text PCWarning;
    public Text UserTeamName;
    public List<int> NotAssignPlayerID;
    public List<string> NotAssignPlayerName;
    public List<int> NotAssignPlayerNumber;
    public List<int> NotAssignPlayerPos;
    public List<int> AssignPlayerID;
    public List<string> AssignPlayerName;
    public List<int> AssignPlayerNumber;
    public List<int> AssignPlayerPos;
    public RectTransform AddContent;
    public RectTransform NotAddContent;
    public GameObject AssignScrollView;
    public GameObject NotAssignScrollView;
    public List<int> OtherNotAssignPlayerID;
    public List<string> OtherNotAssignPlayerName;
    public List<int> OtherNotAssignPlayerNumber;
    public List<int> OtherNotAssignPlayerPos;
    public List<int> OtherNotAssignPlayerTeamID;
    public List<int> OtherAssignPlayerID;
    public List<string> OtherAssignPlayerName;
    public List<int> OtherAssignPlayerNumber;
    public List<int> OtherAssignPlayerPos;
    public List<int> OtherAssignPlayerTeamID;
    public List<bool> GameAlreadySet;
    public List<bool> GameAlreadyFinish;
    public RectTransform OtherAddContent;
    public RectTransform OtherNotAddContent;
    public GameObject OtherAssignScrollView;
    public GameObject OtherNotAssignScrollView;
    public Dropdown OtherTeamChoice;

    public class ServerToUser{
        public bool success;
        public int situation;
        public List<int> UserPlayerID;
        public List<string> UserPlayerName;
        public List<int> UserPlayerNumber;
        public List<int> UserPlayerPos;
        public List<int> UserGameID;
        public List<string> UserGameDate;
        public List<string> UserGameName;
        public List<int> GameServe;
        public List<int> TeamL;
        public List<int> TeamR;
        public List<int> OtherTeamID;
        public List<string> OtherTeamName;
        public List<int> OtherPlayerID;
        public List<int> OtherPlayerNumber;
        public List<string> OtherPlayerName;
        public List<int> OtherPlayerPos;
        public List<int> OtherPlayerTeamID;
        public List<bool> GameAlreadySet;
        public List<bool> GameAlreadyFinish;
        public string ec;
    }
    public class Return{
        public bool success;
        public int situation;
        public string ec;
    }
    private void Awake(){
        UserName = UserData.Instance.UserName; //後面要連伺服器
        numOfGame = UserData.Instance.numOfGame; // 後面要連伺服器
        numOfPlayer = UserData.Instance.numOfPlayer;
        UserID = UserData.Instance.UserID;
        UserTeamID = UserData.Instance.TeamID;
        User.text = "User: " + UserName;
        CallUpdateUserData();
    }
    private void Start()
    {
        MainCanvas.SetActive(true);
        AddPlayerCanvas.SetActive(false);
        AddPlayerCanvas.SetActive(false);
        CorrPlayerCanvas.SetActive(false);
        AssignCanvas.SetActive(false);
        WarnCanvas.SetActive(false);
        AddOtherCanvas.SetActive(false);

        StartCoroutine(DealView());
    }
    public void CallAddPlayer(){
        Regex regexNum = new Regex("^[0-9]+$");
        Regex regexName = new Regex("^[\u4e00-\u9fa50-9A-Za-z_]+$");
        
        if(string.IsNullOrEmpty(PName.text)){
            PWarning.text = "球員名稱欄位不能為空";
            Debug.Log("球員名稱欄位不能為空");
            return;
        }
        string PNameIn = PName.text;
        if(!regexName.IsMatch(PName.text)){
            PWarning.text = "球員名稱只能輸入 中文 數字 大小寫英文";
            Debug.Log("球員名稱只能輸入 中文 數字 大小寫英文");
            return;
        }

        if(string.IsNullOrEmpty(PNum.text)){
            PWarning.text = "背號欄位不能為空";
            Debug.Log("背號欄位不能為空");
            return;
        }
        if(!regexNum.IsMatch(PNum.text)){
            PWarning.text = "背號只能輸入數字";
            Debug.Log("背號只能輸入數字");
            return;
        }
        int PNumIN = int.Parse(PNum.text);
        if(PNumIN < 0 || PNumIN > 100){
            PWarning.text = "背號請輸入 0 - 100!";
            Debug.Log("背號請輸入 0 - 100!");
            return;
        }
        
        StartCoroutine(AddPlayer(PNumIN, PNameIn, PPos.value));
        
    }
    public IEnumerator AddPlayer(int PlayerNumber, string PlayerName, int PlayerPos){
        WWWForm form = new WWWForm();
        form.AddField("UserID", UserID);
        form.AddField("TeamID", UserTeamID);
        form.AddField("PlayerNumber", PlayerNumber);
        form.AddField("PlayerName", PlayerName);
        form.AddField("PlayerPos", PlayerPos);

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/AddPlayer", form);
        yield return www.SendWebRequest();

        Return result = new Return();
        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            print(response);
            result = JsonUtility.FromJson<Return>(response);
            if(result.success == false){
                switch (result.situation){
                    case -1:
                        Debug.Log("參數傳送錯誤!"); 
                        PWarning.text = "參數傳送錯誤!";
                        break;
                    case -2:
                        Debug.Log("資料庫錯誤!"); 
                        PWarning.text = "資料庫錯誤!";
                        break;
                    case -3:
                        Debug.Log("帳號不存在!"); 
                        PWarning.text = "帳號不存在!";
                        break;
                    case -4:
                        Debug.Log("該背號已存在!"); 
                        PWarning.text = "該背號已存在!";
                        break;
                    case -5:
                        Debug.Log("隊伍不存在!"); 
                        PWarning.text = "隊伍不存在!";
                        break;
                }
            }
            else{
                Debug.Log("Success!");
                yield return StartCoroutine(UpdateUserData());
                PWarning.text = "新增成功!";
                yield return StartCoroutine(DealView());
                yield return new WaitForSeconds(1f);
                MainCanvas.SetActive(true);
                AddPlayerCanvas.SetActive(false);
                AddGameCanvas.SetActive(false);
            }
        }
        else{
            Debug.Log("未連接到伺服器!");
        }

    }
    public void CallAddGame(){
        Regex regexName = new Regex("^[\u4e00-\u9fa50-9A-Za-z_]+$");
        if(string.IsNullOrEmpty(GName.text)){
            GWarning.text = "比賽名稱欄位不可為空!";
            return;
        }
        string GNameIN = GName.text;
        if(!regexName.IsMatch(GNameIN)){
            GWarning.text = "比賽名稱只能輸入 中文 數字 大小寫英文";
            Debug.Log("比賽名稱只能輸入 中文 數字 大小寫英文");
            return;
        }
        if(string.IsNullOrEmpty(GDate.text)){
            GWarning.text = "比賽日期未輸入 自動設定為" + DateTime.Today.ToString("yyyy-MM-dd");
            GDate.text = DateTime.Today.ToString("yyyy-MM-dd");
        }

        StartCoroutine(AddGame(GDate.text, GNameIN));
    }
    public IEnumerator AddGame(string GameDate, string GameName){
        yield return new WaitForSeconds(1f);
        WWWForm form = new WWWForm();
        form.AddField("UserID", UserID);
        form.AddField("GameDate", GameDate);
        form.AddField("GameName", GameName);

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/AddGame", form);
        yield return www.SendWebRequest();
        Return result = new Return();
        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            result = JsonUtility.FromJson<Return>(response);
            if(result.success == false){
                switch (result.situation){
                    
                    case -1:
                        Debug.Log("參數傳送錯誤!"); 
                        GWarning.text = "參數傳送錯誤!";
                        break;
                    case -2:
                        Debug.Log("資料庫錯誤!"); 
                        GWarning.text = "資料庫錯誤!";
                        break;
                    case -3:
                        Debug.Log("帳號不存在!"); 
                        GWarning.text = "帳號不存在!";
                        break;
                    case -4:
                        Debug.Log("該比賽已存在!"); 
                        GWarning.text = "該比賽已存在!";
                        break;
                }
            }
            else{
                Debug.Log("Success!");
                yield return StartCoroutine(UpdateUserData());
                GWarning.text = "新增成功!";
                yield return StartCoroutine(DealView());
                yield return new WaitForSeconds(1f);
                MainCanvas.SetActive(true);
                AddPlayerCanvas.SetActive(false);
                AddGameCanvas.SetActive(false);

            }
        }
        else{
            Debug.Log("未連接到伺服器!");
        }
    }
    public void CallCorrPlayer(){
        Regex regexNum = new Regex("^[0-9]+$");
        Regex regexName = new Regex("^[\u4e00-\u9fa50-9A-Za-z_]+$");
        if(string.IsNullOrEmpty(PCorrName.text)){
            PCWarning.text = "球員名稱欄位不能為空";
            Debug.Log("球員名稱欄位不能為空");
            return;
        }
        string PCorrNameIn = PCorrName.text;
        if(!regexName.IsMatch(PCorrName.text)){
            PCWarning.text = "球員名稱只能輸入 中文 數字 大小寫英文";
            Debug.Log("球員名稱只能輸入 中文 數字 大小寫英文");
            return;
        }

        if(string.IsNullOrEmpty(PCorrNum.text)){
            PCWarning.text = "背號欄位不能為空";
            Debug.Log("背號欄位不能為空");
            return;
        }
        if(!regexNum.IsMatch(PCorrNum.text)){
            PCWarning.text = "背號只能輸入數字";
            Debug.Log("背號只能輸入數字");
            return;
        }
        int PCorrNumIN = int.Parse(PCorrNum.text);
        if(PCorrNumIN<=0 || PCorrNumIN >= 100){
            PCWarning.text = "背號請輸入 0 - 100!";
            Debug.Log("背號請輸入 0 - 100!");
            return;
        }
        int PlayerID = CorrTarget.GetComponent<ID>().ObjID;
        int CorrTeamID = CorrTarget.GetComponent<ID>().TeamID;
        StartCoroutine(CorrectPlayer(PCorrNumIN, PCorrNameIn, PlayerID, PCorrPos.value, CorrTeamID));
    }
    public void CallUpdateUserData(){
        StartCoroutine(UpdateUserData());
    }
    public IEnumerator UpdateUserData(){
        print("in");
        WWWForm form = new WWWForm();
        form.AddField("account", UserName);
        form.AddField("UserID", UserID);
        form.AddField("UserTeamID", UserTeamID);

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/UpdateUserData", form);

        yield return www.SendWebRequest();
        print("send");
        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            print(response);
            UserPlayerID = new List<int>(); //OK
            UserPlayerName = new List<string>(); //OK
            UserPlayerNumber = new List<int>(); //OK
            UserPlayerPos = new List<int>(); //OK
            UserGameID = new List<int>(); //OK
            UserGameDate = new List<string>(); //OK
            UserGameName = new List<string>(); //OK
            GameServe = new List<int>();
            TeamL = new List<int>();
            TeamR = new List<int>();
            OtherTeamID = new List<int>();
            OtherTeamName = new List<string>();
            OtherPlayerID = new List<int>();
            OtherPlayerNumber = new List<int>();
            OtherPlayerName = new List<string>();
            OtherPlayerPos = new List<int>();
            OtherPlayerTeamID = new List<int>();
            GameAlreadySet = new List<bool>();
            GameAlreadyFinish = new List<bool>();


            ServerToUser userReturn = JsonUtility.FromJson<ServerToUser>(response);
            print(userReturn.UserPlayerID.Count);
            if(userReturn.success == true){
                for(int i = 0; i < userReturn.UserPlayerID.Count; i++){
                    UserPlayerID.Add(userReturn.UserPlayerID[i]);
                    UserPlayerNumber.Add(userReturn.UserPlayerNumber[i]);
                    UserPlayerName.Add(userReturn.UserPlayerName[i]);
                    UserPlayerPos.Add(userReturn.UserPlayerPos[i]);
                }
                for(int i = 0; i < userReturn.UserGameID.Count; i++){
                    UserGameID.Add(userReturn.UserGameID[i]);
                    UserGameName.Add(userReturn.UserGameName[i]);
                    DateTimeOffset dateTimeOffset = DateTimeOffset.ParseExact(userReturn.UserGameDate[i], "ddd, dd MMM yyyy HH:mm:ss 'GMT'", 
                        System.Globalization.CultureInfo.InvariantCulture);
                    string formattedDate = dateTimeOffset.ToString("yyyy-MM-dd");
                    UserGameDate.Add(formattedDate);
                    GameServe.Add(userReturn.GameServe[i]);
                    TeamL.Add(userReturn.TeamL[i]);
                    TeamR.Add(userReturn.TeamR[i]);
                }
                for(int i = 0; i < userReturn.OtherTeamID.Count; i++){
                    OtherTeamID.Add(userReturn.OtherTeamID[i]);
                    OtherTeamName.Add(userReturn.OtherTeamName[i]);
                }
                for(int i = 0; i < userReturn.OtherPlayerID.Count; i++){
                    OtherPlayerID.Add(userReturn.OtherPlayerID[i]);
                    OtherPlayerName.Add(userReturn.OtherPlayerName[i]);
                    OtherPlayerNumber.Add(userReturn.OtherPlayerNumber[i]);
                    OtherPlayerPos.Add(userReturn.OtherPlayerPos[i]);
                    OtherPlayerTeamID.Add(userReturn.OtherPlayerTeamID[i]);
                }
                Debug.Log("Success!");
            }
            else if(userReturn.success == false){
                switch (userReturn.situation){
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
    public void LogOut(){
        
        GameObject[] dontDestroyObjects = GameObject.FindGameObjectsWithTag("DontDestroy");
        foreach (GameObject obj in dontDestroyObjects)
            Destroy(obj);
        SceneManager.LoadScene("StartMenu");
    
    }
    public IEnumerator waitsecond(float second){
        yield return new WaitForSeconds(second);
    }
    public void ClickGame(){
        
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        int GameID = obj.GetComponent<ID>().ObjID;
        int Set;
        for(int i = 0; i < UserGameID.Count; i++){
            if(UserGameID[i] == GameID){
                UserData.Instance.GameName = UserGameName[i];
                if(GameAlreadySet[i] && GameAlreadyFinish[i])
                    Set = 1;
                
                else if(GameAlreadySet[i] && !GameAlreadyFinish[i])
                    Set = 2;
                else
                    Set = 3;
                break;
            }
        }
        if(Set == 3){ // 未設定比賽
            UserData.Instance.GameID = GameID;
            UserTeamName.text = UserData.Instance.UserTeamName;
            UserData.Instance.GameName = obj.GetComponentsInChildren<Text>()[1].text;
            
            NotAssignPlayerID = new List<int>();
            NotAssignPlayerName = new List<string>();
            NotAssignPlayerNumber = new List<int>();
            NotAssignPlayerPos = new List<int>();
            AssignPlayerID = new List<int>();
            AssignPlayerName = new List<string>();
            AssignPlayerNumber = new List<int>();
            AssignPlayerPos = new List<int>();

            OtherNotAssignPlayerID = new List<int>();
            OtherNotAssignPlayerName = new List<string>();
            OtherNotAssignPlayerNumber = new List<int>();
            OtherNotAssignPlayerTeamID = new List<int>();
            OtherAssignPlayerID = new List<int>();
            OtherAssignPlayerName = new List<string>();
            OtherAssignPlayerNumber = new List<int>();
            OtherAssignPlayerTeamID = new List<int>();


            for(int i = 0; i < UserPlayerID.Count; i++){
                NotAssignPlayerID.Add(UserPlayerID[i]);
                NotAssignPlayerName.Add(UserPlayerName[i]);
                NotAssignPlayerNumber.Add(UserPlayerNumber[i]);
                NotAssignPlayerPos.Add(UserPlayerPos[i]);
            }

            for(int i = 0; i < OtherNotAssignPlayerID.Count; i++){
                OtherNotAssignPlayerID.Add(UserPlayerID[i]);
                OtherNotAssignPlayerName.Add(UserPlayerName[i]);
                OtherNotAssignPlayerNumber.Add(UserPlayerNumber[i]);
                OtherNotAssignPlayerTeamID.Add(OtherPlayerTeamID[i]);
                OtherNotAssignPlayerPos.Add(OtherPlayerPos[i]);
            }

            MainCanvas.SetActive(false);
            AssignCanvas.SetActive(true);
            StartCoroutine(ShowNotAssign());
        }
        else if(Set == 2){
            print("Error!");
        }
        else{ // show data

        }
        

    }
    public void GoToAssignOther(){
        if(AssignPlayerID.Count != 12){
            TeamWarning.text = "人數不足12人!"; 
        }
        else{
            UserData.Instance.UserPlayerID = AssignPlayerID;
            UserData.Instance.UserPlayerName = AssignPlayerName;
            UserData.Instance.UserPlayerNumber = AssignPlayerNumber;
            UserData.Instance.UserPlayerPlayPos = AssignPlayerPos;
            OtherTeamChoice.ClearOptions();
            OtherTeamChoice.AddOptions(OtherTeamName);
            OtherTeamChoice.value = -1;
            AssignCanvas.SetActive(false);
            AssignOthTeamCanvas.SetActive(true);
        }
    } 
    public void ComfirmEnter(){
        UserData.Instance.UserPlayerID = AssignPlayerID;
        UserData.Instance.UserPlayerName = AssignPlayerName;
        UserData.Instance.UserPlayerNumber = AssignPlayerNumber;
        SceneManager.LoadScene("MainScene");
    }
    public void CancelEnter(){
        AssignCanvas.SetActive(true);
        WarnCanvas.SetActive(false);
    }
    public void ClickPlayer(){
        CorrTarget = EventSystem.current.currentSelectedGameObject;
        Text[] ObjTexts = CorrTarget.GetComponentsInChildren<Text>();
        PCorrNum.text = ObjTexts[0].text;
        PCorrName.text = ObjTexts[1].text;
        PCWarning.text = "";
        int PlayerID = CorrTarget.GetComponent<ID>().ObjID;
        print(PlayerID);
        print(ObjTexts[0].text);
        print(ObjTexts[1].text);
        MainCanvas.SetActive(false);
        CorrPlayerCanvas.SetActive(true);
        
    }
    public void CallAdd(){
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        if(obj.tag == "AddPlayer"){
            MainCanvas.SetActive(false);
            AddPlayerCanvas.SetActive(true);
            AddGameCanvas.SetActive(false);
            PName.text = "";
            PNum.text = "";
            PWarning.text = "";
        }
        else if(obj.tag == "AddGame"){
            MainCanvas.SetActive(false);
            AddPlayerCanvas.SetActive(false);
            AddGameCanvas.SetActive(true);
            GName.text = "";
            GDate.text = "";
            GWarning.text = "";
        }
        else if(obj.tag == "GameSelect"){
            MainCanvas.SetActive(true);
            AddPlayerCanvas.SetActive(false);
            AddGameCanvas.SetActive(false);
            AddOtherCanvas.SetActive(false);
            AssignOthTeamCanvas.SetActive(false);
        }
        else if(obj.tag == "CorrPlayer"){
            MainCanvas.SetActive(true);
            CorrPlayerCanvas.SetActive(false);
        }
        else if(obj.tag == "AssignPlayer"){
            MainCanvas.SetActive(true);
            AssignCanvas.SetActive(false);
            TeamWarning.text = "分配前6即為先發1-6號位!";
        }
        else if(obj.tag == "AddOther"){
            MainCanvas.SetActive(false);
            AddOtherPlayerCanvas.SetActive(false);
            AddOtherCanvas.SetActive(true);
            AddTeamCanvas.SetActive(false);
            OCorrPlayerCanvas.SetActive(false);
        }
        else if(obj.tag == "AddOtherPlayer"){
            AddOtherCanvas.SetActive(false);
            AddOtherPlayerCanvas.SetActive(true);
            OtherAddTeamChoice.ClearOptions();
            OtherAddTeamChoice.AddOptions(OtherTeamName);
            OtherPName.text = "";
            OtherPNum.text = "";
            OtherPPos.value = 0;
            OtherTeamChoice.value = 0;
            AddOPWarn.text = "";
        }
        else if(obj.tag == "AddTeam"){
            AddOtherCanvas.SetActive(false);
            AddTeamCanvas.SetActive(true);
            TeamNameIn.text = "";
            TeamWarn.text = "";
        }
        
    }
    public IEnumerator DealView(){

        // 初始化ScrollView的滾動位置
        for (int i = 0; i < GameContent.childCount; i++){
            Destroy(GameContent.GetChild(i).gameObject);
        }

        for (int i = 0; i < PlayerContent.childCount; i++){
            Destroy(PlayerContent.GetChild(i).gameObject);
        }
        GameScrollview.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
        PlayerScrollview.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;

        // 這裡演示創建3個Prefab的例子，實際上你可以根據需求生成不同數量的Prefab
        for (int i = 0; i < UserGameID.Count; i++)
        {
            // 透過Instantiate生成Prefab
            GameObject newGame = Instantiate(Game, GameContent);

            Text[] GameTexts = newGame.GetComponentsInChildren<Text>();

            GameTexts[0].text = UserGameDate[i];
            GameTexts[1].text = UserGameName[i];
                        newGame.GetComponent<ID>().ObjID = UserGameID[i];
            newGame.GetComponent<Button>().onClick.AddListener(ClickGame);
            // 設定Prefab的位置
            RectTransform rectTransform = newGame.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, -i * prefabHeight);
        }
        for (int i = 0; i < UserPlayerID.Count; i++)
        {

            GameObject newPlayer = Instantiate(Player, PlayerContent);
            Text[] PlayerTexts = newPlayer.GetComponentsInChildren<Text>();

            PlayerTexts[0].text =  UserPlayerNumber[i].ToString();
            PlayerTexts[1].text = UserPlayerName[i];

            switch(UserPlayerPos[i])
            {
                case 0:
                    PlayerTexts[2].text = "主攻";
                    break;
                case 1:
                    PlayerTexts[2].text = "攔中";
                    break;
                case 2:
                    PlayerTexts[2].text = "舉球";
                    break;
                case 3:
                    PlayerTexts[2].text = "輔舉";
                    break;
                case 4:
                    PlayerTexts[2].text = "自由";
                    break;
            }

            PlayerTexts[3].text = UserData.Instance.UserTeamName;

            newPlayer.GetComponent<ID>().ObjID = UserPlayerID[i];
            newPlayer.GetComponent<ID>().ObjID = UserTeamID;
            newPlayer.GetComponent<Button>().onClick.AddListener(ClickPlayer);

            // 設定Prefab的位置
            RectTransform rectTransform = newPlayer.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, -i * prefabHeight);
        }
        GameContent.sizeDelta = new Vector2(GameContent.sizeDelta.x, 0 * prefabHeight);
        PlayerContent.sizeDelta = new Vector2(PlayerContent.sizeDelta.x, 0 * prefabHeight);

        yield return null;
    }
    public IEnumerator CorrectPlayer(int PlayerNumber, string PlayerName, int PlayerID, int PlayerPos, int UserTeamID){
        WWWForm form = new WWWForm();
        form.AddField("UserID", UserID);
        form.AddField("PlayerID", PlayerID);
        form.AddField("TeamID", UserTeamID);
        form.AddField("PlayerNumber", PlayerNumber);
        form.AddField("PlayerName", PlayerName);
        form.AddField("PlayerPos", PlayerPos);

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/CorrectPlayer", form);
        yield return www.SendWebRequest();

        Return result = new Return();
        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            result = JsonUtility.FromJson<Return>(response);
            if(result.success == false){
                switch (result.situation){
                    
                    case -1:
                        Debug.Log("參數傳送錯誤!"); 
                        PCWarning.text = "參數傳送錯誤!";
                        break;
                    case -2:
                        Debug.Log("資料庫錯誤!"); 
                        PCWarning.text = "資料庫錯誤!";
                        break;
                    case -3:
                        Debug.Log("帳號不存在!"); 
                        PCWarning.text = "帳號不存在!";
                        break;
                    case -4:
                        Debug.Log("該球員名稱已被使用!"); 
                        PCWarning.text = "該球員名稱已被使用!";
                        break;
                    case -5:
                        Debug.Log("該背號已被使用!"); 
                        PCWarning.text = "該背號已被使用!";
                        break;
                    case -6:
                        Debug.Log("修改球員的資料不存在!");
                        PCWarning.text = "修改球員的資料不存在!";
                        break;
                }
            }
            else{
                Debug.Log("Success!");
                yield return StartCoroutine(UpdateUserData());
                PCWarning.text = "修正成功!";
                yield return StartCoroutine(DealView());
                yield return new WaitForSeconds(1f);
                MainCanvas.SetActive(true);
                CorrPlayerCanvas.SetActive(false);
            }
        }
        else{
            Debug.Log("未連接到伺服器!");
        }
    }
    public IEnumerator ShowNotAssign(){
        for (int i = 0; i < AddContent.childCount; i++){
            Destroy(AddContent.GetChild(i).gameObject);
        }

        for (int i = 0; i < NotAddContent.childCount; i++){
            Destroy(NotAddContent.GetChild(i).gameObject);
        }
        AssignScrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
        NotAssignScrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
        for (int i = 0; i < AssignPlayerID.Count; i++)
        {

            GameObject NewAssign = Instantiate(Player, AddContent);
            Text[] PlayerTexts = NewAssign.GetComponentsInChildren<Text>();

            PlayerTexts[0].text = AssignPlayerNumber[i].ToString();
            PlayerTexts[1].text = AssignPlayerName[i];
            switch (AssignPlayerPos[i]){
                case 0:
                    PlayerTexts[2].text = "主攻";
                    break;
                case 1:
                    PlayerTexts[2].text = "攔中";
                    break;
                case 2:
                    PlayerTexts[2].text = "舉球";
                    break;
                case 3:
                    PlayerTexts[2].text = "輔舉";
                    break;
                case 4:
                    PlayerTexts[2].text = "自由";
                    break;

            }
            PlayerTexts[3].text = UserData.Instance.UserTeamName;
            NewAssign.GetComponent<ID>().ObjID = AssignPlayerID[i];
            NewAssign.GetComponent<ID>().TeamID = UserTeamID;
            NewAssign.GetComponent<Button>().onClick.AddListener(CancelAssign);

            // 設定Prefab的位置
            RectTransform rectTransform = NewAssign.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, -i * prefabHeight);
        }
        for (int i = 0; i < NotAssignPlayerID.Count; i++)
        {

            GameObject NewCanAssign = Instantiate(Player, NotAddContent);
            Text[] PlayerTexts = NewCanAssign.GetComponentsInChildren<Text>();

            PlayerTexts[0].text =  NotAssignPlayerNumber[i].ToString();
            PlayerTexts[1].text = NotAssignPlayerName[i];

            switch (NotAssignPlayerPos[i]){
                case 0:
                    PlayerTexts[2].text = "主攻";
                    break;
                case 1:
                    PlayerTexts[2].text = "攔中";
                    break;
                case 2:
                    PlayerTexts[2].text = "舉球";
                    break;
                case 3:
                    PlayerTexts[2].text = "輔舉";
                    break;
                case 4:
                    PlayerTexts[2].text = "自由";
                    break;

            }
            PlayerTexts[3].text = UserData.Instance.UserTeamName;
            NewCanAssign.GetComponent<ID>().ObjID = NotAssignPlayerID[i];
            NewCanAssign.GetComponent<ID>().TeamID = UserTeamID;
            NewCanAssign.GetComponent<Button>().onClick.AddListener(Assign);

            // 設定Prefab的位置
            RectTransform rectTransform = NewCanAssign.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, -i * prefabHeight);
        }
        AddContent.sizeDelta = new Vector2(AddContent.sizeDelta.x, 0 * prefabHeight);
        NotAddContent.sizeDelta = new Vector2(NotAddContent.sizeDelta.x, 0 * prefabHeight);

        yield return null;
    }
    public void Assign(){
        if(AssignPlayerID.Count >= 12){
            TeamWarning.text = "人數已到12人無法分配";
            return;
        }
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        int PlayerID = obj.GetComponent<ID>().ObjID;
        for(int i = 0; i < NotAssignPlayerID.Count; i++){
            if(PlayerID == NotAssignPlayerID[i]){
                AssignPlayerID.Add(NotAssignPlayerID[i]);
                AssignPlayerName.Add(NotAssignPlayerName[i]);
                AssignPlayerNumber.Add(NotAssignPlayerNumber[i]);
                AssignPlayerPos.Add(NotAssignPlayerPos[i]);

                NotAssignPlayerID.RemoveAt(i);
                NotAssignPlayerName.RemoveAt(i);
                NotAssignPlayerNumber.RemoveAt(i);
                NotAssignPlayerPos.RemoveAt(i);

                break;
            }
        }
        StartCoroutine(ShowNotAssign());
    }
    public void CancelAssign(){
        
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        int PlayerID = obj.GetComponent<ID>().ObjID;
        for(int i = 0; i < AssignPlayerID.Count; i++){
            if(PlayerID == AssignPlayerID[i]){
                NotAssignPlayerID.Add(AssignPlayerID[i]);
                NotAssignPlayerName.Add(AssignPlayerName[i]);
                NotAssignPlayerNumber.Add(AssignPlayerNumber[i]);
                NotAssignPlayerPos.Add(AssignPlayerPos[i]);

                AssignPlayerID.RemoveAt(i);
                AssignPlayerName.RemoveAt(i);
                AssignPlayerNumber.RemoveAt(i);
                AssignPlayerPos.RemoveAt(i);
                break;
            }
        }
        CallOtherShowNotAssign();
    }
    public void OtherAssign(){

        if(OtherAssignPlayerID.Count >= 12){
            TeamWarning.text = "人數已到12人無法分配";
            return;
        }
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        int PlayerID = obj.GetComponent<ID>().ObjID;
        print(PlayerID);
        for(int i = 0; i < OtherNotAssignPlayerID.Count; i++){
            if(PlayerID == OtherNotAssignPlayerID[i]){
                OtherAssignPlayerID.Add(OtherNotAssignPlayerID[i]);
                OtherAssignPlayerName.Add(OtherNotAssignPlayerName[i]);
                OtherAssignPlayerNumber.Add(OtherNotAssignPlayerNumber[i]);
                OtherAssignPlayerPos.Add(OtherNotAssignPlayerPos[i]);
                OtherAssignPlayerTeamID.Add(OtherNotAssignPlayerTeamID[i]);

                OtherNotAssignPlayerID.RemoveAt(i);
                OtherNotAssignPlayerName.RemoveAt(i);
                OtherNotAssignPlayerNumber.RemoveAt(i);
                OtherNotAssignPlayerPos.RemoveAt(i);
                OtherNotAssignPlayerTeamID.RemoveAt(i);

                break;
            }
        }
        CallOtherShowNotAssign();
    }
    public void OtherCancelAssign(){
        
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        int PlayerID = obj.GetComponent<ID>().ObjID;
        print(PlayerID);
        for(int i = 0; i < OtherAssignPlayerID.Count; i++){
            if(PlayerID == OtherAssignPlayerID[i]){
                OtherNotAssignPlayerID.Add(OtherAssignPlayerID[i]);
                OtherNotAssignPlayerName.Add(OtherAssignPlayerName[i]);
                OtherNotAssignPlayerNumber.Add(OtherAssignPlayerNumber[i]);
                OtherNotAssignPlayerPos.Add(OtherAssignPlayerPos[i]);
                OtherNotAssignPlayerTeamID.Add(OtherAssignPlayerTeamID[i]);

                OtherAssignPlayerID.RemoveAt(i);
                OtherAssignPlayerName.RemoveAt(i);
                OtherAssignPlayerNumber.RemoveAt(i);
                OtherAssignPlayerPos.RemoveAt(i);
                OtherAssignPlayerTeamID.RemoveAt(i);

                break;
            }
        }
        CallOtherShowNotAssign();
    }
    public void CallDealView(){
        StartCoroutine(DealView());
    }
    public IEnumerator OtherShowNotAssign(){
        for (int i = 0; i < OtherAddContent.childCount; i++){
            Destroy(OtherAddContent.GetChild(i).gameObject);
        }

        for (int i = 0; i < OtherNotAddContent.childCount; i++){
            Destroy(OtherNotAddContent.GetChild(i).gameObject);
        }
        
        OtherAssignScrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
        OtherNotAssignScrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
        for (int i = 0; i < OtherAssignPlayerID.Count; i++)
        {

            GameObject OtherNewAssign = Instantiate(Player, OtherAddContent);
            Text[] PlayerTexts = OtherNewAssign.GetComponentsInChildren<Text>();

            PlayerTexts[0].text = OtherAssignPlayerNumber[i].ToString();
            PlayerTexts[1].text = OtherAssignPlayerName[i];
            switch (OtherAssignPlayerPos[i]){
                case 0:
                    PlayerTexts[2].text = "主攻";
                    break;
                case 1:
                    PlayerTexts[2].text = "攔中";
                    break;
                case 2:
                    PlayerTexts[2].text = "舉球";
                    break;
                case 3:
                    PlayerTexts[2].text = "輔舉";
                    break;
                case 4:
                    PlayerTexts[2].text = "自由";
                    break;

            }
            PlayerTexts[3].text = OtherTeamName[OtherTeamChoice.value];
            OtherNewAssign.GetComponent<ID>().ObjID = OtherAssignPlayerID[i];
            OtherNewAssign.GetComponent<ID>().TeamID = OtherAssignPlayerTeamID[i];
            OtherNewAssign.GetComponent<Button>().onClick.AddListener(OtherCancelAssign);

            // 設定Prefab的位置
            RectTransform rectTransform = OtherNewAssign.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, -i * prefabHeight);
        }
        for (int i = 0; i < OtherNotAssignPlayerID.Count; i++)
        {
            print(OtherNotAssignPlayerTeamID.Count);
            
            GameObject OtherNewCanAssign = Instantiate(Player, OtherNotAddContent);
            Text[] PlayerTexts = OtherNewCanAssign.GetComponentsInChildren<Text>();

            PlayerTexts[0].text =  OtherNotAssignPlayerNumber[i].ToString();
            PlayerTexts[1].text = OtherNotAssignPlayerName[i];

            switch (OtherNotAssignPlayerPos[i]){
                case 0:
                    PlayerTexts[2].text = "主攻";
                    break;
                case 1:
                    PlayerTexts[2].text = "攔中";
                    break;
                case 2:
                    PlayerTexts[2].text = "舉球";
                    break;
                case 3:
                    PlayerTexts[2].text = "輔舉";
                    break;
                case 4:
                    PlayerTexts[2].text = "自由";
                    break;

            }
            PlayerTexts[3].text = OtherTeamName[OtherTeamChoice.value];
            OtherNewCanAssign.GetComponent<ID>().ObjID = OtherNotAssignPlayerID[i];
            OtherNewCanAssign.GetComponent<ID>().TeamID = OtherNotAssignPlayerTeamID[i];
            OtherNewCanAssign.GetComponent<Button>().onClick.AddListener(OtherAssign);

            // 設定Prefab的位置
            RectTransform rectTransform = OtherNewCanAssign.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, -i * prefabHeight);
        }
        OtherAddContent.sizeDelta = new Vector2(OtherAddContent.sizeDelta.x, 0 * prefabHeight);
        OtherNotAddContent.sizeDelta = new Vector2(OtherNotAddContent.sizeDelta.x, 0 * prefabHeight);

        yield return null;
    }
    public void CallOtherShowNotAssign(){
        StartCoroutine(OtherShowNotAssign());
    }
    public void TeamChoiceChange(){
        for (int i = 0; i < OtherAddContent.childCount; i++){
            Destroy(OtherAddContent.GetChild(i).gameObject);
        }

        for (int i = 0; i < OtherNotAddContent.childCount; i++){
            Destroy(OtherNotAddContent.GetChild(i).gameObject);
        }
        OtherAssignPlayerID.Clear();
        OtherAssignPlayerName.Clear();
        OtherAssignPlayerNumber.Clear();
        OtherAssignPlayerPos.Clear();
        OtherAssignPlayerTeamID.Clear();

        OtherNotAssignPlayerID.Clear();
        OtherNotAssignPlayerName.Clear();
        OtherNotAssignPlayerNumber.Clear();
        OtherNotAssignPlayerPos.Clear();
        OtherNotAssignPlayerTeamID.Clear();
        
        int chooseTeam = OtherTeamID[OtherTeamChoice.value];
        for(int i = 0; i < OtherPlayerID.Count; i++){
            if(OtherPlayerTeamID[i] == chooseTeam){
                OtherNotAssignPlayerID.Add(OtherPlayerID[i]);
                OtherNotAssignPlayerName.Add(OtherPlayerName[i]);
                OtherNotAssignPlayerNumber.Add(OtherPlayerNumber[i]);
                OtherNotAssignPlayerPos.Add(OtherPlayerPos[i]);
                OtherNotAssignPlayerTeamID.Add(OtherPlayerTeamID[i]);
            }
        }
        CallOtherShowNotAssign();
    }
    public InputField OtherPName;
    public InputField OtherPNum;
    public Dropdown OtherPPos;
    public Dropdown OtherAddTeamChoice;
    public Text AddOPWarn;
    public void CallAddOtherPlayer(){
        Regex regexNum = new Regex("^[0-9]+$");
        Regex regexName = new Regex("^[\u4e00-\u9fa50-9A-Za-z_]+$");

        if(string.IsNullOrEmpty(OtherPName.text)){
            AddOPWarn.text = "球員名稱欄位不能為空";
            Debug.Log("球員名稱欄位不能為空");
            return;
        }
        string PNameIn = OtherPName.text;
        if(!regexName.IsMatch(OtherPName.text)){
            AddOPWarn.text = "球員名稱只能輸入 中文 數字 大小寫英文";
            Debug.Log("球員名稱只能輸入 中文 數字 大小寫英文");
            return;
        }

        if(string.IsNullOrEmpty(OtherPNum.text)){
            AddOPWarn.text = "背號欄位不能為空";
            Debug.Log("背號欄位不能為空");
            return;
        }
        if(!regexNum.IsMatch(OtherPNum.text)){
            AddOPWarn.text = "背號只能輸入數字";
            Debug.Log("背號只能輸入數字");
            return;
        }
        int PNumIN = int.Parse(OtherPNum.text);
        if(PNumIN < 0 || PNumIN > 100){
            AddOPWarn.text = "背號請輸入 0 - 100!";
            Debug.Log("背號請輸入 0 - 100!");
            return;
        }
        
        StartCoroutine(AddOtherPlayer(PNumIN, PNameIn, OtherPPos.value, OtherTeamID[OtherAddTeamChoice.value]));
    }
    public IEnumerator AddOtherPlayer(int PNumIN, string PNameIn, int PPos, int PTeam){
        WWWForm form = new WWWForm();
        form.AddField("UserID", UserID);
        form.AddField("TeamID", PTeam);
        form.AddField("PlayerNumber", PNumIN);
        form.AddField("PlayerName", PNameIn);
        form.AddField("PlayerPos", PPos);

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/AddPlayer", form);
        yield return www.SendWebRequest();

        Return result = new Return();
        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            print(response);
            result = JsonUtility.FromJson<Return>(response);
            if(result.success == false){
                switch (result.situation){
                    case -1:
                        Debug.Log("參數傳送錯誤!"); 
                        PWarning.text = "參數傳送錯誤!";
                        break;
                    case -2:
                        Debug.Log("資料庫錯誤!"); 
                        PWarning.text = "資料庫錯誤!";
                        break;
                    case -3:
                        Debug.Log("帳號不存在!"); 
                        PWarning.text = "帳號不存在!";
                        break;
                    case -4:
                        Debug.Log("該背號已存在!"); 
                        PWarning.text = "該背號已存在!";
                        break;
                    case -5:
                        Debug.Log("隊伍不存在!"); 
                        PWarning.text = "隊伍不存在!";
                        break;
                }
            }
            else{
                Debug.Log("Success!");
                yield return StartCoroutine(UpdateUserData());
                AddOPWarn.text = "新增成功!";
                yield return StartCoroutine(DealOtherView());
                yield return new WaitForSeconds(1f);
                AddOtherCanvas.SetActive(true);
                AddOtherPlayerCanvas.SetActive(false);
                AddTeamCanvas.SetActive(false);
            }
        }
        else{
            Debug.Log("未連接到伺服器!");
        }
    }

    public Text TeamWarn;
    public InputField TeamNameIn;

    public void CallAddTeam(){
        Regex regexName = new Regex("^[\u4e00-\u9fa50-9A-Za-z_]+$");
        if(string.IsNullOrEmpty(TeamNameIn.text)){
            TeamWarn.text = "隊伍名稱欄位不能為空";
            Debug.Log("隊伍名稱欄位不能為空");
            return;
        }
        
        if(!regexName.IsMatch(TeamNameIn.text)){
            TeamWarn.text = "隊伍名稱只能輸入 中文 數字 大小寫英文";
            Debug.Log("隊伍名稱只能輸入 中文 數字 大小寫英文");
            return;
        }

        StartCoroutine(AddTeam(TeamNameIn.text));

        return;
    }
    public IEnumerator AddTeam(string TeamName){
        WWWForm form = new WWWForm();
        form.AddField("UserID", UserID);
        form.AddField("TeamName", TeamName);

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/AddTeam", form);
        yield return www.SendWebRequest();

        Return result = new Return();
        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            result = JsonUtility.FromJson<Return>(response);
            if(result.success == false){
                switch(result.situation){
                    case -1:
                        Debug.Log("參數傳送錯誤!"); 
                        TeamWarn.text = "參數傳送錯誤!";
                        break;
                    case -2:
                        Debug.Log("資料庫錯誤!"); 
                        TeamWarn.text = "資料庫錯誤!";
                        break;
                    case -3:
                        Debug.Log("帳號不存在!"); 
                        TeamWarn.text = "帳號不存在!";
                        break;
                    case -4:
                        Debug.Log("隊伍已存在!"); 
                        TeamWarn.text = "隊伍已存在!";
                        break;
                    
                }
            }
            else{
                Debug.Log("Success!");
                yield return StartCoroutine(UpdateUserData());
                TeamWarn.text = "新增成功!";
                yield return StartCoroutine(DealOtherView());
                yield return new WaitForSeconds(1f);
                AddTeamCanvas.SetActive(false);
                AddOtherCanvas.SetActive(true);
            }
        }
        else{
            TeamWarn.text = "連接伺服器失敗!";
        }

    }

    public GameObject TeamScrollview;
    public RectTransform TeamContent;
    public GameObject OtherTeamPScrollview;
    public RectTransform OtherTeamPContent;
    public GameObject Team;
    public GameObject CorrTeamCanvas;
    public InputField CorrTeamIn;
    public Text CorrTeamWarn;
    public IEnumerator DealOtherView(){

        // 初始化ScrollView的滾動位置
        for (int i = 0; i < TeamContent.childCount; i++){
            Destroy(TeamContent.GetChild(i).gameObject);
        }

        for (int i = 0; i < OtherTeamPContent.childCount; i++){
            Destroy(OtherTeamPContent.GetChild(i).gameObject);
        }
        TeamScrollview.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
        OtherTeamPScrollview.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;

        // 這裡演示創建3個Prefab的例子，實際上你可以根據需求生成不同數量的Prefab
        for (int i = 0; i < OtherTeamID.Count; i++)
        {
            // 透過Instantiate生成Prefab
            GameObject newTeam = Instantiate(Team, TeamContent);

            Text[] TeamTexts = newTeam.GetComponentsInChildren<Text>();

            TeamTexts[0].text = OtherTeamName[i];
            newTeam.GetComponent<ID>().ObjID = OtherTeamID[i];
            // 設定Prefab的位置
            RectTransform rectTransform = newTeam.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, -i * prefabHeight);
        }
        for (int i = 0; i < OtherPlayerID.Count; i++)
        {

            GameObject newPlayer = Instantiate(Player, OtherTeamPContent);
            Text[] PlayerTexts = newPlayer.GetComponentsInChildren<Text>();

            PlayerTexts[0].text =  OtherPlayerNumber[i].ToString();
            PlayerTexts[1].text = OtherPlayerName[i];

            switch(OtherPlayerPos[i])
            {
                case 0:
                    PlayerTexts[2].text = "主攻";
                    break;
                case 1:
                    PlayerTexts[2].text = "攔中";
                    break;
                case 2:
                    PlayerTexts[2].text = "舉球";
                    break;
                case 3:
                    PlayerTexts[2].text = "輔舉";
                    break;
                case 4:
                    PlayerTexts[2].text = "自由";
                    break;
            }

            for(int j = 0; j < OtherTeamID.Count; j++){
                if(OtherPlayerTeamID[i] == OtherTeamID[j]){
                    PlayerTexts[3].text = OtherTeamName[j];
                    break;
                }
            }
            

            newPlayer.GetComponent<ID>().ObjID = OtherPlayerID[i];
            newPlayer.GetComponent<ID>().TeamID = OtherPlayerTeamID[i];
            newPlayer.GetComponent<Button>().onClick.AddListener(ClickOPlayer);

            // 設定Prefab的位置
            RectTransform rectTransform = newPlayer.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, -i * prefabHeight);
        }
        TeamContent.sizeDelta = new Vector2(TeamContent.sizeDelta.x, 0 * prefabHeight);
        OtherTeamPContent.sizeDelta = new Vector2(OtherTeamPContent.sizeDelta.x, 0 * prefabHeight);

        yield return null;
    }
    public void CallDealOtherView(){
        StartCoroutine(DealOtherView());
    }
    public void ClickTeam(){
        CorrTarget = EventSystem.current.currentSelectedGameObject;
        Text[] ObjTexts = CorrTarget.GetComponentsInChildren<Text>();
        CorrTeamIn.text = ObjTexts[0].text;

        int TeamID = CorrTarget.GetComponent<ID>().ObjID;

    }    
    public void ClickOPlayer(){
        CorrTarget = EventSystem.current.currentSelectedGameObject;
        Text[] ObjTexts = CorrTarget.GetComponentsInChildren<Text>();
        OPCorrNum.text = ObjTexts[0].text;
        OPCorrName.text = ObjTexts[1].text;
        if(ObjTexts[2].text == "主攻"){
            OPCorrPos.value = 0;
        }
        else if(ObjTexts[2].text =="攔中"){
            OPCorrPos.value = 1;
        }
        else if(ObjTexts[2].text =="舉球"){
            OPCorrPos.value= 2;
        }
        else if(ObjTexts[2].text =="輔舉"){
            OPCorrPos.value= 3;
        }
        else if(ObjTexts[2].text =="自由"){
            OPCorrPos.value= 4;
        }
        OPCWarning.text = "";
        int PlayerID = CorrTarget.GetComponent<ID>().ObjID;
        
        AddOtherCanvas.SetActive(false);
        OCorrPlayerCanvas.SetActive(true);
        
    }

    public InputField OPCorrName;
    public InputField OPCorrNum;
    public Dropdown OPCorrPos;
    public Text OPCWarning;
    public GameObject OCorrPlayerCanvas;

    public void CallOCorrPlayer(){
        Regex regexNum = new Regex("^[0-9]+$");
        Regex regexName = new Regex("^[\u4e00-\u9fa50-9A-Za-z_]+$");
        if(string.IsNullOrEmpty(OPCorrName.text)){
            OPCWarning.text = "球員名稱欄位不能為空";
            Debug.Log("球員名稱欄位不能為空");
            return;
        }
        string OPCorrNameIn = OPCorrName.text;
        if(!regexName.IsMatch(OPCorrName.text)){
            OPCWarning.text = "球員名稱只能輸入 中文 數字 大小寫英文";
            Debug.Log("球員名稱只能輸入 中文 數字 大小寫英文");
            return;
        }

        if(string.IsNullOrEmpty(OPCorrNum.text)){
            OPCWarning.text = "背號欄位不能為空";
            Debug.Log("背號欄位不能為空");
            return;
        }
        if(!regexNum.IsMatch(OPCorrNum.text)){
            OPCWarning.text = "背號只能輸入數字";
            Debug.Log("背號只能輸入數字");
            return;
        }
        int OPCorrNumIN = int.Parse(OPCorrNum.text);
        if(OPCorrNumIN<=0 || OPCorrNumIN >= 100){
            OPCWarning.text = "背號請輸入 0 - 100!";
            Debug.Log("背號請輸入 0 - 100!");
            return;
        }
        int PlayerID = CorrTarget.GetComponent<ID>().ObjID;
        int CorrTeamID = CorrTarget.GetComponent<ID>().TeamID;
        StartCoroutine(OCorrectPlayer(OPCorrNumIN, OPCorrNameIn, PlayerID, OPCorrPos.value, CorrTeamID));
    }
    public IEnumerator OCorrectPlayer(int PlayerNumber, string PlayerName, int PlayerID, int PlayerPos, int TeamID){
        WWWForm form = new WWWForm();
        form.AddField("UserID", UserID);
        form.AddField("PlayerID", PlayerID);
        form.AddField("TeamID", TeamID);
        form.AddField("PlayerNumber", PlayerNumber);
        form.AddField("PlayerName", PlayerName);
        form.AddField("PlayerPos", PlayerPos);

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/CorrectPlayer", form);
        yield return www.SendWebRequest();

        Return result = new Return();
        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            result = JsonUtility.FromJson<Return>(response);
            if(result.success == false){
                switch (result.situation){
                    
                    case -1:
                        Debug.Log("參數傳送錯誤!"); 
                        OPCWarning.text = "參數傳送錯誤!";
                        break;
                    case -2:
                        Debug.Log("資料庫錯誤!"); 
                        OPCWarning.text = "資料庫錯誤!";
                        break;
                    case -3:
                        Debug.Log("帳號不存在!"); 
                        OPCWarning.text = "帳號不存在!";
                        break;
                    case -4:
                        Debug.Log("該球員名稱已被使用!"); 
                        OPCWarning.text = "該球員名稱已被使用!";
                        break;
                    case -5:
                        Debug.Log("該背號已被使用!"); 
                        OPCWarning.text = "該背號已被使用!";
                        break;
                    case -6:
                        Debug.Log("修改球員的資料不存在!");
                        OPCWarning.text = "修改球員的資料不存在!";
                        break;
                }
            }
            else{
                Debug.Log("Success!");
                yield return StartCoroutine(UpdateUserData());
                OPCWarning.text = "修正成功!";
                yield return StartCoroutine(DealOtherView());
                yield return new WaitForSeconds(1f);
                OCorrPlayerCanvas.SetActive(false);
                AddOtherCanvas.SetActive(true);
            }
        }
        else{
            Debug.Log("未連接到伺服器!");
        }
    }
    public void CallCorrTeam(){

    }
    public IEnumerator CorrTeam(int TeamID, string TeamName){
        WWWForm form = new WWWForm();
        form.AddField("UserID", UserID);
        form.AddField("TeamID", TeamID);
        form.AddField("TeamName", TeamName);

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/CorrectTeam", form);
        yield return www.SendWebRequest();

        Return result = new Return();
        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            result = JsonUtility.FromJson<Return>(response);
            if(result.success == false){
                switch (result.situation){
                    
                    case -1:
                        Debug.Log("參數傳送錯誤!"); 
                        CorrTeamWarn.text = "參數傳送錯誤!";
                        break;
                    case -2:
                        Debug.Log("資料庫錯誤!"); 
                        CorrTeamWarn.text = "資料庫錯誤!";
                        break;
                    case -3:
                        Debug.Log("帳號不存在!"); 
                        CorrTeamWarn.text = "帳號不存在!";
                        break;
                    case -4:
                        Debug.Log("該球員名稱已被使用!"); 
                        CorrTeamWarn.text = "該球員名稱已被使用!";
                        break;
                    case -5:
                        Debug.Log("該背號已被使用!"); 
                        CorrTeamWarn.text = "該背號已被使用!";
                        break;
                    case -6:
                        Debug.Log("修改球員的資料不存在!");
                        CorrTeamWarn.text = "修改球員的資料不存在!";
                        break;
                }
            }
            else{
                Debug.Log("Success!");
                yield return StartCoroutine(UpdateUserData());
                CorrTeamWarn.text = "修正成功!";
                yield return StartCoroutine(DealOtherView());
                yield return new WaitForSeconds(1f);
                CorrTeamCanvas.SetActive(false);
                AddOtherCanvas.SetActive(true);
            }
        }
        else{
            Debug.Log("未連接到伺服器!");
        }
    }
    
    public GameObject SetGameInfoCanvas;
    public Dropdown ServeChoice;
    public Dropdown SideChoice;
    public void GoToMainScene(){
        if(AssignPlayerID.Count != 12 || OtherAssignPlayerID.Count != 12){
            OtherTeamWarning.text = "敵方球員人數不足12!";
        }
        else{
            UserData.Instance.EnemyPlayerID = OtherAssignPlayerID;
            UserData.Instance.EnemyPlayerName = OtherAssignPlayerName;
            UserData.Instance.EnemyPlayerNumber = OtherAssignPlayerNumber;
            UserData.Instance.EnemyPlayerPlayPos = OtherAssignPlayerPos;
            SetGameInfoCanvas.SetActive(true);
            AssignOthTeamCanvas.SetActive(false);
        }
    }

    public void CallSetGameInfo(){
        int FirstSide = SideChoice.value;
        int ServeSide = ServeChoice.value;
        int leftTeamNum = FirstSide == 0 ? UserTeamID : OtherAssignPlayerID[0];
        int rightTeamNum = FirstSide == 1 ? UserTeamID : OtherAssignPlayerID[0];
        
        StartCoroutine(SetGameInfo(FirstSide, ServeSide, leftTeamNum, rightTeamNum));
    }
    public Text ComfirmWarn;
    public IEnumerator SetGameInfo(int FirstSide, int ServeSide, int leftTeamNum, int rightTeamNum){
        WWWForm form = new WWWForm();
        form.AddField("UserID", UserData.Instance.UserID);
        form.AddField("GameID", UserData.Instance.GameID);
        form.AddField("FirstSide", FirstSide);
        form.AddField("ServeSide", ServeSide);
        form.AddField("leftTeamNum", leftTeamNum);
        form.AddField("rightTeamNum", rightTeamNum);

        form.AddField("PL1", AssignPlayerID[0]);
        form.AddField("PL2", AssignPlayerID[1]);
        form.AddField("PL3", AssignPlayerID[2]);
        form.AddField("PL4", AssignPlayerID[3]);
        form.AddField("PL5", AssignPlayerID[4]);
        form.AddField("PL6", AssignPlayerID[5]);

        form.AddField("PR1", OtherAssignPlayerID[0]);
        form.AddField("PR2", OtherAssignPlayerID[1]);
        form.AddField("PR3", OtherAssignPlayerID[2]);
        form.AddField("PR4", OtherAssignPlayerID[3]);
        form.AddField("PR5", OtherAssignPlayerID[4]);
        form.AddField("PR6", OtherAssignPlayerID[5]);
        
        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/SetGameInfo", form);
        yield return www.SendWebRequest();

        Return result = new Return();
        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            print(response);
            result = JsonUtility.FromJson<Return>(response);
            if(result.success == false){
                switch (result.situation){
                    case -1:
                        Debug.Log("參數傳送錯誤!"); 
                        ComfirmWarn.text = "參數傳送錯誤!";
                        break;
                    case -2:
                        Debug.Log("已設定過比賽資訊!"); 
                        ComfirmWarn.text = "已設定過比賽資訊!";
                        break;
                    case -3:
                        Debug.Log("資料庫錯誤!"); 
                        ComfirmWarn.text = "資料庫錯誤!";
                        break;
                    case -4:
                        Debug.Log("帳號不存在!"); 
                        ComfirmWarn.text = "帳號不存在!";
                        break;
                    case -5: case -6: case -7:
                        Debug.Log("資料庫錯誤!");
                        ComfirmWarn.text = "資料庫錯誤!";
                        break;
                }
            }
            else{
                ComfirmWarn.text = "成功!";
                Test();
            }
        }
        else{
            print("未連接到伺服器!");
            ComfirmWarn.text = "未連接到伺服器!";
        }

        yield return null;
    }
    public void ClickBack(){
        SetGameInfoCanvas.SetActive(false);
        MainCanvas.SetActive(true);
    }
    public void Test(){
        SceneManager.LoadScene("MainScene");
    }

}
