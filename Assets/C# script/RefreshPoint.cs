using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class RefreshPoint : MonoBehaviour
{
    [SerializeField] public static int Self_Point;
    [SerializeField] Text Self_Point_Text;
    [SerializeField] public static int Enemy_Point;
    [SerializeField] Text Enemy_Point_Text;
    [SerializeField] public static int Self_Score;
    [SerializeField] Text Self_Score_Text;
    [SerializeField] public static int Enemy_Score;
    [SerializeField] Text Enemy_Score_Text;
    [SerializeField] Text Self_Team_Name;
    [SerializeField] Text Enemy_Team_Name;
    [SerializeField] GameObject[] Self_Player;
    [SerializeField] GameObject[] Enemy_Player;
    [SerializeField] GameObject Self_Serve;
    [SerializeField] GameObject Enemy_Serve;
    [SerializeField] public static int FirstTimeOpen = 0;
    [SerializeField] public static int setServe = 0; //每回合發球方

    // Start is called before the first frame update
    void Start()
    {
        UpdatePointScore();
        changeServe(0);
        changeSideServe();
    }
    public void UpdatePointScore() {
        if(Self_Point < 10)
            Self_Point_Text.text = "0" + Self_Point.ToString();
        else
            Self_Point_Text.text = Self_Point.ToString();
        Self_Score_Text.text = Self_Score.ToString();

        if(Enemy_Point < 10)
            Enemy_Point_Text.text = "0" + Enemy_Point.ToString();
        else
            Enemy_Point_Text.text = Enemy_Point.ToString();
        Enemy_Score_Text.text = Enemy_Score.ToString();
    }
    public void add_point(){//小比分
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        if(obj.tag == "SelfPoint"){
            
            Self_Point++;
            if(Self_Point < 10){
                Self_Point_Text.text = "0" + Self_Point.ToString();
            }
            else{
                Self_Point_Text.text = Self_Point.ToString();
            }
            changeServe(-1); //-1=left win
            add_score();
        }
        else{
            Enemy_Point++;
            if(Enemy_Point < 10){
                Enemy_Point_Text.text = "0" + Enemy_Point.ToString();
            }
            else{
                Enemy_Point_Text.text = Enemy_Point.ToString();
            }
            changeServe(1); //1=right win
            add_score();   
        }
    }

    public void add_score(){//大比分
        //Debug.Log(Self_Point + " " + Enemy_Point);
        int winPoints = 25;
        if(Self_Score + Enemy_Score >= 4)
            winPoints = 15;
        if(Self_Point >= winPoints && ((Self_Point - Enemy_Point) >= 2)){
            Self_Score++;
            Self_Score_Text.text = Self_Score.ToString();
            Self_Point_Text.text = "00";
            Enemy_Point_Text.text = "00";
            Self_Point = 0;
            Enemy_Point = 0;
            changeAll();
            changeSideServe();
        }
        else if(Enemy_Point >= winPoints && ((Enemy_Point - Self_Point) >= 2)){
            Enemy_Score++;
            Enemy_Score_Text.text = Enemy_Score.ToString();
            Enemy_Point_Text.text = "00";
            Self_Point_Text.text = "00";
            Enemy_Point = 0;
            Self_Point = 0;
            changeAll();
            changeSideServe();
        }
    }
    //setServe 0,1
    //whoWin -1,0,1
    public void changeServe(int whoWin) {
        if(whoWin == 0) {
            if(setServe == 1)
                Self_Serve.SetActive(false);
            else if(setServe == 0)
                Enemy_Serve.SetActive(false);
        }
        if(whoWin == -1 && setServe == 1) { //self win and enemy serve
            Self_Serve.SetActive(true);
            Enemy_Serve.SetActive(false);
            setServe = 0;
            enemyRotation();
        }
        if(whoWin == 1 && setServe == 0) { //enemy win and self serve
            Self_Serve.SetActive(false);
            Enemy_Serve.SetActive(true);
            setServe = 1;
            selfRotation();
        }
    }

    public void changeSideServe() {
        if(Self_Point != 0 && Enemy_Point != 0)
            return;
        if(Setting.whoServe == 0){
            Self_Serve.SetActive(true);
            Enemy_Serve.SetActive(false);
            setServe = 0;
        }
        else if(Setting.whoServe == 1){
            Self_Serve.SetActive(false);
            Enemy_Serve.SetActive(true);
            setServe = 1;
        }
    }
    public void selfRotation() {
        string text_tmp = Self_Player[0].gameObject.GetComponentInChildren<TextMeshPro>().text;
        for(int i = 0;i < 5;i++) {
            Self_Player[i].gameObject.GetComponentInChildren<TextMeshPro>().text = Self_Player[i+1].gameObject.GetComponentInChildren<TextMeshPro>().text;
        }
        Self_Player[5].gameObject.GetComponentInChildren<TextMeshPro>().text = text_tmp;

        string Name_tmp = Self_Player[0].gameObject.GetComponent<dragPlayer>().playerName;
        for(int i = 0;i < 5;i++) {
            Self_Player[i].gameObject.GetComponent<dragPlayer>().playerName = Self_Player[i+1].gameObject.GetComponent<dragPlayer>().playerName;
        }
        Self_Player[5].gameObject.GetComponent<dragPlayer>().playerName = Name_tmp;
    }
    public void enemyRotation() {
        string text_tmp = Enemy_Player[0].gameObject.GetComponentInChildren<TextMeshPro>().text;
        for(int i = 0;i < 5;i++) {
            Enemy_Player[i].gameObject.GetComponentInChildren<TextMeshPro>().text = Enemy_Player[i+1].gameObject.GetComponentInChildren<TextMeshPro>().text;
        }
        Enemy_Player[5].gameObject.GetComponentInChildren<TextMeshPro>().text = text_tmp;

        string Name_tmp = Enemy_Player[0].gameObject.GetComponent<dragPlayer>().playerName;
        for(int i = 0;i < 5;i++) {
            Enemy_Player[i].gameObject.GetComponent<dragPlayer>().playerName = Enemy_Player[i+1].gameObject.GetComponent<dragPlayer>().playerName;
        }
        Enemy_Player[5].gameObject.GetComponent<dragPlayer>().playerName = Name_tmp;
    }

    public void changeAll() {
        string teamName = SaveAndLoadName.TeamName[0];
        SaveAndLoadName.TeamName[0] = SaveAndLoadName.TeamName[1];
        SaveAndLoadName.TeamName[1] = teamName;

        for(int i = 0;i < 12;i++) {
            string num_tmp = SaveAndLoadName.SelfPlayerInfo[i,0];
            SaveAndLoadName.SelfPlayerInfo[i,0] = SaveAndLoadName.EnemyPlayerInfo[i,0];
            SaveAndLoadName.EnemyPlayerInfo[i,0] = num_tmp;

            string name_tmp = SaveAndLoadName.SelfPlayerInfo[i,1];
            SaveAndLoadName.SelfPlayerInfo[i,1] = SaveAndLoadName.EnemyPlayerInfo[i,1];
            SaveAndLoadName.EnemyPlayerInfo[i,1] = name_tmp;
        }

        for(int i = 0;i < 6;i++) {
            string playerText_tmp = Self_Player[i].gameObject.GetComponentInChildren<TextMeshPro>().text;
            Self_Player[i].gameObject.GetComponentInChildren<TextMeshPro>().text = Enemy_Player[i].gameObject.GetComponentInChildren<TextMeshPro>().text;
            Enemy_Player[i].gameObject.GetComponentInChildren<TextMeshPro>().text = playerText_tmp;

            string playerName_tmp = Self_Player[i].gameObject.GetComponent<dragPlayer>().playerName;
            Self_Player[i].gameObject.GetComponent<dragPlayer>().playerName = Enemy_Player[i].gameObject.GetComponent<dragPlayer>().playerName;
            Enemy_Player[i].gameObject.GetComponent<dragPlayer>().playerName = playerName_tmp;
        }

        string teamName_tmp = Self_Team_Name.text;
        Self_Team_Name.text = Enemy_Team_Name.text;
        Enemy_Team_Name.text = teamName_tmp;

        int teamScore = Self_Score;
        Self_Score = Enemy_Score;
        Enemy_Score = teamScore;

        string teamScore_tmp = Self_Score_Text.text;
        Self_Score_Text.text = Enemy_Score_Text.text;
        Enemy_Score_Text.text = teamScore_tmp;
    }
}
