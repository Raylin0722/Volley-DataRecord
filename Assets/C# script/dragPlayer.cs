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


//int saveIndex = 0;

public class dragPlayer : MonoBehaviour {
    [SerializeField] GameObject SelfScore;
    [SerializeField] GameObject EnemyScore;
    [SerializeField] GameObject database;
    [SerializeField] GameObject canvas;
    [SerializeField] public string playerName;
    [SerializeField] public string playerNum;
    static int changePosition = 0; //更換位子變數判斷
    private Vector3 initialPosition; // 球員初始位置
    private Vector3 AfterDragPosition; // 球員移動後位置
    public Vector3 PlayerSize; //球員大小
    Vector2 difference = Vector2.zero;

    static string block; // 儲存動作 block

    static string blockTag;
    static string oldblock; // 更新block顏色用

    GraphicRaycaster m_Raycaster; // 判斷block用
    PointerEventData m_PointerEventData; // 判斷block用
    EventSystem m_EventSystem; // 判斷block用

    static GameObject oldGameobject; // 更新block顏色用
    static Vector2 oldPoisition; // 判斷動作用
    static float duringTime; // 判斷動作用
    public RectTransform content;
    private Text LogText;

    public List<dealDB.Data> saveData;

    public string formation;
    
    public int leftTouch;
    public int rightTouch;

    void Start(){
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = canvas.GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = canvas.GetComponent<EventSystem>();
        oldblock = null;
        oldGameobject = null;
        oldPoisition = Vector2.zero;
        duringTime = 0f;
        saveData = database.GetComponent<dealDB>().saveData;
        formation = "";
        leftTouch = 0;
        rightTouch = 0;
    }
    private void OnMouseDown() {
        if(openSettingScene.interactable == 0)
            return;
        initialPosition = transform.position;
        PlayerSize = transform.localScale;
        //Debug.Log(PlayerSize);
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
    }
    private void OnMouseDrag() {
        if(openSettingScene.interactable == 0)
            return;
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
        
        m_PointerEventData = new PointerEventData(m_EventSystem);

        m_PointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        m_Raycaster.Raycast(m_PointerEventData, results);

        if(results.Count != 0 && changePosition == 0 && (results.Last().gameObject.tag == "block" || results.Last().gameObject.tag == "blocking" || results.Last().gameObject.tag == "outside" || results.Last().gameObject.tag == "serve")){
            
            Color temp = results.Last().gameObject.GetComponent<Image>().color;
            temp.a = 255f;
            results.Last().gameObject.GetComponent<Image>().color = temp;
            block = results.Last().gameObject.GetComponent<block>().blockID;
            blockTag = results.Last().gameObject.tag;
            if(oldblock == null){
                oldblock = block;
                oldGameobject = results.Last().gameObject;
            }
            else if(oldblock != null && oldblock != block){
                Color revert = oldGameobject.GetComponent<Image>().color;
                revert.a = 0f;
                oldGameobject.GetComponent<Image>().color = revert;
                oldGameobject = results.Last().gameObject;
                oldblock = block;
            }
        }

        if(changePosition == 0){
            if(Math.Abs(transform.position.x - oldPoisition[0]) < 0.3f && Math.Abs(transform.position.y - oldPoisition[1]) < 0.3f){
                duringTime += Time.deltaTime;
            }
            else{
                oldPoisition = (Vector2)transform.position;
                duringTime = 0f;
            }

            if(duringTime > 0.55f){
                transform.localScale = new Vector3(PlayerSize[0] * 2, PlayerSize[1] * 2, PlayerSize[2] * 2);
            }
            else{
                transform.localScale = PlayerSize;
            }
        }
    }
    private void OnMouseUp() {
        if(openSettingScene.interactable == 0)
            return;
        AfterDragPosition = transform.position;
        string formationLeft = "", formationRight = "";
        TextMeshPro[] WPlayer = database.GetComponent<dealDB>().WPlayer;
        TextMeshPro[] BPlayer = database.GetComponent<dealDB>().BPlayer;
        for(int i = 0; i < 6; i++){
            formationLeft += $"L{WPlayer[i].text} ";
            formationRight += $"R{BPlayer[i].text} ";
        }
        formation = formationLeft + formationRight;
        
        int mode = clickOrDrag();

        if(changePosition == 0){
            
            if(mode == dealDB.CATCH){
                setData(formation, block, null, dealDB.CATCH, 0);
            }
            else if(mode == dealDB.ATTACK){
               
                setData(formation, null, block, dealDB.ATTACK, 0);

            }
            else if(mode == dealDB.SERVE){
                
                setData(formation, null, block, dealDB.SERVE, 0);
                
            }
            else if(mode == dealDB.BLOCK){
                
                setData(formation, null, block, dealDB.BLOCK, 0);
            }   
            //Debug.Log(PlayerSize);
            transform.localScale = PlayerSize;
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
        if(saveData.Count == 0){ // serve
            //Debug.Log("serve");
            return dealDB.SERVE;
        }
        else if(duringTime < 0.5f && blockTag != "blocking"){ // catch
            //Debug.Log("catch");
            return dealDB.CATCH;
        }
        else if(duringTime < 0.5f && blockTag == "blocking"){ // block
            //Debug.Log("block");
            return dealDB.BLOCK;
        }
        else if(duringTime >= 0.5f){ // atack
            //Debug.Log("attack");
            return dealDB.ATTACK;
        }
        return 0;
    }

    private void setData(string formation, string catchBlock, string attackBlock, int situation, int score){
        
        int round = RefreshPoint.Self_Score + RefreshPoint.Enemy_Score + 1;
        dealDB.Data newData = new dealDB.Data(formation, round, playerName, attackBlock, catchBlock, situation, score);

        saveData.Add(newData);
    }
    
    public void dealData(){
        
        LogText = content.GetComponent<Text>();
        
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        if(obj.tag == "SelfPoint"){
            LogText.text += "Self Score\n";
            setData(formation, null, null, dealDB.SCORE, 1);
        }
        else if(obj.tag == "EnemyPoint"){
            LogText.text += "Enemy Score\n";
            setData(formation, null, null, dealDB.SCORE, -1);
            
        }
        //database.GetComponent<dealDB>().CallInsertData();
    }

    public void deletednewData(){
        
        if(saveData.Count > 0)
            saveData.RemoveAt(saveData.Count - 1);
        
        GenerateLogTable();
    }

    public void GenerateLogTable(){
        LogText = content.GetComponent<Text>();
        LogText.text = "";
        for(int i = 0; i < saveData.Count; i++){
            string roleColor = changeRoleColor(saveData[i].role);
            if(saveData[i].situation == 0){
                LogText.text += $"R{saveData[i].round}, <color={roleColor}>{saveData[i].role}</color>, Situ: CATCHING\n";
            }
            else if(saveData[i].situation == 1){
                LogText.text += $"R{saveData[i].round}, <color={roleColor}>{saveData[i].role}</color>, Situ: SERVING\n";
            }
            else if(saveData[i].situation == 2){
                LogText.text += $"R{saveData[i].round}, <color={roleColor}>{saveData[i].role}</color>, Situ: ATTACKING\n";
            }
            else{
                LogText.text += $"R{saveData[i].round}, <color={roleColor}>{saveData[i].role}</color>, Situ: BLOCKING\n";
            }
        }
    }

    public string changeRoleColor(string role_tmp) {
        string color_tmp;
        for(int i = 0;i < 12;i++) {
            if(role_tmp == SaveAndLoadName.SelfPlayerInfo[i,1]) {
                color_tmp = "red";
                return color_tmp;
            }
        }
        color_tmp = "blue";
        return color_tmp;
    }

    void OnDisable() {
        if (SelfScore != null) 
            SelfScore = null;

        if(EnemyScore != null)
           EnemyScore = null;
        
        if(database != null)
            database = null;
        
        if(canvas != null)
           canvas = null; 

        if(playerName != null)
            playerName = null;    
    }
}


