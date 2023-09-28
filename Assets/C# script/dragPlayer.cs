using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class dragPlayer : MonoBehaviour {
    private Vector3 initialPosition;
    Vector2 difference = Vector2.zero;
    private void OnMouseDown() {
        initialPosition = transform.position;
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
    }
    private void OnMouseDrag() {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
    }
    private void OnMouseUp() {
        MoveToWhichPosition();
        transform.position = initialPosition;
    }
    private void MoveToWhichPosition() {
        int xPosition, yPosition;
        if(transform.position.x >= -2.2 && transform.position.x < -1.76)
            xPosition = 0;
        else if(transform.position.x >= -1.76 && transform.position.x < -1.32)
            xPosition = 1;
        else if(transform.position.x >= -1.32 && transform.position.x < -0.88)
            xPosition = 2;
        else if(transform.position.x >= -0.88 && transform.position.x < -0.44)
            xPosition = 3;
        else if(transform.position.x >= -0.44 && transform.position.x < 0)
            xPosition = 4;
        else if(transform.position.x > 0 && transform.position.x <= 0.44)
            xPosition = 5;
        else if(transform.position.x > 0.44 && transform.position.x <= 0.88)
            xPosition = 6;
        else if(transform.position.x > 0.88 && transform.position.x <= 1.32)
            xPosition = 7;
        else if(transform.position.x > 1.32 && transform.position.x <= 1.76)
            xPosition = 8;
        else if(transform.position.x > 1.76 && transform.position.x <= 2.2)
            xPosition = 9;
        else
            xPosition = -1;
        
        if(transform.position.y >= 0.9 && transform.position.y < 1.34)
            yPosition = 0;
        else if(transform.position.y >= 1.34 && transform.position.y < 1.78)
            yPosition = 1;
        else if(transform.position.y >= 1.78 && transform.position.y < 2.22)
            yPosition = 2;
        else if(transform.position.y >= 2.22 && transform.position.y < 2.66)
            yPosition = 3;
        else if(transform.position.y >= 2.66 && transform.position.y <= 3.1)
            yPosition = 4;
        else
            yPosition = -1;
        
        if(xPosition == -1 || yPosition == -1)
            Debug.Log("界外");
        else
            Debug.Log("拖曳座標: "+xPosition+"-"+yPosition);
    }
}