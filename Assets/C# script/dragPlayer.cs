using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class dragPlayer : MonoBehaviour {
    int changePosition = 0; //更換位子變數判斷
    private Vector3 initialPosition;
    private Vector3 AfterDragPosition;
    Vector2 difference = Vector2.zero;
    int xPosition, yPosition;
    private void OnMouseDown() {
        initialPosition = transform.position;
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
    }
    private void OnMouseDrag() {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
    }
    private void OnMouseUp() {
        AfterDragPosition = transform.position;
        clickOrDrag();
        if(changePosition == 0)
            transform.position = initialPosition;
    }
    public void startChangePosition() {
        changePosition = 1 - changePosition;
    }
    private void clickOrDrag() {
        if(AfterDragPosition == initialPosition) { //click
            Debug.Log("點擊click");
        }
        else { //drag
            Debug.Log("拖曳drag");
        }
    }
}