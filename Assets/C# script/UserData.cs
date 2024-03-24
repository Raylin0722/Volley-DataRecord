using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public static UserData Instance;

    // 登入系統需要資料
    public string UserName;
    public int UserID;
    public int numOfGame;
    public int numOfPlayer;

    // 以下為紀錄系統需要資料
    public int GameID;
    public string GameName;
    public List<int> UserPlayerID; // 伺服器中儲存的ID
    public List<string> UserPlayerName; // 名稱
    public List<int> UserPlayerNumber; // 背號
    public List<int> UserPlayerPlayPos; // 打的位置
    public List<int> EnemyPlayerID;
    public List<string> EnemyPlayerName;
    public List<int> EnemyPlayerNumber;
    public List<int> EnemyPlayerPlayPos;
    public string UserTeamName;
    public string EnemyTeamName;
    public int whoServe;
    public int leftRight;
    public UserData(){
        
    }
    void Awake(){
        /*if(Instance != null){
            Destroy(gameObject);
            return;
        }*/

        /*Instance = this;
        Instance.tag = "DontDestroy";
        DontDestroyOnLoad(gameObject);*/
        Instance = this;
        UserName = "Test";
        UserID = 1; 
        numOfGame = 1;
        numOfPlayer = 12;
        GameID = 1;
        GameName  = "TestGame";
        UserPlayerID = new List<int>{1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};
        UserPlayerName = new List<string>{"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l"};
        UserPlayerNumber = new List<int>{1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};
        EnemyPlayerID = new List<int>{13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24};
        EnemyPlayerName = new List<string>{"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L"};
        EnemyPlayerNumber = new List<int>{13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24};
        UserTeamName = "Team A";
        EnemyTeamName = "Team B";
        whoServe = 0; // 0 Left 1 Right
        leftRight = 0; // 0 Left 1 Right

    }
}
