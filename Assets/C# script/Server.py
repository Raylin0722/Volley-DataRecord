from flask import Flask, request
import mysql.connector
import secrets
import hashlib
from datetime import datetime, timedelta
import json
import sys

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

    if len(result) == 0:
        hashResult = hashlib.sha256(password.encode("utf-8")).hexdigest()
        cur.execute("insert into users(account, hash) value(%s, %s);", (account, hashResult))
        cnx.commit()
        returnResult['success'] = True

    cur.close()
    cnx.close()

    return returnResult

@app.route("/initDB")
def initDB():
    ()