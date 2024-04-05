from flask import Flask, request, render_template
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

@app.route('/', methods=['GET', 'POST'])
def index():
    return render_template("test.html")

@app.route("/login", methods=['GET', 'POST'])
def login():
    account = request.form.get("account")
    password = request.form.get("password")

    resultReturn = {"success" : False, "situation" : -1, "UserName" : None, "UserID" : None, "numOfGame" : None, "numOfPlayer" : None, "TeamID": None, "ec": None}


    if account == None or password == None:
        resultReturn['ec'] = "form argument error"
        return resultReturn


    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)

    cur.execute("select * from users where account=%s;", (account, ))
    result = cur.fetchall()

    if len(result) == 1:
        try:
            if password == result[0][2]:
                resultReturn['UserName'] = account
                resultReturn['UserID'] = result[0][0]
                cur.execute("select * from users where account=%s;", (account, ))
                UserInfo = cur.fetchall()
                
                if(len(UserInfo) != 1):
                    resultReturn['situation'] = -2
                    resultReturn['ec'] = "len of UserInfo != 1"

                cur.execute("select * from Team where UserID=%s and Name=%s;", (UserInfo[0][0], UserInfo[0][3]))
                TeamInfo = cur.fetchall()
                if len(TeamInfo) != 1:
                    resultReturn['situation'] = -3
                    resultReturn['ec'] = "len of TeamInfo != 1"
                    
                cur.execute("select * from GameInfo where UserID=%s;", (UserInfo[0][0], ))
                resultReturn['numOfGame'] = len(cur.fetchall())
                cur.execute("select * from Player where UserID=%s and TeamID=%s;", (UserInfo[0][0], TeamInfo[0][1]))
                resultReturn['numOfPlayer'] = len(cur.fetchall())
                
                resultReturn['success'] = True
                resultReturn['situation'] = 0
            else:
                resultReturn['situation'] = -4
                resultReturn['ec'] = "pwd error"
        except Exception as ec:
            resultReturn['ec'] = str(ec)
            resultReturn['situation'] = -5
        finally:
            cur.close()
            cnx.close()
    else:
        resultReturn['ec'] = "len of users != 1"
        resultReturn['situation'] = -6

    return resultReturn

@app.route("/register", methods=['GET', 'POST'])
def register():
    account = request.form.get("account")
    password = request.form.get("password")
    
    resultReturn = {"success" : False, "situation" : -1, "UserName" : None, "UserID" : None, "numOfGame" : None, "numOfPlayer" : None, "ec":""}


    if account == None or password == None:
        return resultReturn

    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)

    cur.execute("select account from users where account=%s;", (account,))
    result = cur.fetchall()


    if len(result) == 0:
        if re.match("^[a-zA-Z0-9]+$", account):
            
            try:
                cur.execute("insert into users(account, hash) value(%s, %s);", (account, password))
                cnx.commit()
                cur.execute("select * from users where account=%s;", (account, ))
                userId = int(cur.fetchall()[0][0])
            except Exception as ec:
                resultReturn['ec'] = str(ec)
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
    else:
        resultReturn['situation'] = -4

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
                             "round int, role varchar(50), attackblock varchar(50), catchblock varchar(50), situation int, score int) engine = InnoDB default charset=utf8mb4 collate=utf8mb4_bin;")    
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
    print(check1)

    if len(check1) == 1: # 帳號存在
        cur.execute("select * from GameInfo where UserID=%s and GameID=%s;", (UserID, GameID))
        check2 = cur.fetchall()
        if len(check2) == 1: # 比賽存在
            cur.execute("show tables like 'GameData';")
            check3 = cur.fetchall()
            if len(check3) == 1: # 資料表存在
                try:
                    for i in range(len(data)):
                        
                        cur.execute("insert into GameData(UserID, GameID, `set`, TeamID, side, Player1, Player2, Player3, formation, startx, starty, endx, endy, behavior, score) "
                                    "value(%s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s);", 
                                    (UserID, GameID, data[i]['round'], data[i]['teamNum'], data[i]['side'], data[i]['role1'], data[i]['role2'], data[i]['role3'], 
                                     data[i]['formation'], data[i]['startx'], data[i]['starty'], data[i]['endx'], data[i]['endy'], data[i]['situation'], data[i]['score']))
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
    TeamID = request.form.get("TeamID")
    PlayerName = request.form.get("PlayerName")
    PlayerNumber = request.form.get("PlayerNumber")
    position = request.form.get("position")
    
    resultReturn = {"success" : False, "situation": -1} # 0 成功 -1 參數錯誤 -2 資料庫錯誤 -3 帳號不存在 -4 該球員已存在 -5 該背號已存在
    
    if account == None or UserID == None or PlayerName == None or PlayerNumber == None or TeamID == None:
        return resultReturn
    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)

    cur.execute("select account from users where account=%s;", (account,))
    result = cur.fetchall()
    
    cur.execute("select * from Player where UserID=%s and TeamID=%s and PNum=%s;", (UserID, TeamID, PlayerNumber))
    check1 = cur.fetchall()
    
    cur.execute("select * from Team where UserID=%s and TeamID=%s;", (UserID, TeamID))
    check2 = cur.fetchall()
    
    if len(result) == 1 and len(check1) == 0 and len(check2) == 1:
        try:
            cur.execute("insert into Player(UserID, TeamID, PName, PNum, Position) value(%s, %s, %s, %s, %s)", (UserID, TeamID, PlayerName, PlayerNumber, position))
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
    elif len(result) == 1 and len(check2) != 1:
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
    
    cur.execute("select * from GameInfo where UserID=%s and GameName=%s and GameDate=%s;", (UserID, GameName, GameDate))
    check = cur.fetchall()
    
    if len(result) == 1 and len(check) == 0:
        try:
            GameDate = datetime.strptime(GameDate, "%Y-%m-%d")
            cur.execute("insert into GameInfo(UserID, Serve, TeamL, TeamR, GameDate, GameName) value(%s, %s, %s, %s, %s, %s)", (UserID, -1, -1, -1, GameDate, GameName))
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

    resultReturn = {"success" : False, "situation": -1, 
                    "UserPlayerName" : None, "UserPlayerNumber" : None, "UserPlayerPos": None,
                    "UserGameID" : None, "GameServe": None, "TeamL": None, "TeamR": None, 
                    "UserGameDate" : None, "UserGameName" : None}
    
    if account == None or UserID == None: # 0 成功 -1 參數錯誤 -2 資料庫錯誤 -3 帳號不存在
        return resultReturn
    
    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)
    
    cur.execute("select account from users where account=%s;", (account,))
    result = cur.fetchall()
    UserPlayerID = [], UserPlayerName = []; UserPlayerNumber = [], UserPlayerPos = []
    UserGameID = []; UserGameDate = []; UserGameName = []; GameServe = []; TeamL = []; TeamR = []
    
    if len(result) == 1:
        try:
            cur.execute("select * from Player where UserID=%s order by PNum;", (UserID))
            Player = cur.fetchall()
            cur.execute("select * from GameInfo where UserID=%s order by GameDate;", (UserID, ))
            Game = cur.fetchall()
            for i in Player:
                UserPlayerID.append(i[0])
                UserPlayerName.append(i[3])
                UserPlayerNumber.append(i[4])
                UserPlayerPos.append(i[5])
            for i in Game:
                UserGameID.append(i[0])
                GameServe.append(i[2])
                TeamL.append(i[3])
                TeamR.append(i[4])
                UserGameDate.append(i[5])
                UserGameName.append(i[6])
            
            resultReturn['UserPlayerName'] = UserPlayerName
            resultReturn['UserPlayerNumber'] = UserPlayerNumber
            resultReturn['UserPlayerPos'] = UserPlayerPos
            resultReturn['UserGameID'] = UserGameID
            resultReturn["GameServe"] = GameServe
            resultReturn['TeamL'] = TeamL
            resultReturn['TeamR'] = TeamR
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
    PlayerID = request.form.get("PlayerID")
    UserID = request.form.get("UserID")
    PlayerName = request.form.get("PlayerName")
    PlayerNumber = request.form.get("PlayerNumber")
    
    resultReturn = {"success" : False, "situation": -1} # 0 成功 -1 參數錯誤 -2 資料庫錯誤 -3 帳號不存在 -4 該球員已存在 -5 該背號已存在
    
    if account == None or PlayerID == None or UserID == None or PlayerName == None or PlayerNumber == None :
        return resultReturn
    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)

    cur.execute("select account from users where account=%s;", (account,))
    result = cur.fetchall()
    
    cur.execute("select * from Player where PlayerID=%s;", (PlayerID,))
    check1 = cur.fetchall()
    
    cur.execute("select * from Player where PName=%s and PlayerID != %s;", (PlayerName, PlayerID))
    check2 = cur.fetchall()
    
    cur.execute("select * from Player where PNum=%s and PlayerID != %s;", (PlayerNumber, PlayerID))
    check3 = cur.fetchall()
    
    if len(result) == 1 and len(check1) == 1 and len(check2) == 0 and len(check3) == 0:
        try:
            cur.execute("update Player set PName=%s, PNum=%s where PlayerID=%s;", (PlayerName, PlayerNumber, int(PlayerID)))
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
    
@app.route("/graphicData", methods=['GET', 'POST'])
def graphicData():
    UserID = request.args.get("UserID")
    GameID = request.args.get("GameID")


    print(UserID, GameID)

    if UserID == None or GameID == None:
        return "Invalid UserID/GameID"

    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)

    result = {
        
        
        
        "teamL": "左方球隊名",
        "teamR": "右方球隊名",
        "matchDate": "",
        
        "totalSet": 0, #OK
        
        "Serve": 0,
        
        "LREachRound":{ #OK
            "left": [],
            "right": []
        },
        
        "players": { #OK
            "left": [
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0}
            ],
            "right": [
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0},
                {"name": "", "num": 0, "pos": 0}
            ]
        },
        "scores": [
            {"leftScore": 0, "rightScore": 0, "winner":0},
            {"leftScore": 0, "rightScore": 0, "winner":0},
            {"leftScore": 0, "rightScore": 0, "winner":0},
            {"leftScore": 0, "rightScore": 0, "winner":0},
            {"leftScore": 0, "rightScore": 0, "winner":0}
        ],
        "ballRecords": {
            "set1": [],
            "set2": [],
            "set3": [],
            "set4": [],
            "set5": []
        }
    }
    try:
        cur.execute("select * from GameInfo where UserID=%s and GameID=%s;", (UserID, GameID))
        GameInfo = cur.fetchall()
        if(len(GameInfo) != 1):
            return "Database Error(GameInfo)!"
        
        cur.execute("select * from FirstPlayer where UserID=%s and GameID=%s;", (UserID, GameID))
        FirstPlayer = cur.fetchall()
        if(len(FirstPlayer) != 1):
            return "Database Error(FirstPlayer)!"
        
        cur.execute("select * from SetSide where UserID=%s and GameID=%s;", (UserID, GameID))
        SetSide = cur.fetchall()
        if(len(SetSide) != 1):
            return "Database Error!(SetSide)"
        
        cur.execute("select * from GameData where UserID=%s and GameID=%s;", (UserID, GameID))
        GameData = cur.fetchall()
        if(len(GameData) == 0):
            return "No Data"
        
        cur.execute("select * from Player where UserID=%s and TeamID=%s;", (UserID, GameInfo[0][3]))
        playerDataL = cur.fetchall()
        if(len(playerDataL) < 6):
            return "Player Error!"
    
        cur.execute("select * from Player where UserID=%s and TeamID=%s;", (UserID, GameInfo[0][4]))
        playerDataR = cur.fetchall()
        if(len(playerDataR) < 6):
            return "Player Error!"

        cur.execute("select * from Team where UserID=%s and TeamID=%s;", (UserID, GameInfo[0][3]))
        TeamNameL = cur.fetchall()
        if(len(TeamNameL) != 1 ):
            return "Team Error!"
        cur.execute("select * from Team where UserID=%s and TeamID=%s;", (UserID, GameInfo[0][4]))
        TeamNameR = cur.fetchall()
        if(len(TeamNameR) != 1):
            return "Team Error!"
    
    
    except Exception as ex:
        print(ex)
        return "Database Error!" + ex
    finally:
        cur.close()
        cnx.close()
    
    for i in range(12):
        if(i < len(playerDataL)):
            result["players"]["left"][i]["name"] = playerDataL[i][2]
            result["players"]["left"][i]["num"] = playerDataL[i][3]
            result["players"]["left"][i]["pos"] = playerDataL[i][4]
        if(i < len(playerDataR)):
            result["players"]["right"][i]["name"] = playerDataR[i][2]
            result["players"]["right"][i]["num"] = playerDataR[i][3]
            result["players"]["right"][i]["pos"] = playerDataR[i][4]
    
    set1L = 0; set1R = 0
    set2L = 0; set2R = 0
    set3L = 0; set3R = 0
    set4L = 0; set4R = 0
    set5L = 0; set5R = 0
    
    for data in GameData:
        tmp = {"set": 0, "format": "", "player1": 0, "player2": 0, "player3": 0, "startx": 0, "starty": 0, "endx": 0, "endy": 0, "behavior": 0, "score": 0}
        tmp["set"] = data[3]; tmp["format"] = data[9]; 
        tmp["player1"] = data[6]; tmp["player2"] = data[7]; tmp["player3"] = data[8]
        tmp["startx"] = 10; tmp["starty"] = 11; tmp["endx"] = 12; tmp["endy"] = data[13]
        tmp["behavior"] = data[14]; tmp["score"] = data[15]
        if(data[3] == 1):
            set1L = set1L + 1 if data[15] > 0 else set1L
            set1R = set1R + 1 if data[15] < 0 else set1R
            result["ballRecords"]["set1"].append(tmp)
        elif(data[3] == 2):
            set2L = set2L + 1 if data[15] > 0 else set2L
            set2R = set2R + 1 if data[15] < 0 else set2R
            result["ballRecords"]["set2"].append(tmp)
        elif(data[3] == 3):
            set3L = set3L + 1 if data[15] > 0 else set3L
            set3R = set3R + 1 if data[15] < 0 else set3R
            result["ballRecords"]["set3"].append(tmp)
        elif(data[3] == 4):
            set4L = set4L + 1 if data[15] > 0 else set4L
            set4R = set4R + 1 if data[15] < 0 else set4R
            result["ballRecords"]["set4"].append(tmp)
        elif(data[3] == 5):
            set5L = set5L + 1 if data[15] > 0 else set5L
            set5R = set5R + 1 if data[15] < 0 else set5R
            result["ballRecords"]["set5"].append(tmp)

    result['scores'][0]["leftScore"] = set1L
    result['scores'][0]["rightScore"] = set1R
    result['scores'][1]["leftScore"] = set2L
    result['scores'][1]["rightScore"] = set2R
    result['scores'][2]["leftScore"] = set3L
    result['scores'][2]["rightScore"] = set3R
    result['scores'][3]["leftScore"] = set4L
    result['scores'][3]["rightScore"] = set4R
    result['scores'][4]["leftScore"] = set5L
    result['scores'][4]["rightScore"] = set5R
    
    result["Serve"] = GameInfo[0][2]
    result["teamL"] = TeamNameL[0][2]
    result['teamR'] = TeamNameR[0][2]
    for i in range(5):
        left = GameInfo[0][3]
        right = GameInfo[0][4]
        if left != SetSide[0][i+2]:
            right = left
            left = SetSide[0][i+2]
        result["LREachRound"]["left"].append(left)
        result["LREachRound"]["right"].append(right)
    
    try:
        cnx = mysql.connector.connect(**config)
        cur = cnx.cursor(buffered=True)
        cur.execute("select * from SetWinner where UserID=%s and GameID=%s;", (UserID, GameID))
        SetWinner = cur.fetchall()
        if(len(SetWinner) == 0):
            SetWinner = []
            count = 0
            for i in range(5):
                if result['scores'][i]["leftScore"] != 0 or result['scores'][i]["rightScore"]:
                    count += 1
                if result['scores'][i]["leftScore"] > result['scores'][i]["rightScore"]:
                    SetWinner.append(result["LREachRound"]["left"][i])
                elif result['scores'][i]["leftScore"] < result['scores'][i]["rightScore"]:
                    SetWinner.append(result["LREachRound"]["right"][i])
                else:
                    SetWinner.append(-1)
                result["scores"][i]['winner'] = SetWinner[0][i]
            cur.execute("insert into SetWinner(UserID, GameID, TotalSet, Set1, Set2, Set3, Set4, Set5) value(%s, %s, %s, %s, %s, %s, %s, %s)"
                        ,(UserID, GameID, count, SetWinner[0], SetWinner[1], SetWinner[2], SetWinner[3], SetWinner[4]))
            cnx.commit()
            result["totalSet"] = count
        elif(len(SetWinner) == 1):
            for i in range(5):
                result["scores"][i]['winner'] = SetWinner[0][i + 3]
            result["totalSet"] = SetWinner[0][2]
        elif(len(SetWinner) > 1):
            return "Database Error!"
        
    except Exception as ec:
        print(ec)
        return "Database Error!"
    finally:
        cur.close()
        cnx.close()    
    
    



    return result

@app.route("/SetTeam", methods=['GET', 'POST'])
def SetTeam():
    UserID = request.form.get("UserID")
    TeamName = request.form.get("TeamName")
    
    resultReturn = {"success" : False, "situation": -1, "TeamID": 0, "ec":""} # 0 成功 -1 參數錯誤 -2 資料庫錯誤 -3 帳號不存在 -4 該球員已存在 -5 該背號已存在
    if UserID == None or TeamName == None:
        return resultReturn
    
    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)
    
    try:
        cur.execute("select * from users where UserID=%s;", (UserID, ))
        check1 = cur.fetchall()
        if len(check1) != 1:
            resultReturn["situation"] = -2
            return resultReturn
        
        cur.execute("select * from Team where UserID=%s and Name=%s", (UserID, TeamName))
        check2 = cur.fetchall()
        if len(check2) != 0:
            resultReturn["situation"] = -3
            return resultReturn
        cur.execute("insert into Team(UserID, Name) value(%s, %s);", (UserID, TeamName))
        cur.execute("update users set TeamName=%s where UserID=%s;", (TeamName, UserID))
        cnx.commit()
        cur.execute("select * from Team where UserID=%s and Name=%s", (UserID, TeamName))
        check3 = cur.fetchall()
        if(len(check3) != 1):
            resultReturn["situation"] = -4
            return resultReturn
        resultReturn["success"] = True
        resultReturn["situation"] = 0
        resultReturn["TeamID"] = check3[0][1]
        
        return resultReturn
        
    except Exception as ec:
        resultReturn["situation"] = -5
        resultReturn["ec"] = str(ec)
        print(ec)
        return resultReturn
    finally:
        cur.close()
        cnx.close()

if __name__ == '__main__':
    app.run(port=5000, debug=True)   