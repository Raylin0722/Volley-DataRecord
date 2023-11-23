using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveAndLoadName : MonoBehaviour
{
    [SerializeField] public static string[] TeamName = new string[2];
    [SerializeField] public static string[,] SelfPlayerInfo = new string[12, 2];
    [SerializeField] public static string[,] EnemyPlayerInfo = new string[12, 2];
    [SerializeField] InputField SelfTeamName, EnemyTeamName;
    [SerializeField] InputField[] SPNumber;
    [SerializeField] InputField[] SPName;
    [SerializeField] InputField[] EPNumber;
    [SerializeField] InputField[] EPName;
    public void readTeamName() {
        TeamName[0] = SelfTeamName.text;
        TeamName[1] = EnemyTeamName.text;
    }
    public void readSelfInfo() {
        for(int i = 0;i < 12;i++) {
            SelfPlayerInfo[i, 0] = SPNumber[i].text;
            SelfPlayerInfo[i, 1] = SPName[i].text;
        }
    }
    public void readEnemyInfo() {
        for(int i = 0;i < 12;i++) {
            EnemyPlayerInfo[i, 0] = EPNumber[i].text;
            EnemyPlayerInfo[i, 1] = EPName[i].text;
        }
    }
    void Awake() {
        SelfTeamName.text = TeamName[0]; EnemyTeamName.text = TeamName[1];
        for(int i = 0;i < 12;i++) {
            SPNumber[i].text = SelfPlayerInfo[i,0];
            SPName[i].text = SelfPlayerInfo[i,1];
            EPNumber[i].text = EnemyPlayerInfo[i,0];
            EPName[i].text = EnemyPlayerInfo[i,1];
        }
    }
}
