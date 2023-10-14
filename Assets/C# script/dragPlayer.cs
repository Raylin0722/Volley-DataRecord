using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using database;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.Linq;

//int saveIndex = 0;

public class dragPlayer : MonoBehaviour {
    [SerializeField] public string playerName;
    [SerializeField] GameObject SelfScore;
    [SerializeField] GameObject EnemyScore;

    [SerializeField] GameObject canvas;

    static int changePosition = 0; //更換位子變數判斷
    private Vector3 initialPosition; // 球員初始位置
    private Vector3 AfterDragPosition; // 球員移動後位置
    Vector2 difference = Vector2.zero;

    public static dealDB.Data[] saveData; // 儲存資料用
    public static int saveIndex; // 儲存資料用

    static int block; // 儲存動作 block

    GraphicRaycaster m_Raycaster; // 判斷block用
    PointerEventData m_PointerEventData; // 判斷block用
    EventSystem m_EventSystem; // 判斷block用

    static int oldblock; // 更新block顏色用

    static GameObject oldGameobject; // 更新block顏色用
    static Vector2 oldPoisition; // 判斷動作用
    static float duringTime; // 判斷動作用
    public RectTransform content;
    private Text LogText;
    private string SApath = Application.streamingAssetsPath;
    private string dbName = "database.db";
    public string databasePath;

    public string gameName;


    

    void Start(){
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = canvas.GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = canvas.GetComponent<EventSystem>();
        saveData = new dealDB.Data[300];
        saveIndex = 0;
        oldblock = -1;
        oldGameobject = null;
        oldPoisition = Vector2.zero;
        duringTime = 0f;
        databasePath = System.IO.Path.Combine(SApath, dbName);
    
        databasePath = "URI=file:" + databasePath;
        DateTime now = DateTime.Now;
        gameName = now.ToString("yyyy_MM_dd");
    }
    private void OnMouseDown() {
        initialPosition = transform.position;
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
    }
    private void OnMouseDrag() {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
        
        m_PointerEventData = new PointerEventData(m_EventSystem);

        m_PointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        m_Raycaster.Raycast(m_PointerEventData, results);

        if(results.Count != 0 && changePosition == 0){
            
            Color temp = results.Last().gameObject.GetComponent<Image>().color;
            temp.a = 255f;
            results.Last().gameObject.GetComponent<Image>().color = temp;
            block = results.Last().gameObject.GetComponent<block>().blockID;
            if(oldblock == -1){
                oldblock = block;
                oldGameobject = results.Last().gameObject;
            }
            else if(oldblock != -1 && oldblock != block){
                Color revert = oldGameobject.GetComponent<Image>().color;
                revert.a = 0f;
                oldGameobject.GetComponent<Image>().color = revert;

                oldGameobject = results.Last().gameObject;
                oldblock = block;
            }
        }

        if(Math.Abs(transform.position.x - oldPoisition[0]) < 0.1f && Math.Abs(transform.position.y - oldPoisition[1]) < 0.01f){
            duringTime += Time.deltaTime;
        }
        else{
            oldPoisition = (Vector2)transform.position;
            duringTime = 0f;
        }

        if(duringTime > 0.7f){
            //這邊要把角色發光 找時間回來做
        }
        
    }
    private void OnMouseUp() {

        AfterDragPosition = transform.position;
        int mode = clickOrDrag();

        if(changePosition == 0){
            
            if(mode == dealDB.CATCH){
                setData(null, block, -1, dealDB.CATCH);
        
            }
            else if(mode == dealDB.ATTACK){
               
                setData(null, -1, block, dealDB.ATTACK);

            }
            else if(mode == dealDB.SERVE){
                
                setData(null, -1, block, dealDB.SERVE);
                
            }
            else if(mode == dealDB.BLOCK){
                
                setData(null, -1, block, dealDB.BLOCK);
            }   

            transform.position = initialPosition;

            Color revert = oldGameobject.GetComponent<Image>().color;
            revert.a = 0f;
            oldGameobject.GetComponent<Image>().color = revert;
            GenerateLogTable();
        }
        
    }
    public void startChangePosition() {
        changePosition = 1 - changePosition;
    }
    private int clickOrDrag() {
        if(saveIndex == 0){ // serve
            Debug.Log("serve");
            return dealDB.SERVE;
        }
        else if(duringTime < 0.7f && block != 8 && saveIndex != 0){ // catch
            Debug.Log("catch");
            return dealDB.CATCH;
        }
        else if(duringTime < 0.7f && block == 8){ // block
            Debug.Log("block");
            return dealDB.BLOCK;
        }
        else if(duringTime >= 0.7f){ // atack
            Debug.Log("attack");
            return dealDB.ATTACK;
        }
        return 0;
    }

    private void setData(string formation, int catchBlock, int attackBlock, int situation){
        saveData[saveIndex].formation = formation;
        saveData[saveIndex].catchblock = catchBlock;
        saveData[saveIndex].attackblock = attackBlock;
        saveData[saveIndex].role = playerName;
        saveData[saveIndex].round = SelfScore.GetComponent<RefreshPoint>().Self_Score + EnemyScore.GetComponent<RefreshPoint>().Enemy_Score + 1;
        saveData[saveIndex].situation = situation;
        saveIndex++;
    }

    
    
    public void insertData(dealDB.Data data){
        using(var connection = new SqliteConnection(databasePath)){
            connection.Open();

            using (var command = connection.CreateCommand()){
                command.CommandText = "INSERT INTO '" + gameName + "_contestData' (formation, round, role, attackblock, catchblock, situation, score) VALUES (\"" + data.formation + "\", " + 
                data.round + ", \"" + data.role + "\", " + data.attackblock + ", " +
                data.catchblock + "," + data.situation + ", " + data.score + ");";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    
    public void dealData(){
        
        LogText = content.GetComponent<Text>();
        
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        if(obj.tag == "SelfPoint"){
            LogText.text += "Self Score\n";
            if(saveIndex != 0){
                saveData[saveIndex - 1].score = 1;
            }
        }
        else if(obj.tag == "EnemyPoint"){
            LogText.text += "Enemy Score\n";
            if(saveIndex != 0){
                saveData[saveIndex - 1].score = -1;
            }
        }
        for(int i = 0; i < saveIndex; i++){
            insertData(saveData[i]);
            
        }
        saveIndex = 0;
    }

    public void deletednewData(){
        if(saveIndex > 0)
            saveIndex--;
        GenerateLogTable();
    }

    public void GenerateLogTable(){
        LogText = content.GetComponent<Text>();
        LogText.text = "";
        for(int i = 0;i < saveIndex;i++){
            if(saveData[i].situation == 0){
                LogText.text += $"Round:{saveData[i].round}, {saveData[i].role}:, Situation: CATCHING\n";
            }
            else if(saveData[i].situation == 1){
                LogText.text += $"Round:{saveData[i].round}, {saveData[i].role}:, Situation: SERVING\n";
            }
            else if(saveData[i].situation == 2){
                LogText.text += $"Round:{saveData[i].round}, {saveData[i].role}:, Situation: ATTACKING\n";
            }
            else{
                LogText.text += $"Round:{saveData[i].round}, {saveData[i].role}:, Situation: BLOCKING\n";
            }
        }
        
    }
}


