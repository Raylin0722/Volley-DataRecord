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
    [SerializeField] int typeofPlayer;
    public void OnMouseDown() {
        initialPosition = transform.position;
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
    }
    public void OnMouseDrag() {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
    }
    public void OnMouseUp() {
        checkPlayerToSwtich();
        transform.position = initialPosition;
    }
    [SerializeField] GameObject allofPlayer;
    [SerializeField] GameObject WPlayer1;
    [SerializeField] GameObject WPlayer2;
    [SerializeField] GameObject WPlayer3;
    [SerializeField] GameObject WPlayer4;
    [SerializeField] GameObject WPlayer5;
    [SerializeField] GameObject WPlayer6;
    [SerializeField] GameObject BPlayer1;
    [SerializeField] GameObject BPlayer2;
    [SerializeField] GameObject BPlayer3;
    [SerializeField] GameObject BPlayer4;
    [SerializeField] GameObject BPlayer5;
    [SerializeField] GameObject BPlayer6;
    public void checkPlayerToSwtich() {
        if(FirstPlayer == false && timeOfChange == 0)
            return;
        
        if(typeofPlayer == 0) {
            Bounds boundsPlayer = allofPlayer.GetComponent<Renderer>().bounds;
            Bounds boundsWPlayer1 = WPlayer1.GetComponent<Renderer>().bounds;
            Bounds boundsWPlayer2 = WPlayer2.GetComponent<Renderer>().bounds;
            Bounds boundsWPlayer3 = WPlayer3.GetComponent<Renderer>().bounds;
            Bounds boundsWPlayer4 = WPlayer4.GetComponent<Renderer>().bounds;
            Bounds boundsWPlayer5 = WPlayer5.GetComponent<Renderer>().bounds;
            Bounds boundsWPlayer6 = WPlayer6.GetComponent<Renderer>().bounds;
            if(boundsPlayer.Intersects(boundsWPlayer1) ){
                SwitchPlayerInfo(WPlayer1);
                print("WP1");
            }
            else if(boundsPlayer.Intersects(boundsWPlayer2)){
                SwitchPlayerInfo(WPlayer2);
                print("WP2");
            }
            else if(boundsPlayer.Intersects(boundsWPlayer3)){
                SwitchPlayerInfo(WPlayer3);
                print("WP3");
            }
            else if(boundsPlayer.Intersects(boundsWPlayer4)){
                SwitchPlayerInfo(WPlayer4);
                print("WP4");
            }
            else if(boundsPlayer.Intersects(boundsWPlayer5)){
                SwitchPlayerInfo(WPlayer5);
                print("WP5");
            }
            else if(boundsPlayer.Intersects(boundsWPlayer6)){
                SwitchPlayerInfo(WPlayer6);
                print("WP6");
            }
        }
        if(typeofPlayer == 1) {
            Bounds boundsPlayer = allofPlayer.GetComponent<Renderer>().bounds;
            Bounds boundsBPlayer1 = BPlayer1.GetComponent<Renderer>().bounds;
            Bounds boundsBPlayer2 = BPlayer2.GetComponent<Renderer>().bounds;
            Bounds boundsBPlayer3 = BPlayer3.GetComponent<Renderer>().bounds;
            Bounds boundsBPlayer4 = BPlayer4.GetComponent<Renderer>().bounds;
            Bounds boundsBPlayer5 = BPlayer5.GetComponent<Renderer>().bounds;
            Bounds boundsBPlayer6 = BPlayer6.GetComponent<Renderer>().bounds;
            if(boundsPlayer.Intersects(boundsBPlayer1)){
                SwitchPlayerInfo(BPlayer1);
                print("BP1");
            }
            else if(boundsPlayer.Intersects(boundsBPlayer2)){
                SwitchPlayerInfo(BPlayer2);
                print("BP2");
            }
            else if(boundsPlayer.Intersects(boundsBPlayer3)){
                SwitchPlayerInfo(BPlayer3);
                print("BP3");
            }
            else if(boundsPlayer.Intersects(boundsBPlayer4)){
                SwitchPlayerInfo(BPlayer4);
                print("BP4");
            }
            else if(boundsPlayer.Intersects(boundsBPlayer5)){
                SwitchPlayerInfo(BPlayer5);
                print("BP5");
            }
            else if(boundsPlayer.Intersects(boundsBPlayer6)){
                SwitchPlayerInfo(BPlayer6);
                print("BP6");
            }
        }
    }
    public void SwitchPlayerInfo(GameObject playertoSwitch) {
        playertoSwitch.gameObject.GetComponentInChildren<TextMeshPro>().text = allofPlayer.gameObject.GetComponentInChildren<TextMeshPro>().text;
        playertoSwitch.gameObject.GetComponent<dragPlayer>().playerNum = allofPlayer.gameObject.GetComponent<dragPlayerToChange>().playerNum;
        playertoSwitch.gameObject.GetComponent<dragPlayer>().playerName = allofPlayer.gameObject.GetComponent<dragPlayerToChange>().playerName;
    }

    
    
}
