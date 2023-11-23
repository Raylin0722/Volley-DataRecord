using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class InsertInfo : MonoBehaviour
{
    [SerializeField] Text SelfTeamName, EnemyTeamName;
    [SerializeField] GameObject[] WP, BP, SP, EP; // self/enemy player on field, all of self/enemy player
    [SerializeField] GameObject WarningMessage;
    void Update() {
        int showWarningMessage = 0;
        for(int i = 0;i < 6;i++) {
            if(string.IsNullOrWhiteSpace(SaveAndLoadName.TeamName[0]) || string.IsNullOrWhiteSpace(SaveAndLoadName.TeamName[1]))
                showWarningMessage = 1;
            if(string.IsNullOrWhiteSpace(SaveAndLoadName.SelfPlayerInfo[i,0]) || string.IsNullOrWhiteSpace(SaveAndLoadName.SelfPlayerInfo[i,1]))
                showWarningMessage = 1;
            if(string.IsNullOrWhiteSpace(SaveAndLoadName.EnemyPlayerInfo[i,0]) || string.IsNullOrWhiteSpace(SaveAndLoadName.EnemyPlayerInfo[i,1]))
                showWarningMessage = 1;
        }

        if(showWarningMessage == 1)
            WarningMessage.SetActive(true);
        else
            WarningMessage.SetActive(false);
    }
    public void DefaultNameforTesting() {
        SaveAndLoadName.TeamName[0] = "預設A";
        SaveAndLoadName.TeamName[1] = "預設B";
        SaveAndLoadName.SelfPlayerInfo[0,0] = "00";
        SaveAndLoadName.SelfPlayerInfo[0,1] = "Apple";
        SaveAndLoadName.SelfPlayerInfo[1,0] = "23";
        SaveAndLoadName.SelfPlayerInfo[1,1] = "Banana";
        SaveAndLoadName.SelfPlayerInfo[2,0] = "99";
        SaveAndLoadName.SelfPlayerInfo[2,1] = "Cake";
        SaveAndLoadName.SelfPlayerInfo[3,0] = "70";
        SaveAndLoadName.SelfPlayerInfo[3,1] = "Duck";
        SaveAndLoadName.SelfPlayerInfo[4,0] = "45";
        SaveAndLoadName.SelfPlayerInfo[4,1] = "Eagle";
        SaveAndLoadName.SelfPlayerInfo[5,0] = "69";
        SaveAndLoadName.SelfPlayerInfo[5,1] = "Frog";

        SaveAndLoadName.EnemyPlayerInfo[0,0] = "32";
        SaveAndLoadName.EnemyPlayerInfo[0,1] = "Gigi";
        SaveAndLoadName.EnemyPlayerInfo[1,0] = "17";
        SaveAndLoadName.EnemyPlayerInfo[1,1] = "Hello";
        SaveAndLoadName.EnemyPlayerInfo[2,0] = "05";
        SaveAndLoadName.EnemyPlayerInfo[2,1] = "Iphone";
        SaveAndLoadName.EnemyPlayerInfo[3,0] = "77";
        SaveAndLoadName.EnemyPlayerInfo[3,1] = "Joker";
        SaveAndLoadName.EnemyPlayerInfo[4,0] = "59";
        SaveAndLoadName.EnemyPlayerInfo[4,1] = "Kevin";
        SaveAndLoadName.EnemyPlayerInfo[5,0] = "88";
        SaveAndLoadName.EnemyPlayerInfo[5,1] = "Lemon";
    }
    void Start() {
        DefaultNameforTesting();
        SelfTeamName.text = SaveAndLoadName.TeamName[0];
        EnemyTeamName.text = SaveAndLoadName.TeamName[1];
        for(int i = 0;i < 6;i++) {
            WP[i].gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.SelfPlayerInfo[i,0];
            WP[i].gameObject.GetComponent<dragPlayer>().playerName = SaveAndLoadName.SelfPlayerInfo[i,1];
            BP[i].gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.EnemyPlayerInfo[i,0];
            BP[i].gameObject.GetComponent<dragPlayer>().playerName = SaveAndLoadName.EnemyPlayerInfo[i,1];
        }

        for(int i = 0;i < 12;i++) {
            SP[i].gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.SelfPlayerInfo[i,0];
            SP[i].gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.SelfPlayerInfo[i,1];
            EP[i].gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.EnemyPlayerInfo[i,0];
            EP[i].gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.EnemyPlayerInfo[i,1];
        }
    }
}
