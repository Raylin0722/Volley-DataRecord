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
    public List<int> UserPlayerID;
    public List<string> UserPlayerName;
    public List<int> UserPlayerNumber;
    public List<int> EnemyPlayerID;
    public List<string> EnemyPlayerName;
    public List<int> EnemyPlayerNumber;
    public string UserTeamName;
    public string EnemyTeamName;
    public int whoServe;
    public int leftRight;
    void Awake(){
        if(Instance != null){
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Instance.tag = "DontDestroy";
        DontDestroyOnLoad(gameObject);   
    }
}
