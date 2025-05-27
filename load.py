import csv
import hashlib
import mysql.connector
from mysql.connector import errorcode
from datetime import datetime

DB_CONFIG = {
    'user': 'root',
    'password': 'root',
    'host': '127.0.0.1',
    'database': 'genai',
    'autocommit': False
}

CSV_FILE = 'data/italian_credit_card_transactions.csv'
BATCH_SIZE = 1000

# Connect to MySQL and create schema if not exists
def create_schema():
    cnx = mysql.connector.connect(user=DB_CONFIG['user'], password=DB_CONFIG['password'], host=DB_CONFIG['host'])
    cursor = cnx.cursor()
    with open('ddl.sql', 'r') as f:
        ddl = f.read()
        for stmt in ddl.split(';'):
            if stmt.strip():
                cursor.execute(stmt)
    cnx.commit()
    cursor.close()
    cnx.close()

def hash_password(login_name):
    return hashlib.sha256(login_name.encode('utf-8')).hexdigest()

def main():
    create_schema()
    cnx = mysql.connector.connect(**DB_CONFIG)
    cursor = cnx.cursor()

    jobs = {}
    categories = {}
    users = {}
    job_id = 1
    category_id = 1
    user_id = 1

    # First pass: collect unique jobs, categories, users
    with open(CSV_FILE, newline='', encoding='utf-8') as csvfile:
        reader = csv.DictReader(csvfile)
        for row in reader:
            job = row['job'].strip()
            if job and job not in jobs:
                jobs[job] = job_id
                job_id += 1
            category = row['category'].strip()
            if category and category not in categories:
                categories[category] = category_id
                category_id += 1
            user_key = (row['first'].strip(), row['last'].strip(), row['dob'].strip())
            if user_key not in users:
                users[user_key] = {
                    'id': user_id,
                    'first_name': row['first'].strip(),
                    'last_name': row['last'].strip(),
                    'dob': row['dob'].strip(),
                    'gender': row['gender'].strip(),
                    'city': row['city'].strip(),
                    'job': job,
                    'creation_time': datetime.now().strftime('%Y-%m-%d %H:%M:%S'),
                    'last_login_time': datetime.now().strftime('%Y-%m-%d %H:%M:%S'),
                }
                user_id += 1

    # Insert jobs
    job_items = [(jid, name) for name, jid in jobs.items()]
    job_items.sort()
    cursor.executemany('INSERT IGNORE INTO job (id, name) VALUES (%s, %s)', job_items)

    # Insert categories
    category_items = [(cid, name) for name, cid in categories.items()]
    category_items.sort()
    cursor.executemany('INSERT IGNORE INTO category (id, name) VALUES (%s, %s)', category_items)

    # Insert users
    user_batch = []
    for user_key, user in users.items():
        login_name = (user['first_name'].replace(' ', '') + user['last_name'].replace(' ', '')).lower()
        password = hash_password(login_name)
        job_id_fk = jobs.get(user['job'])
        user_batch.append((user['id'], user['first_name'], user['last_name'], user['dob'], user['gender'], user['city'], job_id_fk, user['creation_time'], user['last_login_time'], login_name, password))
    cursor.executemany('''INSERT IGNORE INTO user (id, first_name, last_name, dob, gender, city, job_id, creation_time, last_login_time, login_name, password) VALUES (%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s)''', user_batch)
    cnx.commit()

    # Second pass: load transactions
    with open(CSV_FILE, newline='', encoding='utf-8') as csvfile:
        reader = csv.DictReader(csvfile)
        batch = []
        count = 0
        total = 1296675  # from data/README.md
        for row in reader:
            user_key = (row['first'].strip(), row['last'].strip(), row['dob'].strip())
            uid = users[user_key]['id']
            cid = categories[row['category'].strip()]
            amount = float(row['amount'])
            trans_date = row['trans_date']
            trans_time = row['trans_time']
            trans_num = row['trans_num']
            batch.append((trans_num, uid, cid, amount, trans_date, trans_time))
            count += 1
            if count % BATCH_SIZE == 0:
                cursor.executemany('''INSERT IGNORE INTO transaction (trans_num, user_id, category_id, amount, trans_date, trans_time) VALUES (%s,%s,%s,%s,%s,%s)''', batch)
                cnx.commit()
                print(f"Loaded {count} records ({count*100//total}%)")
                batch = []
        if batch:
            cursor.executemany('''INSERT IGNORE INTO transaction (trans_num, user_id, category_id, amount, trans_date, trans_time) VALUES (%s,%s,%s,%s,%s,%s)''', batch)
            cnx.commit()
            print(f"Loaded {count} records (100%)")

    cursor.close()
    cnx.close()
    print("Done.")

if __name__ == '__main__':
    main()
