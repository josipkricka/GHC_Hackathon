import csv
import mysql.connector
from mysql.connector import errorcode
import os

DB_CONFIG = {
    'user': 'root',
    'password': 'root',
    'host': '127.0.0.1',
    'database': 'genai',
    'autocommit': False
}

DDL_FILE = 'ddl.sql'
CSV_FILE = os.path.join('data', 'italian_credit_card_transactions.csv')
BATCH_SIZE = 1000

def run_ddl(cursor):
    with open(DDL_FILE, 'r') as f:
        ddl = f.read()
    for stmt in ddl.split(';'):
        stmt = stmt.strip()
        if stmt:
            cursor.execute(stmt)

def main():
    conn = mysql.connector.connect(user=DB_CONFIG['user'], password=DB_CONFIG['password'], host=DB_CONFIG['host'])
    cursor = conn.cursor()
    # Create schema and tables if not exist
    run_ddl(cursor)
    conn.database = DB_CONFIG['database']

    # Prepare caches
    job_cache = {}
    category_cache = {}
    user_cache = {}

    # Preload existing jobs and categories
    cursor.execute('SELECT job_id, job_name FROM job')
    for job_id, job_name in cursor.fetchall():
        job_cache[job_name] = job_id
    cursor.execute('SELECT category_id, category_name FROM category')
    for cat_id, cat_name in cursor.fetchall():
        category_cache[cat_name] = cat_id
    cursor.execute('SELECT user_id, first, last, dob FROM user')
    for user_id, first, last, dob in cursor.fetchall():
        user_cache[(first, last, str(dob))] = user_id

    with open(CSV_FILE, newline='', encoding='utf-8') as csvfile:
        reader = csv.DictReader(csvfile)
        jobs_to_insert = []
        cats_to_insert = []
        users_to_insert = []
        trans_to_insert = []
        total = sum(1 for _ in open(CSV_FILE, encoding='utf-8')) - 1
        csvfile.seek(0)
        next(reader)  # skip header
        count = 0
        for row in reader:
            # Job
            job_name = row['job']
            if job_name not in job_cache:
                jobs_to_insert.append((job_name,))
                job_cache[job_name] = None
            # Category
            cat_name = row['category']
            if cat_name not in category_cache:
                cats_to_insert.append((cat_name,))
                category_cache[cat_name] = None
            # User
            user_key = (row['first'], row['last'], row['dob'])
            if user_key not in user_cache:
                users_to_insert.append((row['first'], row['last'], row['gender'], row['dob'], row['city']))
                user_cache[user_key] = None
            count += 1
            if count % BATCH_SIZE == 0 or count == total:
                # Insert jobs
                if jobs_to_insert:
                    cursor.executemany('INSERT IGNORE INTO job (job_name) VALUES (%s)', jobs_to_insert)
                    conn.commit()
                    cursor.execute('SELECT job_id, job_name FROM job WHERE job_name IN (%s)' % ','.join(['%s']*len(jobs_to_insert)), [j[0] for j in jobs_to_insert])
                    for job_id, job_name in cursor.fetchall():
                        job_cache[job_name] = job_id
                    jobs_to_insert.clear()
                # Insert categories
                if cats_to_insert:
                    cursor.executemany('INSERT IGNORE INTO category (category_name) VALUES (%s)', cats_to_insert)
                    conn.commit()
                    cursor.execute('SELECT category_id, category_name FROM category WHERE category_name IN (%s)' % ','.join(['%s']*len(cats_to_insert)), [c[0] for c in cats_to_insert])
                    for cat_id, cat_name in cursor.fetchall():
                        category_cache[cat_name] = cat_id
                    cats_to_insert.clear()
                # Insert users
                if users_to_insert:
                    cursor.executemany('INSERT INTO user (first, last, gender, dob, city) VALUES (%s, %s, %s, %s, %s)', users_to_insert)
                    conn.commit()
                    cursor.execute('SELECT user_id, first, last, dob FROM user WHERE (first, last, dob) IN (%s)' % ','.join(['(%s,%s,%s)']*len(users_to_insert)), [item for u in users_to_insert for item in (u[0], u[1], u[3])])
                    for user_id, first, last, dob in cursor.fetchall():
                        user_cache[(first, last, str(dob))] = user_id
                    users_to_insert.clear()
                print(f"{int(count/total*100)}% complete ({count}/{total})")
        # Second pass for transactions
        csvfile.seek(0)
        next(reader)
        count = 0
        for row in reader:
            job_id = job_cache[row['job']]
            cat_id = category_cache[row['category']]
            user_id = user_cache[(row['first'], row['last'], row['dob'])]
            trans_to_insert.append((row['trans_num'], row['amount'], row['trans_date'], row['trans_time'], cat_id, user_id, job_id))
            count += 1
            if count % BATCH_SIZE == 0 or count == total:
                cursor.executemany('INSERT INTO transaction (trans_num, amount, trans_date, trans_time, category_id, user_id, job_id) VALUES (%s, %s, %s, %s, %s, %s, %s)', trans_to_insert)
                conn.commit()
                trans_to_insert.clear()
                print(f"{int(count/total*100)}% complete ({count}/{total})")
    cursor.close()
    conn.close()

if __name__ == '__main__':
    main()
