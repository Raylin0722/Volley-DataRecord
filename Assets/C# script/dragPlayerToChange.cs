using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
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
    }
    void Update(){
        if(!refresh[0]){
            TextMeshPro textMeshPro = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();
            if (textMeshPro != null)
                textMeshPro.text = playerName;
            refresh[0] = true;   
        }
    }
    public void OnMouseDown() {
        for(int i = 0; i < 6; i++){
            if(side[0] == 0){ // LEFT
                if(SystemScript.changeLeftPlayers[i] == this.gameObject){
                    SystemScript.CformationTmp[0] = side[0];
                    SystemScript.CformationTmp[1] = i;
                    break;
                }
            }
            else{ // RIGHT
                if(SystemScript.changeRightPlayers[i] == this.gameObject){
                    SystemScript.CformationTmp[0] = side[0];
                    SystemScript.CformationTmp[1] = i;
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
