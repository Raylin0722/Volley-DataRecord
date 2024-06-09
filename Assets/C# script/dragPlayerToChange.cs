using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class dragPlayerToChange : MonoBehaviour {
    private Vector3 initialPosition;
    Vector2 difference = Vector2.zero;
    [SerializeField] public string playerName;
    [SerializeField] public string playerNum;
    [SerializeField] public int playerPlayPos;
    [SerializeField] public bool FirstPlayer;
    [SerializeField] public int timeOfChange;
    [SerializeField] public int changeTarget;
    public int[] side;
    public bool[] refresh;
    public GameObject system;
    SystemData SystemScript;

    void Start(){
        SystemScript = system.GetComponent<SystemData>();
        refresh = new bool[1]{false};
        side = new int[1]{0};
    }
    void Update(){
        if(!refresh[0]){
            TextMeshPro textMeshPro = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();
            if (SystemScript.nameMode == 0)
                textMeshPro.text = playerName[playerName.Length - 1].ToString();
            else if (SystemScript.nameMode == 1)
                textMeshPro.text = playerNum;
            refresh[0] = true;   
        }
    }
    public void OnMouseDown() {
        for(int i = 0; i < 6; i++){
            if(this.gameObject.tag == "Left"){ // LEFT
                print(String.Format("{0} {1} {2}", SystemScript.changeLeftPlayers[i], this.gameObject, SystemScript.changeLeftPlayers[i] == this.gameObject));
                if(SystemScript.changeLeftPlayers[i] == this.gameObject){
                    SystemScript.CformationTmp[0] = 0;
                    SystemScript.CformationTmp[1] = i;
                    print(SystemScript.CformationTmp[0]);
                    print(SystemScript.CformationTmp[1]);
                    break;
                }
            }
            else{ // RIGHT
                if(SystemScript.changeRightPlayers[i] == this.gameObject){
                    SystemScript.CformationTmp[0] = 1;
                    SystemScript.CformationTmp[1] = i;
                    print(SystemScript.CformationTmp[0]);
                    print(SystemScript.CformationTmp[1]);
                    break;
                }
            }
        }
    }
    
    public void OnMouseUp() {
        //checkPlayerToSwtich();
        //transform.position = initialPosition;
    }
    
    public void SwitchPlayerInfo(GameObject playertoSwitch) {
    
    }

    
    
}
