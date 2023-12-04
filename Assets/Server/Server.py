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

@app.route("/login")
def login():
    account = request.form.get("account")
    password = request.form.get("password")

    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)

    resultReturn = {"success" : False}
    cur.execute("selcet * from users where account=%s;", (account, ))
    result = cur.fetchall()

    if len(result) == 1:
        if hashlib.sha256(password.encode("utf-8")).hexdigest() == result[0][1]:
            resultReturn['success'] = True

    cur.close()
    cnx.close()

@app.route("/register")
def register():
    account = request.form.get("account")
    password = request.form.get("password")
        

    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)

    cur.execute("select account from users where account=%s;", (account,))
    result = cur.fetchall()

    returnResult = {"success" : False}

    if len(result) == 0 and re.match("^[a-zA-Z0-9]+$", account):
        hashResult = hashlib.sha256(password.encode("utf-8")).hexdigest()
        cur.execute("insert into users(account, hash) value(%s, %s);", (account, hashResult))
        cnx.commit()
        try:
            cur.execute(f"create table {account} (ID INT AUTO_INCREMENT PRIMARY KEY, gameName VARCHAR(50));")
        except:
            returnResult['success'] = False
        else:
            returnResult['success'] = True
        finally:
            cur.close()
            cnx.close()

    return returnResult

@app.route("/initDB")
def initDB():
    gameName = request.form.get("gameName")
    account = request.form.get("account")
    
    cnx = mysql.connector.connect(**config)
    cur = cnx.cursor(buffered=True)

    resultReturn = {"success" : False}

    try:
        cur.execute(f"show tables like {account};")
        result = cur.fetchall()
        if result and re.match("^[a-zA-Z0-9]+$", account) and re.match("^[a-zA-Z0-9]+$", gameName): 
            cur.execute("select * from {account} where gameName=%s", (gameName, ))
            checkGameExist = cur.fetchall()
            if len(checkGameExist) == 0:
                cur.execute("insert into `{account}`(`gameName`) value(%s);", (account+'_'+gameName, ))
                cnx.commit()
                resultReturn['success'] = True
    except:
        resultReturn['success'] = False
    finally:
        cur.close()
        cnx.close()
        
    return resultReturn
    

    
    