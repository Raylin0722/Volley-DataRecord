using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public static UserData Instance;

    public string UserName;
    public int UserID;
    public int numOfGame;
    public int numOfPlayer;
    public int GameID;
    public string GameName;
    public List<int> UserPlayerID;
    public List<string> UserPlayerName;
    public List<int> UserPlayerNumber;
    public string TeamName;
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
