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
    [SerializeField] public int playerPlayPos;
    [SerializeField] GameObject BeGameObject;
    [SerializeField] GameObject system;
    [SerializeField] Sprite[] playerShape;
    private Vector3 initialPosition; // 球員初始位置
    private Vector3 AfterDragPosition; // 球員移動後位置
    Vector2 difference = Vector2.zero;
    public RectTransform content;
    private Text LogText;
    public float pressTime;
    public DateTime LastClick, StartClick, EndClick;
    bool isPress, isClick, isDoubleClick;
    public bool[] isSelect;
    public bool[] updata;
    public bool[] flashing;
    ClickRecord DataScript;
    SystemData SystemScript;
    public Color orginColor;
    public Color flashColor;
    public float[] flashTime;
    
    
    void Awake(){
        if(this.gameObject.tag == "Right")
            orginColor = this.gameObject.GetComponent<SpriteRenderer>().color;
        else
            orginColor = new Color(255, 255, 255, 255);
    }
    void Start(){
        pressTime = 0f;
        LastClick = DateTime.Now.AddHours(-1);
        StartClick = DateTime.Now;
        EndClick = DateTime.Now;
        isPress = false;
        isClick = false;
        isDoubleClick = false;
        
        DataScript = BeGameObject.GetComponent<ClickRecord>();
        SystemScript = system.GetComponent<SystemData>();
        isSelect = new bool[1];
        isSelect[0] = false;
        updata = new bool[1];
        updata[0] = false;
        flashing = new bool[1];
        flashing[0] = false;
        flashColor = new Color(255, 255, 0, 255);
        flashTime = new float[1];
        flashTime[0] = 0f;
        
        print(this.gameObject);
        print(this.transform.position);
    }
    void Update(){
        if(!updata[0]){
            TextMeshPro textMeshPro = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();
            if (textMeshPro != null){
                if(SystemScript.nameMode == 0)
                    textMeshPro.text = playerName[playerName.Length - 1].ToString();
                else if(SystemScript.nameMode == 1)
                    textMeshPro.text = playerNum.ToString();

            }
            switch (playerPlayPos){
                case 0: case 3:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = playerShape[0];
                    break;
                case 1:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = playerShape[1];
                    break;
                case 2:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = playerShape[2];
                    break;
                case 4:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = playerShape[3];
                    break;

            }
            updata[0] = true;   
        }
        if(flashing[0]){
            playerFlash();
        }
        
    }
    void OnMouseDown(){
        if(SystemScript.changePlayer){
            dragPlayerToChange changeTmp;
            string tmpName = playerName, tmpNum = playerNum;
            int tmpPos = playerPlayPos;
            
            if(SystemScript.CformationTmp[0] != -1){
                int index = SystemScript.CformationTmp[1];
                if(SystemScript.CformationTmp[0] == 0 && this.gameObject.tag == "Left"){
                    changeTmp = SystemScript.changeLeftPlayers[index].GetComponent<dragPlayerToChange>();

                    playerName = changeTmp.playerName;
                    playerNum = changeTmp.playerNum;
                    playerPlayPos = changeTmp.playerPlayPos;
                    changeTmp.playerName = tmpName;
                    changeTmp.playerNum = tmpNum;
                    changeTmp.playerPlayPos = tmpPos;
                    changeTmp.refresh[0] = false;
                    
                }
                else if(SystemScript.CformationTmp[0] == 1 && this.gameObject.tag == "Right"){
                    changeTmp = SystemScript.changeRightPlayers[index].GetComponent<dragPlayerToChange>();
                    playerName = changeTmp.playerName;
                    playerNum = changeTmp.playerNum;
                    playerPlayPos = changeTmp.playerPlayPos;
                    changeTmp.playerName = tmpName;
                    changeTmp.playerNum = tmpNum;
                    changeTmp.playerPlayPos = tmpPos;
                    changeTmp.refresh[0] = false;
                }
                else
                    return;
            }
            SystemScript.CformationTmp[0] = -1;
            SystemScript.CformationTmp[1] = -1;
            updata[0] = false;
            return;
        }
        if(DataScript.Behavior.Last().complete == false && DataScript.Behavior.Last().behavior == -1)
            return;
        if(!SystemScript.changePosition){
            for(int i = 0; i < 6; i++){
                SystemScript.leftPlayers[i].GetComponent<dragPlayer>().flashing[0] = false;
                SystemScript.leftPlayers[i].GetComponent<dragPlayer>().flashTime[0] = 0f;
                SystemScript.leftPlayers[i].GetComponent<SpriteRenderer>().color = SystemScript.leftPlayers[i].GetComponent<dragPlayer>().orginColor;
                SystemScript.rightPlayers[i].GetComponent<dragPlayer>().flashing[0] = false;
                SystemScript.rightPlayers[i].GetComponent<dragPlayer>().flashTime[0] = 0f;
                SystemScript.rightPlayers[i].GetComponent<SpriteRenderer>().color = SystemScript.rightPlayers[i].GetComponent<dragPlayer>().orginColor;
            }
            CancelInvoke();
            StartClick = DateTime.Now;
            TimeSpan twoClickDuring = StartClick - LastClick;
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
            if(DataScript.Behavior.Count == 1){
                isClick = true;
                isDoubleClick = false;
                isPress = false;
            }
        }

    }
    void OnMouseDrag(){
        if(SystemScript.changePosition){
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f; // 如果物体是在平面上移动，可以将 z 坐标设为固定值，例如 0

            // 将物体的位置设置为鼠标当前位置
            transform.position = mousePosition;
        }
    }
    void OnMouseUp(){
        if(DataScript.Behavior.Last().complete == false && DataScript.Behavior.Last().behavior == -1)
            return;
        if(!SystemScript.changePosition){
            EndClick = DateTime.Now;
            TimeSpan pressTime = EndClick - StartClick;
            if(isSelect[0] && !isDoubleClick){
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
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 255, 1);
                Invoke("colorBack", 1f);
            }
            LastClick = DateTime.Now;
        }
    }

    void checkClickType(){
        if(isClick && !isDoubleClick && !isPress){
            print("Click! ");
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 1);
            isSelect[0] = true;
            hideOtherClick();
            Record(CLICK);
        }
        else if(!isClick && isDoubleClick && !isPress){
            print("Double Click ");
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0, 1);
            isSelect[0] = true;
            hideOtherBlock();
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
        for(int i = 0; i < DataScript.Behavior.Last().players.Count; i++)
            print(DataScript.Behavior.Last().players[i]);
    }
    void colorBack(){
        this.gameObject.GetComponent<SpriteRenderer>().color = orginColor;
    }
    void hideOtherClick(){
        for(int i = 0; i < 6; i++){
            SystemScript.leftPlayers[i].SetActive(false);
            SystemScript.rightPlayers[i].SetActive(false);
        }
        this.gameObject.SetActive(true);
    }
    void hideOtherBlock(){
        print(this.gameObject.tag);
        if(this.gameObject.tag == "Left"){
            for(int i = 0; i < 6; i++){
                SystemScript.rightPlayers[i].SetActive(false);
            }
            SystemScript.leftPlayers[0].SetActive(false);
            SystemScript.leftPlayers[4].SetActive(false);
            SystemScript.leftPlayers[5].SetActive(false);
            SystemScript.leftPlayers[1].GetComponent<dragPlayer>().flashing[0] = false;
            SystemScript.leftPlayers[2].GetComponent<dragPlayer>().flashing[0] = false;
            SystemScript.leftPlayers[3].GetComponent<dragPlayer>().flashing[0] = false;
        }
        else{
            for(int i = 0; i < 6; i++){
                SystemScript.leftPlayers[i].SetActive(false);
            }
            SystemScript.rightPlayers[0].SetActive(false);
            SystemScript.rightPlayers[4].SetActive(false);
            SystemScript.rightPlayers[5].SetActive(false);
            SystemScript.rightPlayers[1].GetComponent<dragPlayer>().flashing[0] = false;
            SystemScript.rightPlayers[2].GetComponent<dragPlayer>().flashing[0] = false;
            SystemScript.rightPlayers[3].GetComponent<dragPlayer>().flashing[0] = false;
        }
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
    }
    void playerFlash(){

        flashTime[0] += Time.deltaTime;
        if (flashTime[0] % 1 > 0.5f)
            this.gameObject.GetComponent<SpriteRenderer>().color = flashColor;

        else
            this.gameObject.GetComponent<SpriteRenderer>().color = orginColor;

        return;
    }
}


    