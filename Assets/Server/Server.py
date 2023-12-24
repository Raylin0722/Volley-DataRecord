from flask import Flask, request
import mysql.connector
import secrets
import hashlib
from datetime import datetime, timedelta
import json
import sys
import re

app = Flask(__name__)

config = {
    'user': 'root',        
    'password': 'test',        
    'database': 'Volleyball',        
    'host': 'localhost',        
    'port': '3306'        
}


@app.route('/')
def index():
    return 'index'

@app.route("/login", methods=['GET', 'POST'])
def login():
    account = request.form.get("account")
    password = request.form.get("password")

    resultReturn = {"success" : False, "situation" : -1, "UserName" : None, "UserID" : None, "numOfGame" : None, "numOfPlayer" : None}


    print(password)

    if account == None or password == None:
        return resultReturn


    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)

    cur.execute("select * from users where account=%s;", (account, ))
    result = cur.fetchall()

    if len(result) == 1:
        try:
            if password == result[0][2]:
                print("a")
                resultReturn['success'] = True
                resultReturn['situation'] = 0
                resultReturn['UserName'] = account
                resultReturn['UserID'] = result[0][0]
                cur.execute("select * from users where account=%s;", (account, ))
                userID = int(cur.fetchall()[0][0])
                cur.execute(f"SELECT COUNT(*) FROM userGame{userID};")
                resultReturn['numOfGame'] = int(cur.fetchall()[0][0])
                cur.execute(f"SELECT COUNT(*) FROM userPlayer{userID};")
                resultReturn['numOfPlayer'] = int(cur.fetchall()[0][0])
            else:
                resultReturn['situation'] = -2
        except Exception as ec:
            print(ec)
            resultReturn['situation'] = -1
        finally:
            cur.close()
            cnx.close()

    return resultReturn

@app.route("/register", methods=['GET', 'POST'])
def register():
    account = request.form.get("account")
    password = request.form.get("password")
    
    resultReturn = {"success" : False, "situation" : -1, "UserName" : None, "UserID" : None, "numOfGame" : None, "numOfPlayer" : None}


    if account == None or password == None:
        resultReturn['situation'] = -3
        return resultReturn

    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)

    cur.execute("select account from users where account=%s;", (account,))
    result = cur.fetchall()


    if len(result) == 0:
        if re.match("^[a-zA-Z0-9]+$", account):
            
            cur.execute("insert into users(account, hash) value(%s, %s);", (account, password))
            cnx.commit()
            cur.execute("select * from users;")
            userId = int(cur.fetchall()[0][0])
            try:
                cur.execute(f"create table userGame{userId} (ID INT AUTO_INCREMENT PRIMARY KEY, GameDate Date, GameName VARCHAR(50));")
                cur.execute(f"create table userPlayer{userId} (ID INT AUTO_INCREMENT PRIMARY KEY, PlayerName VARCHAR(50), PlayerNumber int);")
            except Exception as ec:
                print(ec)
                resultReturn['success'] = False
                resultReturn['situation'] = -2
                
            else:
                resultReturn['success'] = True
                resultReturn['situation'] = 0
                resultReturn['UserID'] = userId
                resultReturn['numOfGame'] = 0
                resultReturn['numOfPlayer'] = 0
                resultReturn['UserName'] = account

            finally:
                cur.close()
                cnx.close()
        else:
            resultReturn['situation'] = -3

    return resultReturn

@app.route("/initDB", methods=['GET', 'POST'])
def initDB():
    GameID = request.form.get("GameID")
    UserID = request.form.get("UserID")
    resultReturn = {"success" : False, "situation": -1}
    
    if GameID == None or UserID == None:
        return resultReturn

    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)
    
    cur.execute("select * from users where id=%s;", (UserID, ))
    check1 = cur.fetchall()
    
    if len(check1) == 1: # 帳號存在
        cur.execute(f"select * from userGame{UserID} where ID=%s", (GameID, ))
        check2 = cur.fetchall()
        if len(check2) == 1: # 比賽存在
            try:
                cur.execute(f"create table if not exists GameDataU{UserID}G{GameID}(BallID int auto_increment primary key, formation varchar(50), "
                             "round int, role varchar(50), attackblock varchar(50), catchblock varchar(50), situation int, score int);")    
                resultReturn['situation'] = 0
                resultReturn['success'] = True
            except Exception as ec:
                print(ec)
                resultReturn['situation'] = -2
            finally:
                cur.close()
                cnx.close()
        else :
            resultReturn['situation'] = -3
    else:
        resultReturn['situation'] = -4
        
    return resultReturn
    
@app.route("/insertData", methods=['GET', 'POST'])
def insertData():
    data = request.get_json()
    GameID = request.args.get("GameID")
    UserID = request.args.get("UserID")

    print(data); print(GameID); print(UserID)
    resultReturn = {"success":False, "situation":-1}
    if data == None or GameID == None or UserID == None:
        return resultReturn
    
    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)
    
    cur.execute("select * from users where id=%s;", (UserID, ))
    check1 = cur.fetchall()

    if len(check1) == 1: # 帳號存在
        cur.execute(f"select * from userGame{UserID} where ID=%s", (GameID, ))
        check2 = cur.fetchall()
        if len(check2) == 1: # 比賽存在
            cur.execute(f"SELECT * FROM information_schema.tables WHERE table_schema = 'Volleyball' AND table_name = %s;", (f'GameDataU{UserID}G{GameID}',))
            check3 = cur.fetchall()
            if len(check3) == 1: # 資料表存在
                try:
                    for i in range(len(data)):
                        cur.execute(f"insert into GameDataU{UserID}G{GameID}(formation, round, role, attackblock, catchblock, situation, score) value(%s, %s, %s, %s, %s, %s, %s);", 
                                   (data[i]['formation'], data[i]['round'], data[i]['role'], data[i]['attackblock'], 
                                    data[i]['catchblock'], data[i]['situation'], data[i]['score']))
                        print(data[i])
                    cnx.commit()
                    resultReturn['situation'] = 0
                    resultReturn['success'] = True
                except Exception as ec:
                    print(ec)
                    resultReturn['situation'] = -2
                finally:
                    cur.close()
                    cnx.close()
            else:
                resultReturn['situation'] = -3

        else:
            resultReturn['situation'] = -4
            
    else:
        resultReturn["situation"] = -5
    
    
    return resultReturn
   
@app.route("/displayData", methods=['GET', 'POST'])
def displayData():
    UserID = request.form.get("UserID")
    GameID = request.form.get("GameID")

    if UserID == None or GameID == None:
        return None
    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)
    cur.execute(f"SELECT * FROM information_schema.tables WHERE table_schema = 'Volleyball' AND table_name = %s;", (f'GameDataU{UserID}G{GameID}',))
    result = cur.fetchall()
    dataReturn = []
    if len(result) == 1:
        cur.execute(f"select * from GameDataU{UserID}G{GameID};")
        data = cur.fetchall()
        for i in range(len(data)):
            temp = {"formatiion":data[i][1], "round" : data[i][2], "role" : data[i][3], "attackblock": data[i][4], "catchblock":data[i][5], "situation":data[i][6], "score":data[i][7]}
            dataReturn.append(temp)
        
    return dataReturn
   
@app.route("/AddPlayer", methods=['GET', 'POST'])
def AddPlayer():
    account = request.form.get("account")
    UserID = request.form.get("UserID")
    PlayerName = request.form.get("PlayerName")
    PlayerNumber = request.form.get("PlayerNumber")
    
    resultReturn = {"success" : False, "situation": -1} # 0 成功 -1 參數錯誤 -2 資料庫錯誤 -3 帳號不存在 -4 該球員已存在 -5 該背號已存在
    
    if account == None or UserID == None or PlayerName == None or PlayerNumber == None:
        return resultReturn
    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)

    cur.execute("select account from users where account=%s;", (account,))
    result = cur.fetchall()
    
    cur.execute(f"select * from userPlayer{UserID} where PlayerName=%s;", (PlayerName, ))
    check1 = cur.fetchall()
    
    cur.execute(f"select * from userPlayer{UserID} where PlayerNumber=%s;", (PlayerNumber, ))
    check2 = cur.fetchall()
    
    if len(result) == 1 and len(check1) == 0 and len(check2) == 0:
        try:
            cur.execute(f"insert into userPlayer{UserID}(playerName, playerNumber) value(%s, %s)", (PlayerName, PlayerNumber))
            cnx.commit()
            resultReturn['situation'] = 0
            resultReturn['success'] = True
        except Exception as ex:
            resultReturn['situation'] = -2
            print(ex)
        finally:
            cur.close()
            cnx.close()
    elif len(result) != 1:
        resultReturn['situation'] = -3
        print("-3")
    elif len(result) == 1 and len(check1) != 0:
        resultReturn['situation'] = -4
        print("-4")
    elif len(result) == 1 and len(check2) != 0:
        resultReturn['situation'] = -5
        print("-5")
    return resultReturn

@app.route("/AddGame", methods=['GET', 'POST'])
def AddGame():
    account = request.form.get("account")
    UserID = request.form.get("UserID")
    GameDate = request.form.get("GameDate")
    GameName = request.form.get("GameName")
    
    resultReturn = {"success" : False, "situation": -1} # 0 成功 -1 參數錯誤 -2 資料庫錯誤 -3 帳號不存在 -4 比賽名稱已存在
    
    if account == None or UserID == None or GameName == None or GameDate == None:
        return resultReturn
    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)
    cur.execute("select account from users where account=%s;", (account,))
    result = cur.fetchall()
    
    cur.execute(f"select * from userGame{UserID} where GameName=%s;", (GameName, ))
    check = cur.fetchall()
    
    if len(result) == 1 and len(check) == 0:
        try:
            GameDate = datetime.strptime(GameDate, "%Y-%m-%d")
            cur.execute(f"insert into userGame{UserID}(GameDate, GameName) value(%s, %s)", (GameDate, GameName))
            cnx.commit()
            resultReturn['situation'] = 0
            resultReturn['success'] = True
        except Exception as ex:
            resultReturn['situation'] = -2
            print(ex)
        finally:
            cur.close()
            cnx.close()
    elif len(result) != 1:
        resultReturn['situation'] = -3
    elif len(check) != 0:
        resultReturn['situation'] = -4
    return resultReturn
    
@app.route("/UpdateUserData", methods=['GET', 'POST']) 
def UpdateUserData():
    account = request.form.get("account")
    UserID = request.form.get("UserID")
    print(account, UserID)
    resultReturn = {"success" : False, "situation": -1, 
                    "UserPlayerID" : None, "UserPlayerName" : None, "UserPlayerNumber" : None,
                    "UserGameID" : None, "UserGameDate" : None, "UserGameName" : None}
    
    if account == None or UserID == None: # 0 成功 -1 參數錯誤 -2 資料庫錯誤 -3 帳號不存在
        return resultReturn
    
    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)
    
    cur.execute("select account from users where account=%s;", (account,))
    result = cur.fetchall()
    # cur.execute(f"select * from userGame{UserID} where gameName=%s;", ())
    UserPlayerID = []; UserPlayerName = []; UserPlayerNumber = []
    UserGameID = []; UserGameDate = []; UserGameName = []
    
    if len(result) == 1:
        try:
            cur.execute(f"select * from userPlayer{UserID} order by PlayerNumber;")
            Player = cur.fetchall()
            cur.execute(f"select * from userGame{UserID} order by GameDate;")
            Game = cur.fetchall()
            for i in Player:
                UserPlayerID.append(i[0])
                UserPlayerName.append(i[1])
                UserPlayerNumber.append(i[2])
            for i in Game:
                UserGameID.append(i[0])
                UserGameDate.append(i[1])
                UserGameName.append(i[2])
            resultReturn['UserPlayerID'] = UserPlayerID
            resultReturn['UserPlayerName'] = UserPlayerName
            resultReturn['UserPlayerNumber'] = UserPlayerNumber
            resultReturn['UserGameID'] = UserGameID
            resultReturn['UserGameDate'] = UserGameDate
            resultReturn['UserGameName'] = UserGameName
            resultReturn['success'] = True
            resultReturn['situation'] = 0
        except Exception as es:
            print(es)
            resultReturn['situation'] = -2
            return resultReturn
    
        finally:
            cur.close()
            cnx.close()
    else:
        resultReturn['situation'] = -3
    return resultReturn

@app.route("/CorrectPlayer", methods=['GET', 'POST'])    
def CorrectPlayer():
    account = request.form.get("account")
    UserID = request.form.get("UserID")
    PlayerID = request.form.get("PlayerID")
    PlayerName = request.form.get("PlayerName")
    PlayerNumber = request.form.get("PlayerNumber")
    
    resultReturn = {"success" : False, "situation": -1} # 0 成功 -1 參數錯誤 -2 資料庫錯誤 -3 帳號不存在 -4 該球員已存在 -5 該背號已存在
    
    if account == None or UserID == None or PlayerName == None or PlayerNumber == None or PlayerID == None:
        return resultReturn
    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)

    cur.execute("select account from users where account=%s;", (account,))
    result = cur.fetchall()
    
    cur.execute(f"select * from userPlayer{UserID} where ID=%s;", (PlayerID,))
    check1 = cur.fetchall()
    
    cur.execute(f"select * from userPlayer{UserID} where PlayerName=%s and ID != %s;", (PlayerName, PlayerID))
    check2 = cur.fetchall()
    
    cur.execute(f"select * from userPlayer{UserID} where PlayerNumber=%s and ID != %s;", (PlayerNumber, PlayerID))
    check3 = cur.fetchall()
    
    if len(result) == 1 and len(check1) == 1 and len(check2) == 0 and len(check3) == 0:
        try:
            cur.execute(f"update userPlayer{UserID} set PlayerName=%s, PlayerNumber=%s where ID=%s;", (PlayerName, PlayerNumber, int(PlayerID)))
            cnx.commit()
            resultReturn['situation'] = 0
            resultReturn['success'] = True
        except Exception as ex:
            resultReturn['situation'] = -2
            print(ex)
        finally:
            cur.close()
            cnx.close()
    elif len(result) != 1:
        resultReturn['situation'] = -3
        print("-3")
    elif len(check1) != 1:
        resultReturn['situation'] = -6
        print("-6")
    elif len(result) == 1 and len(check2) != 0:
        resultReturn['situation'] = -4
        print("-4")
    elif len(result) == 1 and len(check3) != 0:
        resultReturn['situation'] = -5
        print("-5")
    return resultReturn
    
if __name__ == '__main__':
    app.run(port=5000)   