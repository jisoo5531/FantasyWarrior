/*CREATE TABLE Jobs (
    JobID INT PRIMARY KEY AUTO_INCREMENT,
    JobName VARCHAR(255) NOT NULL,
    JobDescription TEXT
);*/
/*CREATE TABLE Skills (
    Skill_ID INT PRIMARY KEY AUTO_INCREMENT,
    Skill_Name VARCHAR(255) NOT NULL,
    MaxLevel INT DEFAULT 1,
    Damage INT	DEFAULT 0,
    Unlock_Level INT DEFAULT 0,
    Skill_Order INT DEFAULT 0,
    Mana_Cost INT DEFAULT 0,
    
    Skill_Description TEXT
);*/
/*CREATE TABLE JobSkills (
    JobSkill_ID INT PRIMARY KEY AUTO_INCREMENT,
    Job_ID INT,
    Skill_ID INT,
    FOREIGN KEY (Job_ID) REFERENCES Jobs(Job_ID),
    FOREIGN KEY (Skill_ID) REFERENCES Skills(Skill_ID)
);*/
/*CREATE TABLE UserJobs (
    UserJob_ID INT PRIMARY KEY AUTO_INCREMENT,
    User_ID INT,
    Job_ID INT,
    Job_Level INT DEFAULT 1,
    Job_Experience INT DEFAULT 0,
    FOREIGN KEY (User_ID) REFERENCES Users(User_ID),
    FOREIGN KEY (Job_ID) REFERENCES Jobs(Job_ID)
);*/
/*CREATE TABLE UserSkills (
    UserSkill_ID INT PRIMARY KEY AUTO_INCREMENT,
    User_ID INT,
    Job_ID INT,
    Skill_ID INT,
    Skill_Level INT DEFAULT 1,
    Skill_Experience INT DEFAULT 0,
    FOREIGN KEY (User_ID) REFERENCES Users(User_ID),
    FOREIGN KEY (Job_ID) REFERENCES Jobs(Job_ID),
    FOREIGN KEY (Skill_ID) REFERENCES Skills(Skill_ID)
);*/
/*CREATE TABLE Inventory (
    InventoryID INT PRIMARY KEY AUTO_INCREMENT,
    User_ID INT,
    Item_ID INT,
    Quantity INT DEFAULT 1,
    FOREIGN KEY (User_ID) REFERENCES Users(User_ID),
    FOREIGN KEY (Item_ID) REFERENCES Items(Item_ID)
);*/
/*CREATE TABLE Items (
    ItemID INT PRIMARY KEY AUTO_INCREMENT,
    ItemName VARCHAR(255) NOT NULL,
    ItemType VARCHAR(100),
    ItemDescription TEXT,
    Rarity INT
);*/
CREATE TABLE UserStats (
    User_ID INT PRIMARY KEY,                -- 유저 고유 ID (Users 테이블의 외래 키)
    Level INT DEFAULT 1,                   -- 유저 레벨
    Exp INT DEFAULT 0,              -- 유저 경험치
    MaxHp INT DEFAULT 100,             -- 최대 체력
    Hp INT DEFAULT 100,         -- 현재 체력
    MaxMana INT DEFAULT 50,                -- 최대 마나
    Mana INT DEFAULT 50,            -- 현재 마나
    STR INT DEFAULT 10,               -- 힘 (Physical Power)
    DEX INT DEFAULT 10,                -- 민첩성 (Dexterity)
    Intelligence INT DEFAULT 10,           -- 지능 (Magical Power)
    Defense INT DEFAULT 5,                 -- 방어력
    Luck INT DEFAULT 5,                    -- 행운
    Gold INT DEFAULT 0,                    -- 유저가 보유한 골드
    FOREIGN KEY (User_ID) REFERENCES Users(User_ID)  -- Users 테이블과 연관
);

