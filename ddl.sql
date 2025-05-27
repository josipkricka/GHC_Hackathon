-- DDL for genai database schema based on entities.md

CREATE DATABASE IF NOT EXISTS genai;
USE genai;

-- User table
CREATE TABLE IF NOT EXISTS user (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    first VARCHAR(64) NOT NULL,
    last VARCHAR(64) NOT NULL,
    gender CHAR(1),
    dob DATE,
    city VARCHAR(128),
    creation_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    last_login_time DATETIME
);
CREATE INDEX idx_user_name ON user(first, last);

-- Job table
CREATE TABLE IF NOT EXISTS job (
    job_id INT AUTO_INCREMENT PRIMARY KEY,
    job_name VARCHAR(128) UNIQUE NOT NULL
);

-- Category table
CREATE TABLE IF NOT EXISTS category (
    category_id INT AUTO_INCREMENT PRIMARY KEY,
    category_name VARCHAR(128) UNIQUE NOT NULL
);

-- Transaction table
CREATE TABLE IF NOT EXISTS transaction (
    trans_num CHAR(32) PRIMARY KEY,
    amount DECIMAL(12,2) NOT NULL,
    trans_date DATE NOT NULL,
    trans_time TIME NOT NULL,
    category_id INT,
    user_id INT,
    job_id INT,
    FOREIGN KEY (category_id) REFERENCES category(category_id),
    FOREIGN KEY (user_id) REFERENCES user(user_id),
    FOREIGN KEY (job_id) REFERENCES job(job_id)
);
CREATE INDEX idx_transaction_user ON transaction(user_id);
CREATE INDEX idx_transaction_category ON transaction(category_id);
CREATE INDEX idx_transaction_job ON transaction(job_id);
