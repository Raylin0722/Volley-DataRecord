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
    [SerializeField] public GameObject[] initLeftPlayers;
    [SerializeField] public GameObject[] initRightPlayers;
    
    public Vector2[] leftPLayersPos;
    public Vector2[] rightPlayersPos;
    public int[] point; // 小分
    public int[] score; // 大分
    public Text leftScoreText; // 左方分數文字
    public Text rightScoreText; // 右方分數文字
    public Text leftPointText;
    public Text rightPointText;
    public Text leftTeamName;
    public Text rightTeamName;
    public int whoWin;
    public int serServe;
    public List<dealDB.Data> saveData; // 資料儲存
    public bool changePosition; //更換位子變數判斷

    void Awake(){
        score = new int[2];
        leftScoreText.text = "0";
        rightScoreText.text = "0";
        leftPointText.text = "00";
        rightPointText.text = "00";
        saveData = new List<dealDB.Data>();
        changePosition = false;
        leftPLayersPos = new Vector2[6];
        rightPlayersPos = new Vector2[6];
        for(int i = 0; i < 6; i++){
            
        }
    }

}
