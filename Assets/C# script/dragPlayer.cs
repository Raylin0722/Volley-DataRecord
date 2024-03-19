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

    const int CLICK = 1, DOUBLECLICK = 2, PRESS = 3;
    const float LONGPRESS = 0.4f, DOUBLECLICKTIME= 0.2f;
    [SerializeField] public string playerName;
    [SerializeField] public string playerNum;
    [SerializeField] GameObject BeGameObject;
    [SerializeField] GameObject system;
    Color orginColor;
    private Vector3 initialPosition; // 球員初始位置
    private Vector3 AfterDragPosition; // 球員移動後位置
    Vector2 difference = Vector2.zero;
    public RectTransform content;
    private Text LogText;
    public float pressTime;
    public DateTime LastClick, StartClick, EndClick;
    bool isPress, isClick, isDoubleClick;
    public bool[] isSelect;
    
    ClickRecord DataScript;
    SystemData SystemScript;
    void Start(){
        pressTime = 0f;
        LastClick = DateTime.Now.AddHours(-1);
        StartClick = DateTime.Now;
        EndClick = DateTime.Now;
        isPress = false;
        isClick = false;
        isDoubleClick = false;
        orginColor = this.gameObject.GetComponent<SpriteRenderer>().color;
        DataScript = BeGameObject.GetComponent<ClickRecord>();
        SystemScript = system.GetComponent<SystemData>();
        isSelect = new bool[1];
        isSelect[0] = false;
    }
    void Update(){
        
        
    }
    void OnMouseDown(){
        CancelInvoke();
        StartClick = DateTime.Now;
        TimeSpan twoClickDuring = StartClick - LastClick;
        print(twoClickDuring.TotalSeconds);
        if(twoClickDuring.TotalSeconds <= DOUBLECLICKTIME){ // double click
            isDoubleClick = true;
            isClick = false;
            isPress = false;
        }
        else if(twoClickDuring.TotalSeconds > DOUBLECLICKTIME){
            isDoubleClick = false;
            isClick = true;
            isPress = false;
        }

    }
    void OnMouseDrag(){
        
    }
    void OnMouseUp(){
        EndClick = DateTime.Now;
        TimeSpan pressTime = EndClick - StartClick;
        print(isClick);
        if(isSelect[0]){
            for(int i = 0; i < 6; i++){
                SystemScript.leftPlayers[i].SetActive(true);
                SystemScript.rightPlayers[i].SetActive(true);
            }
            DataScript.Behavior.RemoveAt(DataScript.Behavior.Count - 1);
            isSelect[0] = false;
            return;
        }
        if(pressTime.TotalSeconds <= LONGPRESS){
            Invoke("checkClickType", DOUBLECLICKTIME);
        }
        else{
            //print("Press ");
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 255, 1);
            Invoke("colorBack", 1f);
        }
        LastClick = DateTime.Now;
    }

    void checkClickType(){
        if(isClick && !isDoubleClick && !isPress){
            print("Click! ");
            //this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 1);
            isSelect[0] = true;
            hideOther();
            Record(CLICK);
        }
        else if(!isClick && isDoubleClick && !isPress){
            print("Double Click ");
            //this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0, 1);
            isSelect[0] = true;
            hideOther();
            Record(DOUBLECLICK);
        }
        else if(!isClick && !isDoubleClick && isPress){
            print("Press ");
            Record(PRESS);
        }
    }   

    void Record(int type){
        if(!DataScript.Behavior.Any())
            return;
        if(DataScript.Behavior.Last().complete == true){
            // 記錄資料至 ClickData
            ClickRecord.ClickData newData = new ClickRecord.ClickData();
            newData.players = new List<GameObject>();
            newData.clicks = new List<Vector2>();
            newData.players.Add(this.gameObject);
            if(this.gameObject.tag == "Left")
                newData.side = 0;
            else if(this.gameObject.tag == "Right")
                newData.side = 1;
            
            if(type == CLICK){
                newData.clickType = CLICK; 
            }
            else if(type == DOUBLECLICK){
                newData.clickType = DOUBLECLICK;
            }
            else if(type == PRESS){
                newData.clickType = PRESS;
            }
        
            DataScript.Behavior.Add(newData);
        }
        else if(DataScript.Behavior.Last().complete == false && 
                DataScript.Behavior.Last().players.Any()){
            DataScript.Behavior.Last().players.Add(this.gameObject);
        }
    }
    void colorBack(){
        this.gameObject.GetComponent<SpriteRenderer>().color = orginColor;
    }

    void hideOther(){
        for(int i = 0; i < 6; i++){
            SystemScript.leftPlayers[i].SetActive(false);
            SystemScript.rightPlayers[i].SetActive(false);
        }
        this.gameObject.SetActive(true);
    }
    public void ShowRecord(){
        foreach (GameObject i in DataScript.Behavior.Last().players){
            print(i);
        }
        foreach (Vector2 i in DataScript.Behavior.Last().clicks){
            print(i);
        }
        print(DataScript.Behavior.Last().side);
        print(DataScript.Behavior.Last().complete);
        print(DataScript.Behavior.Last().behavior);
        print(DataScript.Behavior.Last().touchFieldCount);
    }

}


    