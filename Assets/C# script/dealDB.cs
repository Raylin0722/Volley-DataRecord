using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public struct Data{
    public string formation;
    public int index;
    public int round;
    public int role;
    public int attackblock, catchblock, situation;
    public int score;

    public Data(string formation, int index, int round, int role, int attackblock,
                int catchblock, int situation, int score){
        this.formation = formation;
        this.index = index;
        this.round = round;
        this.role = role;
        this.attackblock = attackblock;
        this.catchblock = catchblock;
        this.situation =situation;
        this.score = score;
    }

}

public class dealDB : MonoBehaviour
{
    private string dbName = "URI=file:"+ Application.dataPath + "database.db";

    // Start is called before the first frame update
    void Start()
    {
        createDB();

        //Data testing = new Data("a", 1, 1, 1, 1, 1, 1, 1);
        //insertData(testing);
        //insertData(testing);

        displayData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    

    public void insertData(Data data){
        using(var connection = new SqliteConnection(dbName)){
            connection.Open();

            using (var command = connection.CreateCommand()){
                command.CommandText = "INSERT INTO contestData (formation, ballID, round, role, attackblock, catchblock, situation, score) VALUES (\"" + data.formation + "\", " + 
                data.index + ", " + data.round + ", " + data.role + ", " + data.attackblock + ", " +
                data.catchblock + "," + data.situation + ", " + data.score + ");";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public void createDB(){
        using(var connection = new SqliteConnection(dbName)){
            connection.Open();

            using(var command = connection.CreateCommand()){
                command.CommandText = "CREATE TABLE IF NOT EXISTS contestData (formation VARCHAR(50), ballID INT, round INT, role, INT, attackblock INT, catchblock INT, situation INT, score INT)";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public void displayData(){
        using(var connection = new SqliteConnection(dbName)){
            connection.Open();
            using(var command = connection.CreateCommand()){
                command.CommandText = "SELECT * FROM contestData;";

                using(IDataReader reader = command.ExecuteReader()){
                    while(reader.Read()){
                        Debug.Log("Formation: " + reader["formation"] + " index: " + reader["ballID"] + " Round: " + reader["round"] + " Role: " + reader["role"] + " Attackblock: " + reader["attackblock"] + " CatchBlock: " + reader["catchblock"] + " Situation: " + reader["situation"] + " Score: " + reader["score"] );
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }
    }

}
