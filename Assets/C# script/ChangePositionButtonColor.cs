using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePositionButtonColor : MonoBehaviour{
    public GameObject button;
    static int changeColor = 0;
    public void changeButtonColor() {
        changeColor = 1 - changeColor;
        if(changeColor == 1)
            button.GetComponent<Image>().color = Color.gray;
        else
            button.GetComponent<Image>().color = Color.white;
    }
}
