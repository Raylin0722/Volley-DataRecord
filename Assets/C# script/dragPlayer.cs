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
        Debug.Log("座標: "+transform.position);
        if(initialPosition == transform.position)
            Debug.Log("點擊=>傳球");
        else
            Debug.Log("拖曳=>攻擊");

        transform.position = initialPosition;
    }
}
