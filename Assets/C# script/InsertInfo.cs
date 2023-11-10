using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class InsertInfo : MonoBehaviour
{
    [SerializeField] Text SelfTeamName, EnemyTeamName;
    [SerializeField] GameObject WP1, WP2, WP3, WP4, WP5, WP6; //self player on field
    [SerializeField] GameObject BP1, BP2, BP3, BP4, BP5, BP6; //enemy player on field
    [SerializeField] GameObject SP1, SP2, SP3, SP4, SP5, SP6, SP7, SP8, SP9, SP10, SP11, SP12; // all of self player
    [SerializeField] GameObject EP1, EP2, EP3, EP4, EP5, EP6, EP7, EP8, EP9, EP10, EP11, EP12; // all of enemy player
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
        WP1.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.SelfPlayerInfo[0,0];
        WP1.gameObject.GetComponent<dragPlayer>().playerName = SaveAndLoadName.SelfPlayerInfo[0,1];
        WP2.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.SelfPlayerInfo[1,0];
        WP2.gameObject.GetComponent<dragPlayer>().playerName = SaveAndLoadName.SelfPlayerInfo[1,1];
        WP3.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.SelfPlayerInfo[2,0];
        WP3.gameObject.GetComponent<dragPlayer>().playerName = SaveAndLoadName.SelfPlayerInfo[2,1];
        WP4.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.SelfPlayerInfo[3,0];
        WP4.gameObject.GetComponent<dragPlayer>().playerName = SaveAndLoadName.SelfPlayerInfo[3,1];
        WP5.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.SelfPlayerInfo[4,0];
        WP5.gameObject.GetComponent<dragPlayer>().playerName = SaveAndLoadName.SelfPlayerInfo[4,1];
        WP6.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.SelfPlayerInfo[5,0];
        WP6.gameObject.GetComponent<dragPlayer>().playerName = SaveAndLoadName.SelfPlayerInfo[5,1];

        BP1.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.EnemyPlayerInfo[0,0];
        BP1.gameObject.GetComponent<dragPlayer>().playerName = SaveAndLoadName.EnemyPlayerInfo[0,1];
        BP2.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.EnemyPlayerInfo[1,0];
        BP2.gameObject.GetComponent<dragPlayer>().playerName = SaveAndLoadName.EnemyPlayerInfo[1,1];
        BP3.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.EnemyPlayerInfo[2,0];
        BP3.gameObject.GetComponent<dragPlayer>().playerName = SaveAndLoadName.EnemyPlayerInfo[2,1];
        BP4.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.EnemyPlayerInfo[3,0];
        BP4.gameObject.GetComponent<dragPlayer>().playerName = SaveAndLoadName.EnemyPlayerInfo[3,1];
        BP5.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.EnemyPlayerInfo[4,0];
        BP5.gameObject.GetComponent<dragPlayer>().playerName = SaveAndLoadName.EnemyPlayerInfo[4,1];
        BP6.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.EnemyPlayerInfo[5,0];
        BP6.gameObject.GetComponent<dragPlayer>().playerName = SaveAndLoadName.EnemyPlayerInfo[5,1];

        SP1.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.SelfPlayerInfo[0,0];
        SP1.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.SelfPlayerInfo[0,1];
        SP2.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.SelfPlayerInfo[1,0];
        SP2.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.SelfPlayerInfo[1,1];
        SP3.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.SelfPlayerInfo[2,0];
        SP3.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.SelfPlayerInfo[2,1];
        SP4.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.SelfPlayerInfo[3,0];
        SP4.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.SelfPlayerInfo[3,1];
        SP5.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.SelfPlayerInfo[4,0];
        SP5.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.SelfPlayerInfo[4,1];
        SP6.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.SelfPlayerInfo[5,0];
        SP6.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.SelfPlayerInfo[5,1];
        SP7.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.SelfPlayerInfo[6,0];
        SP7.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.SelfPlayerInfo[6,1];
        SP8.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.SelfPlayerInfo[7,0];
        SP8.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.SelfPlayerInfo[7,1];
        SP9.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.SelfPlayerInfo[8,0];
        SP9.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.SelfPlayerInfo[8,1];
        SP10.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.SelfPlayerInfo[9,0];
        SP10.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.SelfPlayerInfo[9,1];
        SP11.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.SelfPlayerInfo[10,0];
        SP11.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.SelfPlayerInfo[10,1];
        SP12.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.SelfPlayerInfo[11,0];
        SP12.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.SelfPlayerInfo[11,1];

        EP1.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.EnemyPlayerInfo[0,0];
        EP1.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.EnemyPlayerInfo[0,1];
        EP2.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.EnemyPlayerInfo[1,0];
        EP2.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.EnemyPlayerInfo[1,1];
        EP3.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.EnemyPlayerInfo[2,0];
        EP3.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.EnemyPlayerInfo[2,1];
        EP4.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.EnemyPlayerInfo[3,0];
        EP4.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.EnemyPlayerInfo[3,1];
        EP5.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.EnemyPlayerInfo[4,0];
        EP5.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.EnemyPlayerInfo[4,1];
        EP6.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.EnemyPlayerInfo[5,0];
        EP6.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.EnemyPlayerInfo[5,1];
        EP7.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.EnemyPlayerInfo[6,0];
        EP7.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.EnemyPlayerInfo[6,1];
        EP8.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.EnemyPlayerInfo[7,0];
        EP8.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.EnemyPlayerInfo[7,1];
        EP9.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.EnemyPlayerInfo[8,0];
        EP9.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.EnemyPlayerInfo[8,1];
        EP10.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.EnemyPlayerInfo[9,0];
        EP10.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.EnemyPlayerInfo[9,1];
        EP11.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.EnemyPlayerInfo[10,0];
        EP11.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.EnemyPlayerInfo[10,1];
        EP12.gameObject.GetComponentInChildren<TextMeshPro>().text = SaveAndLoadName.EnemyPlayerInfo[11,0];
        EP12.gameObject.GetComponent<dragPlayerToChange>().PlayerName = SaveAndLoadName.EnemyPlayerInfo[11,1];
    }
}
