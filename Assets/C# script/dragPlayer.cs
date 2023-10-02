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


    static int changePosition = 0; //更換位子變數判斷

    static bool alreadtAttack = false;
    private Vector3 initialPosition;
    private Vector3 AfterDragPosition;
    Vector2 difference = Vector2.zero;
    int xPosition, yPosition;

    static dealDB.Data[] saveData = new dealDB.Data[300];
    static int saveIndex = 0;

    private void OnMouseDown() {
        initialPosition = transform.position;
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
    }
    private void OnMouseDrag() {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
    }
    private void OnMouseUp() {
        AfterDragPosition = transform.position;
        int mode = clickOrDrag();
        if(changePosition == 0){
            int block = getBlock(AfterDragPosition);
            Debug.Log("mode: " + mode + " alreadAttack: " + alreadtAttack + " saveIndex: " + saveIndex);
            if(mode == 1 || mode == -1 && alreadtAttack){
                saveData[saveIndex].formation = null;
                saveData[saveIndex].catchblock = block;
                saveData[saveIndex].attackblock = -1;
                saveData[saveIndex].role = playerName;
                saveData[saveIndex].round = SelfScore.GetComponent<RefreshPoint>().Self_Score + EnemyScore.GetComponent<RefreshPoint>().Enemy_Score + 1;
                saveData[saveIndex].situation = dealDB.CATCH;

                saveIndex++;
                alreadtAttack = false;

                
                Debug.Log("formation: " + saveData[saveIndex].formation + " Role: " + saveData[saveIndex].role + " Round: " + saveData[saveIndex].round + " Attack: " + saveData[saveIndex].attackblock + " Catch: " + saveData[saveIndex].catchblock + " Situation: " + saveData[saveIndex].situation + " catch");
                
            }
            else if(mode == -1 && !alreadtAttack){
                saveData[saveIndex].formation = null;
                saveData[saveIndex].catchblock = -1;
                saveData[saveIndex].attackblock = block;
                saveData[saveIndex].role = playerName;
                saveData[saveIndex].round = SelfScore.GetComponent<RefreshPoint>().Self_Score + EnemyScore.GetComponent<RefreshPoint>().Enemy_Score + 1;
                saveData[saveIndex].situation = dealDB.ATTACK;

                Debug.Log("formation: " + saveData[saveIndex].formation + " Role: " + saveData[saveIndex].role + " Round: " + saveData[saveIndex].round + " Attack: " + saveData[saveIndex].attackblock + " Catch: " + saveData[saveIndex].catchblock + " Situation: " + saveData[saveIndex].situation + " attack");
                
                saveIndex++;
                alreadtAttack = true;
            }
            else if(mode == 2){
                saveData[saveIndex].formation = null;
                saveData[saveIndex].attackblock = block;
                saveData[saveIndex].role = playerName;
                saveData[saveIndex].round = SelfScore.GetComponent<RefreshPoint>().Self_Score + EnemyScore.GetComponent<RefreshPoint>().Enemy_Score + 1;
                saveData[saveIndex].situation = dealDB.SERVE;
                
                Debug.Log("formation: " + saveData[saveIndex].formation + " Role: " + saveData[saveIndex].role + " Round: " + saveData[saveIndex].round + " Attack: " + saveData[saveIndex].attackblock + " Catch: " + saveData[saveIndex].catchblock + " Situation: " + saveData[saveIndex].situation + " serve");
                saveIndex++;
                alreadtAttack = true;
            }
                
            transform.position = initialPosition;
        }
            
    }
    public void startChangePosition() {
        changePosition = 1 - changePosition;
    }
    private int clickOrDrag() {
        if(AfterDragPosition == initialPosition) { //click
            Debug.Log("點擊click");
            return 1;
        }
        else  if(AfterDragPosition != initialPosition && saveIndex != 0){ //drag and not serve
            Debug.Log("拖曳drag");
            return -1;
        }
        else if(AfterDragPosition != initialPosition && saveIndex == 0){ //serve
            return 2;
        }

        return 0;
    }

    private int getBlock(Vector3 position){
        return 0;
    }
}