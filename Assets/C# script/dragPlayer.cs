using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    static dealDB.Data[] saveData = new dealDB.Data[300];
    static int saveIndex = 0;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    void Start(){
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = canvas.GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = canvas.GetComponent<EventSystem>();
    }
    private void OnMouseDown() {
        initialPosition = transform.position;
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
    }
    private void OnMouseDrag() {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
        //Debug.Log("test: " + transform.position.x + " " + transform.position.y)
    }
    private void OnMouseUp() {
        AfterDragPosition = transform.position;
        int mode = clickOrDrag();

        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        
        Debug.Log("Hit " + results[results.Count - 1].gameObject.name);

        if(changePosition == 0){
            int block = 0;
            //Debug.Log("mode: " + mode + " alreadAttack: " + alreadtAttack + " saveIndex: " + saveIndex);
            if(mode == 1 || mode == -1 && alreadtAttack){
                saveData[saveIndex].formation = null;
                saveData[saveIndex].catchblock = block;
                saveData[saveIndex].attackblock = -1;
                saveData[saveIndex].role = playerName;
                saveData[saveIndex].round = SelfScore.GetComponent<RefreshPoint>().Self_Score + EnemyScore.GetComponent<RefreshPoint>().Enemy_Score + 1;
                saveData[saveIndex].situation = dealDB.CATCH;

                saveIndex++;
                alreadtAttack = false;

                
                //Debug.Log("formation: " + saveData[saveIndex].formation + " Role: " + saveData[saveIndex].role + " Round: " + saveData[saveIndex].round + " Attack: " + saveData[saveIndex].attackblock + " Catch: " + saveData[saveIndex].catchblock + " Situation: " + saveData[saveIndex].situation + " catch");
                
            }
            else if(mode == -1 && !alreadtAttack){
                saveData[saveIndex].formation = null;
                saveData[saveIndex].catchblock = -1;
                saveData[saveIndex].attackblock = block;
                saveData[saveIndex].role = playerName;
                saveData[saveIndex].round = SelfScore.GetComponent<RefreshPoint>().Self_Score + EnemyScore.GetComponent<RefreshPoint>().Enemy_Score + 1;
                saveData[saveIndex].situation = dealDB.ATTACK;

                //Debug.Log("formation: " + saveData[saveIndex].formation + " Role: " + saveData[saveIndex].role + " Round: " + saveData[saveIndex].round + " Attack: " + saveData[saveIndex].attackblock + " Catch: " + saveData[saveIndex].catchblock + " Situation: " + saveData[saveIndex].situation + " attack");
                
                saveIndex++;
                alreadtAttack = true;
            }
            else if(mode == 2){
                saveData[saveIndex].formation = null;
                saveData[saveIndex].attackblock = block;
                saveData[saveIndex].role = playerName;
                saveData[saveIndex].round = SelfScore.GetComponent<RefreshPoint>().Self_Score + EnemyScore.GetComponent<RefreshPoint>().Enemy_Score + 1;
                saveData[saveIndex].situation = dealDB.SERVE;
                
                //Debug.Log("formation: " + saveData[saveIndex].formation + " Role: " + saveData[saveIndex].role + " Round: " + saveData[saveIndex].round + " Attack: " + saveData[saveIndex].attackblock + " Catch: " + saveData[saveIndex].catchblock + " Situation: " + saveData[saveIndex].situation + " serve");
                saveIndex++;
                alreadtAttack = true;
            }
            else if(mode == 3){
                saveData[saveIndex].formation = null;
                saveData[saveIndex].attackblock = -1;
                saveData[saveIndex].catchblock = block;
                saveData[saveIndex].role = playerName;
                saveData[saveIndex].round = SelfScore.GetComponent<RefreshPoint>().Self_Score + EnemyScore.GetComponent<RefreshPoint>().Enemy_Score + 1;
                saveData[saveIndex].situation = dealDB.BLOCK;
                
                //Debug.Log("formation: " + saveData[saveIndex].formation + " Role: " + saveData[saveIndex].role + " Round: " + saveData[saveIndex].round + " Attack: " + saveData[saveIndex].attackblock + " Catch: " + saveData[saveIndex].catchblock + " Situation: " + saveData[saveIndex].situation + " serve");
                saveIndex++;
            }   
                
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
}


