/*CREATE TABLE Users (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(100) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    level INT DEFAULT 1,
    exp INT DEFAULT 0,
    gold INT DEFAULT 0
);*/
INSERT INTO users (username, email, password_hash)
VALUES ('player1', 'player1@test.test', '1234');