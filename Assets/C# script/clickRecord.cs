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
        public int behavior; // 動作 發球:-1 接球:0 舉球:1 攻擊:2 攔網:3 
        public int touchFieldCount;
        public int clickType;
        
    };
    public List<ClickData> Behavior;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject database;
    [SerializeField] GameObject systemData;
    [SerializeField] GameObject[] CanBlock;
    [SerializeField] GameObject[] NetLocate;
    static public int leftTouchCount;
    static public int rightTouchCount;
    Vector3[] NetLocateXY; 
    const int LEFT = 1, RIGHT = 2;
    const float doubleClick = 0.1f, LongClick = 0.5f;
    
    // 轉成世界座標只有Z座標不同
    
    
    // Start is called before the first frame update
    void Awake(){
        Behavior = new List<ClickData>();
    }
    void Start()
    {
        leftTouchCount = 0;
        rightTouchCount = 0;
        NetLocateXY = new Vector3[NetLocate.Length];

        // 前2網 後4左上右上左下右下
        for(int i = 0; i < NetLocate.Length; i++){
            NetLocateXY[i] = NetLocate[i].transform.position;
        }
        //Vector3 uiWorldPosition = GetWorldPositionFromUI(TestImage);

        // 將 Prefab 座標轉換為世界座標
        //Vector3 prefabWorldPosition = TestPlayer.transform.position;

        //Debug.Log("UI World Position: " + uiWorldPosition);
        //Debug.Log("Prefab World Position: " + prefabWorldPosition);
        
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
        if(Input.GetMouseButtonDown(0)){
            //print(Behavior.Any() == false);
            GetClickTarget();
        }
    }
    void GetClickTarget(){

        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        // 球員
        if (hit.collider != null)
        {
            // 獲取2D物體的標籤
            if(hit.collider.gameObject.tag == "Left" || hit.collider.gameObject.tag == "Right"){
                print("Player!");
                return;
            }
        }

        // 地板
        if (IsPointerOverUI())
        {
            // 获取UI元素的标签
            string tagUI = GetUITagUnderPointer();
            Debug.Log("Clicked on UI with tag: " + tagUI);
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
    bool PCheckComplete(){ // 用在點擊球員時
        if(Behavior.Count == 0)
            return true;
        
        if((Behavior.Last()).complete == false)
            return false;
        
        return true;
    }
    bool PCheckValid(GameObject playerClick){ // 點球員使用
        if(PCheckComplete() == false){ // 攔網:OK 其餘:NO
            ClickData nowBehavior = Behavior.Last(); 
            if(nowBehavior.behavior != 3) // 非攔網
                return false;
            if(Behavior.Count <= 2) // 發球不能攔網 or 第一個動作不能是攔網
                return false;
            if(Behavior[Behavior.Count - 2].players.Last().tag == playerClick.tag) // 檢查上個動作是否為敵方
                return false;
            if(nowBehavior.players.Last().tag != playerClick.tag) // 攔網選取的所有球員皆須同隊
                return false;
            if(System.Array.Exists(CanBlock, obj => obj == playerClick) == false) // 選取的球員在後排
                return false;
            if(nowBehavior.players.Count > 3) // 攔網但球員數錯誤
                return false;

            return true;
        }
        else{ // 判別左右方 連擊 擊球數
            ClickData prevBehavior = Behavior.Last(); 
            if(prevBehavior.players.Contains(playerClick) == true && 
               prevBehavior.behavior != 3) // 相同球員連擊 且非攔網
                return false;
            if((playerClick.tag == "Left" && leftTouchCount > 3) ||
               (playerClick.tag == "Right" && rightTouchCount > 3)) // 擊球數超過3
                return false;

            return true;
        }
    }
    bool FCheckValid(float x, float y){
        if(Behavior.Count == 0) // 沒有前置資料點擊地板無效
            return false;
        ClickData checkBehavior = Behavior.Last();
        if(checkBehavior.complete == true) // 前置資料已完成點擊地板無效
            return false;
        if(inField(x, y) == false) // 點擊區域不在場內
            return false;
        
        // 檢查動作是否合法
        int sideClicked = leftOrRight(x, y), behavior = checkBehavior.behavior;
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
        else if(behavior == 3){ // 攔網
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
    bool inField(float x, float y){
        if(x > NetLocateXY[2].x && x < NetLocateXY[3].x && y < NetLocateXY[2].y && y > NetLocateXY[4].y)
            return true;

        return false;
    }
    int leftOrRight(float x, float y){
        if(x >= NetLocateXY[0].x)
            return RIGHT;
        
        return LEFT;
    }

}
