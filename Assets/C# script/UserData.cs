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
    void Awake(){
        if(Instance != null){
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);   
    }
}
