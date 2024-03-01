using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SystemData : MonoBehaviour
{
    public string formation; // 紀錄當前陣容
    [SerializeField] public GameObject[] leftPlayers; // 左方球員
    [SerializeField] public GameObject[] rightPlayers; // 右方球員
    public Vector2[] leftPLayersPos;
    public Vector2[] rightPlayersPos;
    public int leftScore; // 左方分數
    public int rightScore; // 右方分數
    public Text leftScoreText; // 左方分數文字
    public Text rightScoreText; // 右方分數文字
    public int whoWin;
    public int serServe;
    public List<dealDB.Data> saveData; // 資料儲存
    public bool changePosition; //更換位子變數判斷

    void Awake(){
        leftScore = 0;
        rightScore = 0;
        leftScoreText.text = "00";
        rightScoreText.text = "00";
        saveData = new List<dealDB.Data>();
        changePosition = false;
        leftPLayersPos = new Vector2[6];
        rightPlayersPos = new Vector2[6];
        for(int i = 0; i < 6; i++){
            
        }
    }

}
