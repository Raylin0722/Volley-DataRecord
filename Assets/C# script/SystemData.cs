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
    public int leftScore; // 左方分數
    public int rightScore; // 右方分數
    public Text leftScoreText; // 左方分數文字
    public Text rightScoreText; // 右方分數文字
    public int whoWin;
    public int serServe;
    public List<dealDB.Data> saveData; // 資料儲存
    public int changePosition; //更換位子變數判斷
}
