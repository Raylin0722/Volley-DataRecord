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

    static bool alreadtAttack = false;
    private Vector3 initialPosition;
    private Vector3 AfterDragPosition;
    Vector2 difference = Vector2.zero;

    public static dealDB.Data[] saveData;
    public static int saveIndex;

    static int block;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    static int oldblock;

    static GameObject oldGameobject;

    static Vector2 oldPoisition;
    static float duringTime;

    void Start(){
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = canvas.GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = canvas.GetComponent<EventSystem>();
        saveData = new dealDB.Data[300];
        saveIndex = 0;
        oldblock = -1;
        oldGameobject = null;
        oldPoisition = new Vector2(0, 0);
        duringTime = 0f;
    }
    private void OnMouseDown() {
        initialPosition = transform.position;
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
    }
    private void OnMouseDrag() {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
        //Debug.Log("test: " + transform.position.x + " " + transform.position.y)
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
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
        //Debug.Log(duringTime);
        AfterDragPosition = transform.position;
        int mode = clickOrDrag();
        //Debug.Log(changePosition);
        if(changePosition == 0){
            
            if(mode == dealDB.CATCH){
                setData(null, block, -1, dealDB.CATCH);
                
                alreadtAttack = false;
                //Debug.Log("formation: " + saveData[saveIndex - 1].formation + " Role: " + saveData[saveIndex - 1].role + " Round: " + saveData[saveIndex - 1].round + " Attack: " + saveData[saveIndex - 1].attackblock + " Catch: " + saveData[saveIndex - 1].catchblock + " Situation: " + saveData[saveIndex - 1].situation + " catch");
                
            }
            else if(mode == dealDB.ATTACK){
                setData(null, -1, block, dealDB.ATTACK);

                //Debug.Log("formation: " + saveData[saveIndex - 1].formation + " Role: " + saveData[saveIndex - 1].role + " Round: " + saveData[saveIndex - 1].round + " Attack: " + saveData[saveIndex - 1].attackblock + " Catch: " + saveData[saveIndex - 1].catchblock + " Situation: " + saveData[saveIndex - 1].situation + " attack");
                alreadtAttack = true;
            }
            else if(mode == dealDB.SERVE){
                setData(null, -1, block, dealDB.SERVE);
                
                //Debug.Log("formation: " + saveData[saveIndex - 1].formation + " Role: " + saveData[saveIndex - 1].role + " Round: " + saveData[saveIndex - 1].round + " Attack: " + saveData[saveIndex - 1].attackblock + " Catch: " + saveData[saveIndex - 1].catchblock + " Situation: " + saveData[saveIndex - 1].situation + " serve");

                alreadtAttack = true;
            }
            else if(mode == dealDB.BLOCK){
                setData(null, -1, block, dealDB.BLOCK);

                //Debug.Log("formation: " + saveData[saveIndex - 1].formation + " Role: " + saveData[saveIndex - 1].role + " Round: " + saveData[saveIndex - 1].round + " Attack: " + saveData[saveIndex - 1].attackblock + " Catch: " + saveData[saveIndex - 1].catchblock + " Situation: " + saveData[saveIndex - 1].situation + " serve");
            }   

            transform.position = initialPosition;

            Color revert = oldGameobject.GetComponent<Image>().color;
            revert.a = 0f;
            oldGameobject.GetComponent<Image>().color = revert;

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

    public string dbName = "URI=file:"+ Application.dataPath + "database.db";
    public void insertData(dealDB.Data data){
        using(var connection = new SqliteConnection(dbName)){
            connection.Open();

            using (var command = connection.CreateCommand()){
                command.CommandText = "INSERT INTO contestData (formation, round, role, attackblock, catchblock, situation, score) VALUES (\"" + data.formation + "\", " + 
                data.round + ", \"" + data.role + "\", " + data.attackblock + ", " +
                data.catchblock + "," + data.situation + ", " + data.score + ");";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public void dealData(){
        for(int i = 0; i < saveIndex; i++){
            insertData(saveData[i]);
        }
        saveIndex = 0;
    }

    public void deletednewData(){
        
        if(saveIndex > 0)
            saveIndex--;
    }

}


