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


    if account == None or password == None:
        resultReturn['situation'] = -2
        return resultReturn


    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)

    cur.execute("select * from users where account=%s;", (account, ))
    result = cur.fetchall()

    if len(result) == 1:
        try:
            if password == result[0][2]:
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
        except:
            resultReturn['situation'] = -2
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
            cur.execute("SELECT COUNT(*) FROM users;")
            userId = int(cur.fetchall()[0][0])
            try:
                cur.execute(f"create table userGame{userId} (ID INT AUTO_INCREMENT PRIMARY KEY, time Date, gameName VARCHAR(50));")
                cur.execute(f"create table userPlayer{userId} (ID INT AUTO_INCREMENT PRIMARY KEY, playerName VARCHAR(50));")
            except:
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
    gameName = request.form.get("gameName")
    account = request.form.get("account")
    
    resultReturn = {"success" : False, "situation" : -1}

    if not re.match("^[a-zA-Z0-9]+$", account):
        resultReturn['situation'] = -2
        return resultReturn
    
    if not re.match("^[a-zA-Z0-9]+$", gameName):
        resultReturn['situation'] = -3
        return resultReturn


    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)

    try:
        cur.execute(f"show tables like {account};")
        result = cur.fetchall()
        if result: 
            cur.execute("select * from {account} where gameName=%s", (gameName, ))
            checkGameExist = cur.fetchall()
            if len(checkGameExist) == 0:
                cur.execute("insert into `{account}`(`gameName`) value(%s);", (account+'_'+gameName, ))
                cnx.commit()
                resultReturn['success'] = True
            else:
                resultReturn['situation'] = -4
        else:
            resultReturn['situation'] = -5

        
    except:
        resultReturn['success'] = False
    finally:
        cur.close()
        cnx.close()
        
    return resultReturn
    
@app.route("/insertData", methods=['GET', 'POST'])
def insertData():
    data = request.get_json()
    account = request.form.get("account")
    gameName = request.form.get("gameName")
    for i in range(len(data)):
        print(data[i])
    return "Test"
   
@app.route("/displayData", methods=['GET', 'POST'])
def displayData():
    ()
    
if __name__ == '__main__':
    app.run(port=5000, debug=True)   