using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using database;
using System.Data;
using Mono.Data.Sqlite;

//int saveIndex = 0;

public class dragPlayer : MonoBehaviour {
    [SerializeField] string playerName;
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

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    void Start(){
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = canvas.GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = canvas.GetComponent<EventSystem>();
        saveData = new dealDB.Data[300];
        saveIndex = 0;
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

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        
        //Debug.Log("Hit " + results[results.Count - 1].gameObject.name);
    }
    private void OnMouseUp() {
        AfterDragPosition = transform.position;
        int mode = clickOrDrag();

        if(changePosition == 0){
            int block = 0;
            //Debug.Log("mode: " + mode + " alreadAttack: " + alreadtAttack + " saveIndex: " + saveIndex);
            if(mode == 1 || mode == -1 && alreadtAttack){
                setData(null, block, -1, playerName, dealDB.CATCH);
                
                alreadtAttack = false;

                
                Debug.Log("formation: " + saveData[saveIndex - 1].formation + " Role: " + saveData[saveIndex - 1].role + " Round: " + saveData[saveIndex - 1].round + " Attack: " + saveData[saveIndex].attackblock + " Catch: " + saveData[saveIndex].catchblock + " Situation: " + saveData[saveIndex].situation + " catch");
                
            }
            else if(mode == -1 && !alreadtAttack){
                setData(null, -1, block, playerName, dealDB.ATTACK);

                Debug.Log("formation: " + saveData[saveIndex].formation + " Role: " + saveData[saveIndex].role + " Round: " + saveData[saveIndex].round + " Attack: " + saveData[saveIndex].attackblock + " Catch: " + saveData[saveIndex].catchblock + " Situation: " + saveData[saveIndex].situation + " attack");
                alreadtAttack = true;
            }
            else if(mode == 2){
                setData(null, -1, block, playerName, dealDB.SERVE);
                
                Debug.Log("formation: " + saveData[saveIndex].formation + " Role: " + saveData[saveIndex].role + " Round: " + saveData[saveIndex].round + " Attack: " + saveData[saveIndex].attackblock + " Catch: " + saveData[saveIndex].catchblock + " Situation: " + saveData[saveIndex].situation + " serve");

                alreadtAttack = true;
            }
            else if(mode == 3){
                setData(null, -1, block, playerName, dealDB.BLOCK);

                Debug.Log("formation: " + saveData[saveIndex].formation + " Role: " + saveData[saveIndex].role + " Round: " + saveData[saveIndex].round + " Attack: " + saveData[saveIndex].attackblock + " Catch: " + saveData[saveIndex].catchblock + " Situation: " + saveData[saveIndex].situation + " serve");
            }   
            //Debug.Log(saveIndex);  
            transform.position = initialPosition;
        }
            
    }
    public void startChangePosition() {
        changePosition = 1 - changePosition;
        Debug.Log(changePosition);
    }
    private int clickOrDrag() {
        if(AfterDragPosition == initialPosition) { //click
            //Debug.Log("點擊click");
            return 1;
        }
        else  if(AfterDragPosition != initialPosition && saveIndex != 0){ //drag and not serve
            //Debug.Log("拖曳drag");
            return -1;
        }
        else if(AfterDragPosition != initialPosition && saveIndex == 0){ //serve
            return 2;
        }
        else if(AfterDragPosition != initialPosition && ( initialPosition[0] < 0 && AfterDragPosition[0] > 0 || initialPosition[0] > 0 && initialPosition[0] < 0 )){
            return 3;
        }

        return 0;
    }

    private void setData(string formation, int catchBlock, int attackBlock, string role, int situation){
        saveData[saveIndex].formation = formation;
        saveData[saveIndex].catchblock = catchBlock;
        saveData[saveIndex].attackblock = attackBlock;
        saveData[saveIndex].role = role;
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

}


