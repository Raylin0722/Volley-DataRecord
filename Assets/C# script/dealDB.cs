using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data;
using Mono.Data.Sqlite;
using System;


public class dealDB : MonoBehaviour
{
    [SerializeField] public TextMeshPro[] WPlayer;
    [SerializeField] public TextMeshPro[] BPlayer;
    
    public string formation;

    public const int CATCH = 0;
    public const int SERVE = 1;
    public const int ATTACK = 2;
    public const int BLOCK = 3;
    
    public string gameName;
    public struct Data{
        public string formation;
        public int index;
        public int round;
        public string role;
        public string attackblock, catchblock;
        public int score, situation;

        public Data(string formation, int index, int round, string role, string attackblock,
                    string catchblock, int situation, int score){
            this.formation = formation;
            this.index = index;
            this.round = round;
            this.role = role;
            this.attackblock = attackblock;
            this.catchblock = catchblock;
            this.situation = situation;
            this.score = score;
        }

    }
    public Data[] saveData = new Data[300]; // 儲存資料用
    public int[] saveIndex = new int[1]; // 儲存資料用
    public string databasePath;
    private string SApath = Application.streamingAssetsPath;
    private string dbName = "database.db";

    // Start is called before the first frame update
    void Awake(){

        saveData = new Data[300]; // 儲存資料用
        saveIndex = new int[1]; // 儲存資料用

        DateTime now = DateTime.Now;
        gameName = now.ToString("yyyy_MM_dd");

        databasePath = System.IO.Path.Combine(SApath, dbName);
        //Debug.Log(databasePath);
        databasePath = "URI=file:" + databasePath;
        createDB();
    }

    public void createDB(){
        using(var connection = new SqliteConnection(databasePath)){
            connection.Open();

            using(var command = connection.CreateCommand()){
                command.CommandText = "CREATE TABLE IF NOT EXISTS '" + gameName + "_contestData' (ballID INTEGER PRIMARY KEY AUTOINCREMENT, formation VARCHAR(50),  round INTEGER, role VARCHAR(50), attackblock VARCHAR(30), catchblock VARCHAR(30), situation INTEGER, score INTEGER)";
                //Debug.Log(command.CommandText);
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public void displayData(){
        using(var connection = new SqliteConnection(databasePath)){
            connection.Open();
            using(var command = connection.CreateCommand()){
                command.CommandText = "SELECT * FROM contestData;";

                using(IDataReader reader = command.ExecuteReader()){
                    while(reader.Read()){
                        Debug.Log("Index: " + reader["ballID"] + " Formation: " + reader["formation"] +  " Round: " + reader["round"] + " Role: " + reader["role"] + " Attackblock: " + reader["attackblock"] + " CatchBlock: " + reader["catchblock"] + " Situation: " + reader["situation"] + " Score: " + reader["score"] );
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }
    }

    public void insertData(Data data){
        using(var connection = new SqliteConnection(databasePath)){
            connection.Open();

            using (var command = connection.CreateCommand()){
                command.CommandText = "INSERT INTO '" + gameName + "_contestData' (formation, round, role, attackblock, catchblock, situation, score) VALUES (\"" + data.formation + "\", " + 
                data.round + ", \"" + data.role + "\", \"" + data.attackblock + "\",\"" +
                data.catchblock + "\"," + data.situation + ", " + data.score + ");";
                Debug.Log(command.CommandText);
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

}



