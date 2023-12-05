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

    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)

    resultReturn = {"success" : False, "situation" : -1}
    cur.execute("select * from users where account=%s;", (account, ))
    result = cur.fetchall()

    print(password, result[0][1], sep='\n')

    print(result)

    if len(result) == 1:
        if password == result[0][2]:
            resultReturn['success'] = True
            resultReturn['situation'] = 0
        else:
            resultReturn['situation'] = -2


    cur.close()
    cnx.close()

    return resultReturn

@app.route("/register", methods=['GET', 'POST'])
def register():
    account = request.form.get("account")
    password = request.form.get("password")
        
    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)

    cur.execute("select account from users where account=%s;", (account,))
    result = cur.fetchall()

    returnResult = {"success" : False, "situation" : -1}

    if len(result) == 0:
        if re.match("^[a-zA-Z0-9]+$", account):
            
            cur.execute("insert into users(account, hash) value(%s, %s);", (account, password))
            cnx.commit()
            cur.execute("SELECT COUNT(*) FROM users;")
            userId = int(cur.fetchall()[0][0])
            try:
                cur.execute(f"create table user{userId} (ID INT AUTO_INCREMENT PRIMARY KEY, gameName VARCHAR(50));")
            except:
                returnResult['success'] = False
                returnResult['situation'] = -2
                
            else:
                returnResult['success'] = True
                returnResult['situation'] = 0

            finally:
                cur.close()
                cnx.close()
        else:
            returnResult['situation'] = -3

    return returnResult

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