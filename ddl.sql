-- DDL for genai database
CREATE DATABASE IF NOT EXISTS genai;
USE genai;

-- Table: job
CREATE TABLE IF NOT EXISTS job (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE
);

-- Table: category
CREATE TABLE IF NOT EXISTS category (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE
);

-- Table: user
CREATE TABLE IF NOT EXISTS user (
    id INT AUTO_INCREMENT PRIMARY KEY,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    dob DATE NOT NULL,
    gender CHAR(1) NOT NULL,
    city VARCHAR(100),
    job_id INT,
    creation_time DATETIME,
    last_login_time DATETIME,
    login_name VARCHAR(200) NOT NULL UNIQUE,
    password CHAR(64) NOT NULL,
    FOREIGN KEY (job_id) REFERENCES job(id)
);

-- Table: transaction
CREATE TABLE IF NOT EXISTS transaction (
    id INT AUTO_INCREMENT PRIMARY KEY,
    trans_num VARCHAR(100) NOT NULL UNIQUE,
    user_id INT NOT NULL,
    category_id INT NOT NULL,
    amount DECIMAL(10,2) NOT NULL,
    trans_date DATE NOT NULL,
    trans_time TIME NOT NULL,
    FOREIGN KEY (user_id) REFERENCES user(id),
    FOREIGN KEY (category_id) REFERENCES category(id)
);

-- Indexes for performance
CREATE INDEX idx_user_job_id ON user(job_id);
CREATE INDEX idx_transaction_user_id ON transaction(user_id);
CREATE INDEX idx_transaction_category_id ON transaction(category_id);
