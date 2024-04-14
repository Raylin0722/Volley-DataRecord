using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.Linq;
using TMPro;


public class ClickRecord : MonoBehaviour
{
    public struct ClickData{
        public string formation;
        public List<GameObject> players; // 動作球員
        public List<Vector2> clicks;
        public int side; // 左右方 左:0 右:1
        public bool complete; // 是否記錄完成
        public int behavior; // 動作 發球:-1 接球:1 攻擊:2 攔網:3 
        public int clickType;
        
    };
    const int LEFT = 0, RIGHT = 1;
    const float doubleClick = 0.1f, LongClick = 0.5f;
    public List<ClickData> Behavior;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject database;
    [SerializeField] GameObject system;
    [SerializeField] GameObject[] NetLocate;
    [SerializeField] GameObject selectBlock;
    [SerializeField] GameObject pin;
    [SerializeField] GameObject insertBtn;
    [SerializeField] GameObject changePosBtn;
    [SerializeField] GameObject changePlayerBtn;
    [SerializeField] GameObject showDataBtn;
    


    public Vector3[] NetLocateXY; 
    public int[] touchCount;
    private bool isDrag;
    Vector3 startWorldPos, endWorldPos, startPos, endPos;
    SystemData SystemScript;
    dealDB DataBaseScript;

    Vector2[] ServePos;

    void Awake(){
        SystemScript = system.GetComponent<SystemData>();
        Behavior = new List<ClickData>();
        selectBlock.SetActive(false);
        isDrag = false;
        touchCount = new int[2];
        DataBaseScript = database.GetComponent<dealDB>();

        ServePos = new Vector2[2];
        ServePos[0] = new Vector2(317.7997f, 580.384f);
        ServePos[1] = new Vector2(328.9776f, 584.1694f);

    }
    void Start()
    {
        NetLocateXY = new Vector3[NetLocate.Length];
       
        
        // 前2網 後4左上右上左下右下
        for(int i = 0; i < NetLocate.Length; i++){
            NetLocateXY[i] = NetLocate[i].transform.position;
        }
        
        ClickData Serve = new ClickData();
        Serve.behavior = -1;
        Serve.complete = false;
        Serve.clickType = -1;
        Serve.players = new List<GameObject>();
        Serve.clicks = new List<Vector2>();
        //Serve.side = canvas.GetComponent<RefreshPoint>().whoServe;
        Serve.side = UserData.Instance.whoServe;
        print(canvas.GetComponent<RefreshPoint>().whoServe);
        print(Serve.side);
        if(Serve.side == LEFT){
            Serve.players.Add(SystemScript.leftPlayers[0]);
            SystemScript.leftPlayers[0].GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
        }
        else{
            Serve.players.Add(SystemScript.rightPlayers[0]);
            SystemScript.rightPlayers[0].GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
        
        }

        Behavior.Add(Serve);
        
        //print(string.Format("({0}, {1})", NetLocateXY[2].x, NetLocateXY[2].y));
        //print(string.Format("({0}, {1})", NetLocateXY[3].x, NetLocateXY[3].y));
        //print(string.Format("({0}, {1})", NetLocateXY[4].x, NetLocateXY[4].y));
        //print(string.Format("({0}, {1})", NetLocateXY[5].x, NetLocateXY[5].y));
    }
    Vector3 GetWorldPositionFromUI(RectTransform uiElement)
    {
        // 使用 RectTransformUtility 將 UI 元素座標轉換為世界座標
        Vector3[] corners = new Vector3[4];
        uiElement.GetWorldCorners(corners);
        Vector3 uiWorldPosition = (corners[0] + corners[2]) / 2f;

        return uiWorldPosition;
    }
    // Update is called once per frame
    void Update()
    {
        if(SystemScript.changePlayer){
            return;
        }
        if(!SystemScript.changePosition){
            if(Input.GetMouseButtonDown(0)){
                startWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                startPos = Input.mousePosition;
            }
            if(Input.GetMouseButtonUp(0)){
                isDrag = false;
                selectBlock.SetActive(false);
                endWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                endPos = Input.mousePosition;
                GetClickTarget();
                
            }   
        }
    }

    int countL = 0, countR = 0;
    void GetClickTarget(){

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        // 球員
        if (hit.collider != null && Behavior.Count != 1)
        {
            
            return;
        }
        
        if(inField(mousePosition))
        {
            if(Behavior.Last().complete == true)
                return;
            
            pin.SetActive(true);
            Vector3 tmpPos = mousePosition;
            RectTransform pinRect = pin.GetComponent<RectTransform>();
            tmpPos.y += pinRect.rect.height / 200f;
            pinRect.position = tmpPos;
            
            int side = leftOrRight(mousePosition);
            bool inOrout = inField(mousePosition);
            string clickSide = (side == LEFT) ? "Left" : "Right";
            ClickData target = Behavior[Behavior.Count - 1];
            string leftFormation = "", rightFormation = "";
            for(int i = 0; i < 6; i++){
                leftFormation += ("L" + SystemScript.leftPlayers[i].GetComponent<dragPlayer>().playerNum + " ");
                rightFormation += ("R" + SystemScript.rightPlayers[i].GetComponent<dragPlayer>().playerNum + " ");
            }
            target.formation = leftFormation + rightFormation;
            if(Behavior.Last().behavior == -1 && inOrout){ // 發球
                print("Serve finish");
                target.complete = true;
                target.clicks.Add(new Vector2(mousePosition.x, mousePosition.y));
                target.behavior = -1;
                Behavior.RemoveAt(Behavior.Count - 1);
                Behavior.Add(target);
                Behavior.Last().players[0].GetComponent<SpriteRenderer>().color = Behavior.Last().players[0].GetComponent<dragPlayer>().orginColor;
                changePlayerBtn.GetComponent<Button>().interactable = false;
                changePosBtn.GetComponent<Button>().interactable = false;
                showDataBtn.GetComponent<Button>().interactable = false;
                for(int i = 0; i < 6; i++){
                    if(SystemScript.leftGamePos[i].x != -1 && SystemScript.leftGamePos[i].y != -1){
                        SystemScript.leftPlayers[i].transform.position = SystemScript.leftGamePos[i];
                        print(String.Format("{0} {1} {2}", SystemScript.leftGamePos[i].x, SystemScript.leftGamePos[i].y, SystemScript.leftGamePos[i].z));
                    }
                    if(SystemScript.rightGamePos[i].x != -1 && SystemScript.rightGamePos[i].y != -1){
                        SystemScript.rightPlayers[i].transform.position = SystemScript.rightGamePos[i];
                        print(String.Format("{0} {1} {2}", SystemScript.rightGamePos[i].x, SystemScript.rightGamePos[i].y, SystemScript.rightGamePos[i].z));
                    }
                }
            }
            else if(inOrout){ //場內 紀錄
                // 依照點擊類型 地板點擊次數 球員點擊次數判斷動作
                if(target.clickType == 1){ // CLICK
                    if(clickSide == target.players.Last().tag){ // 單擊一次 同側 接球
                        print("Catch");
                        target.behavior = 1;
                        if(target.side == LEFT){
                            countR = 0;
                            countL++;
                        }   
                        else if(target.side == RIGHT){
                            countL = 0;
                            countR++;
                        } 
                        for(int i = 0; i < 6; i++){
                            if(target.side == LEFT && SystemScript.leftPlayers[i].GetComponent<dragPlayer>().playerPlayPos == 2 && countL == 1){
                                SystemScript.leftPlayers[i].GetComponent<dragPlayer>().flashing[0] = true;
                                SystemScript.leftPlayers[i].GetComponent<dragPlayer>().flashTime[0] = 0f;
                            }
                           
                            else if(target.side == RIGHT && SystemScript.rightPlayers[i].GetComponent<dragPlayer>().playerPlayPos == 2 && countR == 1 ){
                                SystemScript.rightPlayers[i].GetComponent<dragPlayer>().flashing[0] = true;
                                SystemScript.rightPlayers[i].GetComponent<dragPlayer>().flashTime[0] = 0f;
                            }
                            
                        }
                    
                    }
                    else{  // 單擊一次 異側 攻擊
                        print("Attack");
                        target.behavior = 2;
                        if(target.side == LEFT){
                            countL++;
                            countR = 0;
                            SystemScript.rightPlayers[1].GetComponent<dragPlayer>().flashing[0] = true;
                            SystemScript.rightPlayers[2].GetComponent<dragPlayer>().flashing[0] = true;
                            SystemScript.rightPlayers[3].GetComponent<dragPlayer>().flashing[0] = true;
                        
                        }
                        else if(target.side == RIGHT){
                            countR++;
                            countL = 0;
                            SystemScript.leftPlayers[1].GetComponent<dragPlayer>().flashing[0] = true;
                            SystemScript.leftPlayers[2].GetComponent<dragPlayer>().flashing[0] = true;
                            SystemScript.leftPlayers[3].GetComponent<dragPlayer>().flashing[0] = true;
                            
                        }
                    }     
                }
                else if(target.clickType == 2){ // DOUBLE CLICK
                    print("block");
                    countL = 0;
                    countR = 0;
                    target.behavior = 3;
                }
                else if(target.clickType == 3){ // PRESS

                }
                target.clicks.Add(new Vector2(mousePosition.x, mousePosition.y));
                target.complete = true;
                touchCount[target.players.Last().tag == "Left" ? LEFT : RIGHT]++; 
                for(int i = 0; i < target.players.Count; i++){
                    target.players[i].GetComponent<dragPlayer>().isSelect[0] = false;
                }
                
                Behavior.RemoveAt(Behavior.Count - 1);
                Behavior.Add(target);
                for(int i = 0; i < Behavior.Last().players.Count; i++){
                    
                    Behavior.Last().players[i].GetComponent<SpriteRenderer>().color = Behavior.Last().players[i].GetComponent<dragPlayer>().orginColor;
                    
                }
                
            }
            for(int i = 0; i < 6; i++){
                SystemScript.leftPlayers[i].SetActive(true);
                SystemScript.rightPlayers[i].SetActive(true);
            }
            if(Behavior.Last().players.Count == 1 && Behavior.Last().behavior != 3)
                Behavior.Last().players[0].SetActive(false);
        }
    }
    bool IsPointerOverUI()
    {
        // 使用 EventSystem 检查是否点击在 UI 上
        return EventSystem.current.IsPointerOverGameObject();
    }
    string GetUITagUnderPointer()
    {
        // 获取当前鼠标位置的 Raycast 结果
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        // 通过 EventSystem 获取当前 Raycast 结果
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        // 获取点击的 UI 元素的标签
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("sitePlan"))
            {
                return result.gameObject.tag;
            }
        }

        return string.Empty; // 如果没有找到对应标签的 UI 元素，返回空字符串
    }
    bool inField(Vector3 pos){
        if(pos.x > NetLocateXY[2].x && pos.x < NetLocateXY[3].x && pos.y < NetLocateXY[2].y && pos.y > NetLocateXY[4].y)
            return true;

        return false;
    }
    int leftOrRight(Vector3 pos){
        if(pos.x >= NetLocateXY[0].x)
            return RIGHT;
        
        return LEFT;
    }
    int checkBehaviorValid(ClickData target){ // -1 連擊次數錯誤
        if(Behavior.Count == 0)
            return 0;
        if(Behavior.Last().players.Last().tag == "Left" && touchCount[LEFT] > 3)
            return -1;
        else if(Behavior.Last().players.Last().tag == "Right" && touchCount[RIGHT] > 3 )
            return -1;
        
        return 0;
    }

    public void clickInsert(){

        canvas.gameObject.GetComponent<RefreshPoint>().rotate();
        pin.SetActive(false);
        Behavior.Clear();
        ClickData Serve = new ClickData();
        Serve.behavior = -1;
        Serve.complete = false;
        Serve.clickType = -1;
        Serve.players = new List<GameObject>();
        Serve.clicks = new List<Vector2>();
        Serve.side = canvas.GetComponent<RefreshPoint>().whoServe;
        

        if(Serve.side == LEFT){
            Serve.players.Add(SystemScript.leftPlayers[0]);
            SystemScript.leftPlayers[0].GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
            SystemScript.leftPlayers[0].SetActive(true);
        }
        else{
            Serve.players.Add(SystemScript.rightPlayers[0]);
            SystemScript.rightPlayers[0].GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
            SystemScript.rightPlayers[0].SetActive(true);
        }
        Behavior.Add(Serve);
        


    }

    public void refreshPlayer(){
        insertBtn.GetComponent<Button>().interactable = true;
        changePosBtn.GetComponent<Button>().interactable = true;
        changePlayerBtn.GetComponent<Button>().interactable = true;
        showDataBtn.GetComponent<Button>().interactable = true;
        for(int i = 0; i < 6; i++){
            SystemScript.leftPlayers[i].SetActive(true);
            SystemScript.rightPlayers[i].SetActive(true);
            SystemScript.leftPlayers[i].GetComponent<dragPlayer>().flashing[0] = false;
            SystemScript.leftPlayers[i].GetComponent<dragPlayer>().flashTime[0] = 0f;
            SystemScript.rightPlayers[i].GetComponent<dragPlayer>().flashing[0] = false;
            SystemScript.rightPlayers[i].GetComponent<dragPlayer>().flashTime[0] = 0f;
            SystemScript.leftPlayers[i].GetComponent<SpriteRenderer>().color = SystemScript.leftPlayers[i].GetComponent<dragPlayer>().orginColor;
            SystemScript.rightPlayers[i].GetComponent<SpriteRenderer>().color = SystemScript.rightPlayers[i].GetComponent<dragPlayer>().orginColor;
        }
    }

    public void clickRectoData(){
        int round = SystemScript.score[LEFT] + SystemScript.score[RIGHT] + 1;
        dealDB.Data tmp;
        int teamNum = Behavior[0].side == LEFT ? SystemScript.leftTeamNum[0] : SystemScript.rightTeamNum[0];
        Vector2 tmpEnd = Behavior[0].clicks[0];
        Vector2 tmpStart = ServePos[Behavior[0].side];
        
        if(Behavior[0].side == LEFT){
            tmpStart.x = ((tmpStart.x - NetLocateXY[2].x) * 500) / (NetLocateXY[3].x - NetLocateXY[2].x);
            tmpStart.y = ((NetLocateXY[2].y - tmpStart.y) * 800) / (NetLocateXY[2].y - NetLocateXY[4].y);
            tmpEnd.x = ((tmpEnd.x - NetLocateXY[2].x) * 500) / (NetLocateXY[3].x - NetLocateXY[2].x);
            tmpEnd.y = ((NetLocateXY[2].y - tmpEnd.y)  * 800) / (NetLocateXY[2].y - NetLocateXY[4].y);
        }
        else{
            tmpStart.x = ((-tmpStart.x + NetLocateXY[5].x) * 500) / (NetLocateXY[3].x - NetLocateXY[2].x);
            tmpStart.y = ((-NetLocateXY[5].y + tmpStart.y) * 800) / (NetLocateXY[2].y - NetLocateXY[4].y);
            tmpEnd.x = ((-tmpEnd.x + NetLocateXY[5].x) * 500) / (NetLocateXY[3].x - NetLocateXY[2].x);
            tmpEnd.y = ((-NetLocateXY[5].y + tmpEnd.y)  * 800) / (NetLocateXY[2].y - NetLocateXY[4].y);
        }

        tmp = new dealDB.Data(
            Behavior[0].formation, round, Behavior[0].players, teamNum,
            (int)tmpStart.x, (int)tmpStart.y, 
            (int)tmpEnd.x, (int)tmpEnd.y,
            Behavior[0].behavior, 0, Behavior[0].side
        );
        DataBaseScript.saveData.Add(tmp);
        
        for(int i = 1; i < Behavior.Count; i++){
            teamNum = Behavior[i].side == LEFT ? SystemScript.leftTeamNum[0] : SystemScript.rightTeamNum[0];
            tmpEnd = Behavior[i].clicks[0];
            tmpStart = Behavior[i - 1].clicks[0];
            if(Behavior[i].side == LEFT){
                tmpStart.x = ((tmpStart.x - NetLocateXY[2].x) * 500) / (NetLocateXY[3].x - NetLocateXY[2].x);
                tmpStart.y = ((NetLocateXY[2].y - tmpStart.y) * 800) / (NetLocateXY[2].y - NetLocateXY[4].y);
                tmpEnd.x = ((tmpEnd.x - NetLocateXY[2].x) * 500) / (NetLocateXY[3].x - NetLocateXY[2].x);
                tmpEnd.y = ((NetLocateXY[2].y - tmpEnd.y)  * 800) / (NetLocateXY[2].y - NetLocateXY[4].y);
            }
            else{
                tmpStart.x = ((-tmpStart.x + NetLocateXY[5].x) * 500) / (NetLocateXY[3].x - NetLocateXY[2].x);
                tmpStart.y = ((-NetLocateXY[5].y + tmpStart.y) * 800) / (NetLocateXY[2].y - NetLocateXY[4].y);
                tmpEnd.x = ((-tmpEnd.x + NetLocateXY[5].x) * 500) / (NetLocateXY[3].x - NetLocateXY[2].x);
                tmpEnd.y = ((-NetLocateXY[5].y + tmpEnd.y)  * 800) / (NetLocateXY[2].y - NetLocateXY[4].y);
            }
            
            tmp = new dealDB.Data(
                Behavior[i].formation, round, Behavior[i].players, teamNum,
                (int)tmpStart.x, (int)tmpStart.y, 
                (int)tmpEnd.x, (int)tmpEnd.y,
                Behavior[i].behavior, 0, Behavior[i].side
            );

            DataBaseScript.saveData.Add(tmp);
        }

        tmp = DataBaseScript.saveData.Last();
        DataBaseScript.saveData.RemoveAt(DataBaseScript.saveData.Count - 1);   
        if(canvas.GetComponent<RefreshPoint>().reClick == LEFT)
           tmp.score = 1;
        else
            tmp.score = -1;
        DataBaseScript.saveData.Add(tmp);

    }
    public void deleteNewData(){    
        if(DataBaseScript.saveData.Count == 0 && Behavior.Last().complete == false && Behavior.Count == 1)
            return;
        if(canvas.GetComponent<RefreshPoint>().reClick == -1){ // 沒按得分
            if(Behavior.Count == 1){
                Behavior.RemoveAt(Behavior.Count - 1);
                ClickData Serve = new ClickData();
                Serve.behavior = -1;
                Serve.complete = false;
                Serve.clickType = -1;
                Serve.players = new List<GameObject>();
                Serve.clicks = new List<Vector2>();
                Serve.side = canvas.GetComponent<RefreshPoint>().whoServe;
                if(Serve.side == LEFT){
                    Serve.players.Add(SystemScript.leftPlayers[0]);
                    SystemScript.leftPlayers[0].GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
                }
                else{
                    Serve.players.Add(SystemScript.rightPlayers[0]);
                    SystemScript.rightPlayers[0].GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
                }
                for(int i = 0; i < 6; i++){
                    SystemScript.leftPlayers[i].SetActive(true);
                    SystemScript.rightPlayers[i].SetActive(true);
                }
                pin.SetActive(false);
                Behavior.Add(Serve);
                changePlayerBtn.GetComponent<Button>().interactable = true;
                changePosBtn.GetComponent<Button>().interactable = true;
                showDataBtn.GetComponent<Button>().interactable = true;
            }
            else{
                Behavior.RemoveAt(Behavior.Count - 1);
                for(int i = 0; i < 6; i++){
                    SystemScript.leftPlayers[i].SetActive(true);
                    SystemScript.rightPlayers[i].SetActive(true);
                }
                ClickData tmp = Behavior.Last();
                pin.GetComponent<RectTransform>().transform.position = tmp.clicks[0];
                if(tmp.behavior != 3 )
                    tmp.players[0].SetActive(false);
                
            }
            
        }
        else{ // 有按得分
            canvas.GetComponent<RefreshPoint>().back();
        }
        if(DataBaseScript.saveData.Count > 0)
            DataBaseScript.saveData.Clear();


    }
}
