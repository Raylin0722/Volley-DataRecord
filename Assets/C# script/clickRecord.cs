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
        public List<GameObject> players; // 動作球員
        public List<Vector2> clicks;
        public int side; // 左右方 左:0 右:1
        public bool complete; // 是否記錄完成
        public int behavior; // 動作 發球:-1 接球:1 攻擊:2 攔網:3 
        public int touchFieldCount;
        public int clickType;
        
    };
    const int LEFT = 0, RIGHT = 1;
    const float doubleClick = 0.1f, LongClick = 0.5f;
    public List<ClickData> Behavior;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject database;
    [SerializeField] GameObject system;
    [SerializeField] GameObject[] CanBlock;
    [SerializeField] GameObject[] NetLocate;
    [SerializeField] GameObject selectBlock;
    [SerializeField] GameObject pin;


    Vector3[] NetLocateXY; 
    Vector3[] CanBlockXY;
    public int[] touchCount;
    private bool isDrag;
    Vector3 startWorldPos, endWorldPos, startPos, endPos;
    SystemData SystemScript;
    void Awake(){
        SystemScript = system.GetComponent<SystemData>();
        Behavior = new List<ClickData>();
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
        Serve.touchFieldCount = 0;
        Behavior.Add(Serve);
        selectBlock.SetActive(false);
        isDrag = false;
        touchCount = new int[2];
        
    }
    void Start()
    {
        NetLocateXY = new Vector3[NetLocate.Length];
        CanBlockXY = new Vector3[CanBlock.Length];
        
        // 前2網 後4左上右上左下右下
        for(int i = 0; i < NetLocate.Length; i++){
            NetLocateXY[i] = NetLocate[i].transform.position;
        }
        for(int i = 0; i < CanBlock.Length; i++){
            CanBlockXY[i] = CanBlock[i].transform.position;
        }   
        
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
        if(!SystemScript.changePosition){
            if(Input.GetMouseButtonDown(0)){
                if(Behavior.Count > 1){ // 發球不能進行攔網
                    isDrag = true;
                    selectBlock.SetActive(true);
                }
                startWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                startPos = Input.mousePosition;

            }
            if(Input.GetMouseButtonUp(0)){
                isDrag = false;
                selectBlock.SetActive(false);
                endWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                endPos = Input.mousePosition;
                //selectBlock.SetActive(false);
                if(Math.Abs(startWorldPos.x - endWorldPos.x) > 5f || Math.Abs(startWorldPos.y - endWorldPos.y) > 5f){ // 有拖曳選取
                    
                }
                else{ // 非拖曳選取 
                    GetClickTarget();
                }
            }
            if(isDrag){
                endWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                endPos = Input.mousePosition;
                selectBlock.SetActive(true);
                Vector3 tmp = new Vector3((startPos.x + endPos.x) / 2, (startPos.y + endPos.y) / 2, 0);
                Vector3 size = new Vector3((startPos.x - endPos.x) / 100, (startPos.y - endPos.y) / 100, selectBlock.transform.localScale.z);
                tmp = Camera.main.ScreenToWorldPoint(tmp);
                //print(tmp);
                tmp.z = 0;
                selectBlock.transform.position = tmp;
                selectBlock.transform.localScale = size;       
            }
        }
    }
    void GetClickTarget(){

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        // 球員
        print(Behavior.Count);
        if (hit.collider != null && Behavior.Count != 1)
        {
            return;
        }

        // 地板
        if (IsPointerOverUI() && inField(mousePosition))
        {
            if(Behavior.Last().complete == true)
                return;
            
            pin.SetActive(true);
            Vector3 tmpPos = mousePosition;
            RectTransform pinRect = pin.GetComponent<RectTransform>();
            tmpPos.y += pinRect.rect.height / 200f;
            if(inField(tmpPos))
                pinRect.position = tmpPos;
            else
                pin.SetActive(false);

            int side = leftOrRight(mousePosition);
            bool inOrout = inField(mousePosition);
            string clickSide = (side == LEFT) ? "Left" : "Right";
            ClickData target = Behavior[Behavior.Count - 1];

            if(Behavior.Last().behavior == -1 && inOrout){ // 發球
                print("Serve finish");
                target.complete = true;
                target.clicks.Add(new Vector2(mousePosition.x, mousePosition.y));
                target.behavior = -1;
                Behavior.RemoveAt(Behavior.Count - 1);
                Behavior.Add(target);
                Behavior.Last().players[0].GetComponent<SpriteRenderer>().color = Behavior.Last().players[0].GetComponent<dragPlayer>().orginColor;

            }
            else if(inOrout){ //場內 紀錄
                // 依照點擊類型 地板點擊次數 球員點擊次數判斷動作
                if(target.clickType == 1){ // CLICK
                    if(clickSide == target.players.Last().tag){ // 單擊一次 同側 接球
                        print("Catch");
                        target.behavior = 1;
                    }
                    else{  // 單擊一次 異側 攻擊
                        print("Attack");
                        target.behavior = 2;
                    }     
                }
                else if(target.clickType == 2){ // DOUBLE CLICK
                    print("block");
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
            if(Behavior.Last().players.Count == 1)
                Behavior.Last().players[0].SetActive(false);
        }
        else
            print("None");
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
    bool FCheckValid(Vector3 pos){
        if(Behavior.Count == 0) // 沒有前置資料點擊地板無效
            return false;
        ClickData checkBehavior = Behavior.Last();
        if(checkBehavior.complete == true) // 前置資料已完成點擊地板無效
            return false;
        if(inField(pos) == false) // 點擊區域不在場內
            return false;
        
        // 檢查動作是否合法
        int sideClicked = leftOrRight(pos), behavior = checkBehavior.behavior;
        if(behavior == -1 && checkBehavior.touchFieldCount == 0){ // 發球
            return true;
        }
        else if(behavior == 0 && checkBehavior.touchFieldCount == 0){ // 接球
            return true;
        }
        else if(behavior == 1 && checkBehavior.touchFieldCount == 0){ // 舉球
            return true;
        }
        else if(behavior == 2 && checkBehavior.touchFieldCount == 0){ // 攻擊
            return true;
        }
        else if(behavior == 3 && checkBehavior.touchFieldCount == 0){ // 吊球
            return true;
        }
        else if(behavior == 4){ // 攔網
            if(checkBehavior.touchFieldCount == 0 && 
               checkBehavior.players.Last().tag != (sideClicked == LEFT ? "Left" : "Right")){
                return false;
            }
            else if(checkBehavior.touchFieldCount == 1){
                return true;
            }
            else if(checkBehavior.touchFieldCount >= 2)
                return false;
        }
        
        
        return true; 
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
        Serve.touchFieldCount = 0;
        

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
        for(int i = 0; i < 6; i++){
            SystemScript.leftPlayers[i].SetActive(true);
            SystemScript.rightPlayers[i].SetActive(true);
            SystemScript.leftPlayers[i].GetComponent<SpriteRenderer>().color = SystemScript.leftPlayers[i].GetComponent<dragPlayer>().orginColor;
            SystemScript.rightPlayers[i].GetComponent<SpriteRenderer>().color = SystemScript.rightPlayers[i].GetComponent<dragPlayer>().orginColor;
        }
    }

}



