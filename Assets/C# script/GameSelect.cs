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

    public class ServerToUser{
        public bool success;
        public int situation;
        public List<int> UserPlayerID;
        public List<string> UserPlayerName;
        public List<int> UserPlayerNumber;
        public List<int> UserGameID;
        public List<string> UserGameDate;
        public List<string> UserGameName;
    }

    public class Return{
        public bool success;
        public int situation;
    }

    public int numOfGame;
    public int numOfPlayer;
    public int UserID;
    public string UserName;
    public List<int> UserPlayerID;
    public List<string> UserPlayerName;
    public List<int> UserPlayerNumber;
    public List<int> UserGameID;
    public List<string> UserGameDate;
    public List<string> UserGameName;

    private void Awake(){
        UserName = UserData.Instance.UserName; //後面要連伺服器
        numOfGame = UserData.Instance.numOfGame; // 後面要連伺服器
        numOfPlayer = UserData.Instance.numOfPlayer;
        UserID = UserData.Instance.UserID;
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
        StartCoroutine(DealView());
    }

    public InputField PName;
    public InputField PNum;
    public InputField GName;
    public TMP_InputField GDate;
    public Text PWarning;
    public Text GWarning;
    public GameObject CorrTarget;
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
        if(PNumIN<=0 || PNumIN >= 100){
            PWarning.text = "背號請輸入 0 - 100!";
            Debug.Log("背號請輸入 0 - 100!");
            return;
        }

        StartCoroutine(AddPlayer(PNumIN, PNameIn));
        
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
        StartCoroutine(CorrectPlayer(PCorrNumIN, PCorrNameIn, PlayerID));


    }
    public void CallUpdateUserData(){
        StartCoroutine(UpdateUserData());
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
    public InputField TeamName;
    public Text TeamWarning;
    public void ClickGame(){
        
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        int GameID = obj.GetComponent<ID>().ObjID;
        for(int i = 0; i < UserGameID.Count; i++){
            if(UserGameID[i] == GameID){
                UserData.Instance.GameName = UserGameName[i];
                break;
            }
        }
        UserData.Instance.GameID = GameID;


        NotAssignPlayerID = UserPlayerID;
        NotAssignPlayerName = UserPlayerName;
        NotAssignPlayerNumber = UserPlayerNumber;
        MainCanvas.SetActive(false);
        AssignCanvas.SetActive(true);
        StartCoroutine(ShowNotAssign());
        //SceneManager.LoadScene("MainScene");

    }
    public void GoToMainScene(){
        
        Regex regexName = new Regex("^[\u4e00-\u9fa50-9A-Za-z_]+$");
        if(string.IsNullOrEmpty(TeamName.text)){
            TeamWarning.text = "隊伍名稱欄位不能為空";
            Debug.Log("隊伍名稱欄位不能為空");
            return;
        }
        string TeamNameIn = TeamName.text;
        if(!regexName.IsMatch(TeamName.text)){
            TeamWarning.text = "隊伍名稱只能輸入 中文 數字 大小寫英文";
            Debug.Log("隊伍名稱只能輸入 中文 數字 大小寫英文");
            return;
        }

        TeamWarning.text = "設定成功!";
        UserData.Instance.TeamName = TeamNameIn;
        waitsecond(1f);
        
        if(AssignPlayerID.Count != 12){
            WarningMsg.text = "已設定人數不為12人 若選擇進入分析系統 需在系統內將人數 補足6人以上才可使用"; 
            AssignCanvas.SetActive(false);
            WarnCanvas.SetActive(true);
        }
        else{
            UserData.Instance.UserPlayerID = AssignPlayerID;
            UserData.Instance.UserPlayerName = AssignPlayerName;
            UserData.Instance.UserPlayerNumber = AssignPlayerNumber;
            SceneManager.LoadScene("MainScene");
        }
        

        
    }
    public Text WarningMsg;
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
    public IEnumerator UpdateUserData(){

        WWWForm form = new WWWForm();
        form.AddField("account", UserName);
        form.AddField("UserID", UserID);

        UnityWebRequest www = UnityWebRequest.Post("http://192.168.17.66:5000/UpdateUserData", form);

        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
            print(response);
            UserPlayerID = new List<int>();
            UserPlayerName = new List<string>();
            UserPlayerNumber = new List<int>();
            UserGameID = new List<int>();
            UserGameDate = new List<string>();
            UserGameName = new List<string>();
            ServerToUser userRetuen = JsonUtility.FromJson<ServerToUser>(response);
            Debug.Log(response);
            if(userRetuen.success == true){
                for(int i = 0; i < userRetuen.UserPlayerID.Count; i++){
                    UserPlayerID.Add(userRetuen.UserPlayerID[i]);
                    UserPlayerNumber.Add(userRetuen.UserPlayerNumber[i]);
                    UserPlayerName.Add(userRetuen.UserPlayerName[i]);
                }
                for(int i = 0; i < userRetuen.UserGameID.Count; i++){
                    UserGameID.Add(userRetuen.UserGameID[i]);
                    UserGameName.Add(userRetuen.UserGameName[i]);
                    DateTimeOffset dateTimeOffset = DateTimeOffset.ParseExact(userRetuen.UserGameDate[i], "ddd, dd MMM yyyy HH:mm:ss 'GMT'", 
                        System.Globalization.CultureInfo.InvariantCulture);
                    string formattedDate = dateTimeOffset.ToString("yyyy-MM-dd");
                    UserGameDate.Add(formattedDate);
                }
                Debug.Log("Success!");
            }
            else if(userRetuen.success == false){
                switch (userRetuen.situation){
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
    public IEnumerator AddPlayer(int PlayerNumber, string PlayerName){
        WWWForm form = new WWWForm();
        form.AddField("account", UserName);
        form.AddField("UserID", UserID);
        form.AddField("PlayerNumber", PlayerNumber);
        form.AddField("PlayerName", PlayerName);

        UnityWebRequest www = UnityWebRequest.Post("http://192.168.17.66:5000/AddPlayer", form);
        yield return www.SendWebRequest();

        Return result = new Return();
        if(www.result == UnityWebRequest.Result.Success){
            string response = www.downloadHandler.text;
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
                        Debug.Log("該球員已存在!"); 
                        PWarning.text = "該球員已存在!";
                        break;
                    case -5:
                        Debug.Log("該背號已存在!"); 
                        PWarning.text = "該背號已存在!";
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
    public IEnumerator AddGame(string GameDate, string GameName){
        yield return new WaitForSeconds(1f);
        WWWForm form = new WWWForm();
        form.AddField("account", UserName);
        form.AddField("UserID", UserID);
        form.AddField("GameDate", GameDate);
        form.AddField("GameName", GameName);

        UnityWebRequest www = UnityWebRequest.Post("http://192.168.17.66:5000/AddGame", form);
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
        }
        else if(obj.tag == "CorrPlayer"){
            MainCanvas.SetActive(true);
            CorrPlayerCanvas.SetActive(false);
        }
        else if(obj.tag == "AssignPlayer"){
            MainCanvas.SetActive(true);
            AssignCanvas.SetActive(false);
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
            newPlayer.GetComponent<ID>().ObjID = UserPlayerID[i];
            newPlayer.GetComponent<Button>().onClick.AddListener(ClickPlayer);

            // 設定Prefab的位置
            RectTransform rectTransform = newPlayer.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, -i * prefabHeight);
        }
        GameContent.sizeDelta = new Vector2(GameContent.sizeDelta.x, 0 * prefabHeight);
        PlayerContent.sizeDelta = new Vector2(PlayerContent.sizeDelta.x, 0 * prefabHeight);

        yield return null;
    }
    
    public InputField PCorrName;
    public InputField PCorrNum;
    public Text PCWarning;
    public IEnumerator CorrectPlayer(int PlayerNumber, string PlayerName, int PlayerID){
        WWWForm form = new WWWForm();
        form.AddField("account", UserName);
        form.AddField("UserID", UserID);
        form.AddField("PlayerID", PlayerID);
        form.AddField("PlayerNumber", PlayerNumber);
        form.AddField("PlayerName", PlayerName);

        UnityWebRequest www = UnityWebRequest.Post("http://192.168.17.66:5000/CorrectPlayer", form);
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
                        Debug.Log("修該球員的資料不存在!");
                        PCWarning.text = "修該球員的資料不存在!";
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
    
    public List<int> NotAssignPlayerID;
    public List<string> NotAssignPlayerName;
    public List<int> NotAssignPlayerNumber;
    public List<int> AssignPlayerID;
    public List<string> AssignPlayerName;
    public List<int> AssignPlayerNumber;
    public RectTransform AddContent;
    public RectTransform NotAddContent;
    public GameObject AssignScrollView;
    public GameObject NotAssignScrollView;
    
    public IEnumerator ShowNotAssign(){
        for (int i = 0; i < AddContent.childCount; i++){
            Destroy(AddContent.GetChild(i).gameObject);
        }

        for (int i = 0; i < NotAddContent.childCount; i++){
            Destroy(NotAddContent.GetChild(i).gameObject);
        }
        GameScrollview.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
        PlayerScrollview.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
        for (int i = 0; i < AssignPlayerID.Count; i++)
        {

            GameObject NewAssign = Instantiate(Player, AddContent);
            Text[] PlayerTexts = NewAssign.GetComponentsInChildren<Text>();

            PlayerTexts[0].text = i + 1 + ".   " + AssignPlayerNumber[i].ToString();
            PlayerTexts[1].text = AssignPlayerName[i];
            NewAssign.GetComponent<ID>().ObjID = AssignPlayerID[i];
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
            NewCanAssign.GetComponent<ID>().ObjID = NotAssignPlayerID[i];
            NewCanAssign.GetComponent<Button>().onClick.AddListener(Assign);

            // 設定Prefab的位置
            RectTransform rectTransform = NewCanAssign.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, -i * prefabHeight);
        }
        AddContent.sizeDelta = new Vector2(GameContent.sizeDelta.x, 0 * prefabHeight);
        NotAddContent.sizeDelta = new Vector2(PlayerContent.sizeDelta.x, 0 * prefabHeight);

        yield return null;
    }
    public void Assign(){
        print("Assign");
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        int PlayerID = obj.GetComponent<ID>().ObjID;
        for(int i = 0; i < NotAssignPlayerID.Count; i++){
            if(PlayerID == NotAssignPlayerID[i]){
                AssignPlayerID.Add(NotAssignPlayerID[i]);
                AssignPlayerName.Add(NotAssignPlayerName[i]);
                AssignPlayerNumber.Add(NotAssignPlayerNumber[i]);

                NotAssignPlayerID.RemoveAt(i);
                NotAssignPlayerName.RemoveAt(i);
                NotAssignPlayerNumber.RemoveAt(i);

                break;
            }
        }
        StartCoroutine(ShowNotAssign());
    }
    public void CancelAssign(){
        print("CancelAssign");
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        int PlayerID = obj.GetComponent<ID>().ObjID;
        for(int i = 0; i < AssignPlayerID.Count; i++){
            if(PlayerID == AssignPlayerID[i]){
                NotAssignPlayerID.Add(AssignPlayerID[i]);
                NotAssignPlayerName.Add(AssignPlayerName[i]);
                NotAssignPlayerNumber.Add(AssignPlayerNumber[i]);

                AssignPlayerID.RemoveAt(i);
                AssignPlayerName.RemoveAt(i);
                AssignPlayerNumber.RemoveAt(i);

                break;
            }
        }
        StartCoroutine(ShowNotAssign());
    }
    public void CallDealView(){
        StartCoroutine(DealView());
    }

}
