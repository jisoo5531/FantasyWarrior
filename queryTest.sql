/*CREATE TABLE User (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_login TIMESTAMP,
    level INT DEFAULT 1,
    exp INT DEFAULT 0,
    gold INT DEFAULT 0
);*/
/*DELETE FROM user WHERE user_id = 6*/
INSERT INTO user (username, email, password_hash)
VALUES ('Player1', 'player1@test.test', '1234');

/*SELECT * FROM user WHERE email='player1@test.test' AND password_hash='1234'*/