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

try:
    a = 0/0
except Exception as ec:
    print(ec)
    exit()
finally:
    print("enter finally")
print("No exit")