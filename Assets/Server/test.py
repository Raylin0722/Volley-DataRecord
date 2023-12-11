from flask import Flask, request
import mysql.connector
import secrets
import hashlib
from datetime import datetime, timedelta
import json
import sys


config = {
    'user': 'root',        
    'password': 'test',        
    'database': 'Volleyball',        
    'host': 'localhost',        
    'port': '3306'        
}

cnx = mysql.connector.connect(**config)
cur = cnx.cursor(buffered=True)

cur.execute("CREATE TABLE %s (column1 INT, column2 VARCHAR(255))", ("test",))
print("success")

cur.close()
cnx.close()