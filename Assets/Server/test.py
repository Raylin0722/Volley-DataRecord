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
    'database': 'test',        
    'host': 'localhost',        
    'port': '3306'        
}

cnx = mysql.connector.connect(**config)
cur = cnx.cursor(buffered=True)

formation = "L1 L2 L3 L4 L5 L6 R13 R14 R15 R16 R17 R18"
formationSplit = formation.split(" ")
serachStrL = ""
serachStrR = "L% L% L% L% L% L% "

print(formationSplit)

for i in range(6):
    serachStrL += formationSplit[i] + " "
    serachStrR += formationSplit[i + 6] + " "
serachStrL += "R% R% R% R% R% R%"
serachStrR = serachStrR[:-1]
print(serachStrL)
print(serachStrR)

cur.execute("select * from testSearch;")
result = cur.fetchall()
for i in result:
    print(i)

cur.execute("select * from testSearch where search LIKE %s;", (serachStrL, ))
SL = cur.fetchall()
for i in SL:
    print(i)

cur.execute("select * from testSearch where search LIKE %s;", (serachStrR, ))
SR = cur.fetchall()
for i in SR:
    print(i)



cur.close()
cnx.close()
