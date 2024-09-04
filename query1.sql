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
CREATE TABLE UserSkills (
    UserSkill_ID INT PRIMARY KEY AUTO_INCREMENT,
    User_ID INT,
    Job_ID INT,
    Skill_ID INT,
    Skill_Level INT DEFAULT 1,
    Skill_Experience INT DEFAULT 0,
    FOREIGN KEY (User_ID) REFERENCES Users(User_ID),
    FOREIGN KEY (Job_ID) REFERENCES Jobs(Job_ID),
    FOREIGN KEY (Skill_ID) REFERENCES Skills(Skill_ID)
);
