using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System;

namespace database
{
    public class dealDB : MonoBehaviour
    {
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
            public int attackblock, catchblock, situation;
            public int score;

            public Data(string formation, int index, int round, string role, int attackblock,
                        int catchblock, int situation, int score){
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
        public string dbName;
        private string datapath = Application.streamingAssetsPath;
        private string name = "database.db";

        // Start is called before the first frame update
        void Start(){
            DateTime now = DateTime.Now;
            gameName = now.ToString("yyyy_MM_dd");

            dbName = System.IO.Path.Combine(datapath, name);
            Debug.Log(dbName);
            dbName = "URI=file:" + dbName;
            createDB();
            Data data = new Data(null,0, 0, null, 0, 0, 0, 0);
            using(var connection = new SqliteConnection(dbName)){
                connection.Open();

                using (var command = connection.CreateCommand()){
                    command.CommandText = "INSERT INTO '" + gameName + "_contestData' (formation, round, role, attackblock, catchblock, situation, score) VALUES (\"" + data.formation + "\", " + 
                    data.round + ", \"" + data.role + "\", " + data.attackblock + ", " +
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
                    command.CommandText = "CREATE TABLE IF NOT EXISTS '" + gameName + "_contestData' (ballID INTEGER PRIMARY KEY AUTOINCREMENT, formation VARCHAR(50),  round INTEGER, role VARCHAR(50), attackblock INTEGER, catchblock INTEGER, situation INTEGER, score INTEGER)";
                    //Debug.Log(command.CommandText);
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
                            Debug.Log("Index: " + reader["ballID"] + " Formation: " + reader["formation"] +  " Round: " + reader["round"] + " Role: " + reader["role"] + " Attackblock: " + reader["attackblock"] + " CatchBlock: " + reader["catchblock"] + " Situation: " + reader["situation"] + " Score: " + reader["score"] );
                        }

                        reader.Close();
                    }
                }

                connection.Close();
            }
        }

    }

}

