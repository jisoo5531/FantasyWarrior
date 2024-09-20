/*CREATE TABLE users(
	User_ID INT PRIMARY KEY AUTO_INCREMENT,
	UserName VARCHAR(255) NOT NULL,
	Email VARCHAR(255) NOT NULL,
	Password_Hash VARCHAR(255) NOT NULL
);
CREATE TABLE Jobs (
    Job_ID INT PRIMARY KEY AUTO_INCREMENT,
    JobName VARCHAR(255) NOT NULL,
    JobDescription TEXT
);
CREATE TABLE Skills (
    Skill_ID INT PRIMARY KEY AUTO_INCREMENT,
    Skill_Name VARCHAR(255) NOT NULL,
    MaxLevel INT DEFAULT 1,
    Damage INT	DEFAULT 0,
    Unlock_Level INT DEFAULT 0,
    Skill_Order INT DEFAULT 0,
    Mana_Cost INT DEFAULT 0,
    
    Skill_Description TEXT
);
CREATE TABLE JobSkills (
    JobSkill_ID INT PRIMARY KEY AUTO_INCREMENT,
    Job_ID INT,
    Skill_ID INT,
    FOREIGN KEY (Job_ID) REFERENCES Jobs(Job_ID),
    FOREIGN KEY (Skill_ID) REFERENCES Skills(Skill_ID)
);
CREATE TABLE UserJobs (
    UserJob_ID INT PRIMARY KEY AUTO_INCREMENT,
    User_ID INT,
    Job_ID INT,
    Job_Level INT DEFAULT 1,
    Job_Experience INT DEFAULT 0,
    FOREIGN KEY (User_ID) REFERENCES Users(User_ID),
    FOREIGN KEY (Job_ID) REFERENCES Jobs(Job_ID)
);
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
CREATE TABLE Items (
    Item_ID INT PRIMARY KEY AUTO_INCREMENT,
    ItemName VARCHAR(255) NOT NULL,
    ItemType VARCHAR(100),
    ItemDescription TEXT,
    Rarity INT
);
CREATE TABLE Inventory (
    InventoryID INT PRIMARY KEY AUTO_INCREMENT,
    User_ID INT,
    Item_ID INT,
    Quantity INT DEFAULT 1,
    FOREIGN KEY (User_ID) REFERENCES Users(User_ID),
    FOREIGN KEY (Item_ID) REFERENCES Items(Item_ID)
);
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
CREATE TABLE EquipmentItems (
    Equipment_ID INT PRIMARY KEY AUTO_INCREMENT, -- 장비 고유 ID
    Item_ID INT,                                 -- Items 테이블과의 외래 키
    Equipment_Type VARCHAR(50),                  -- 장비 유형 (예: Armor, Weapon, Accessory)
    Required_Level INT DEFAULT 1,                -- 장비 착용에 필요한 레벨
    Attack_Power INT DEFAULT 0,                  -- 무기의 공격력
    Defense_Power INT DEFAULT 0,                 -- 방어구의 방어력
    STR_Boost INT DEFAULT 0,                -- 힘을 증가시키는 값
    DEX_Boost INT DEFAULT 0,                 -- 민첩성을 증가시키는 값
    INT_Boost INT DEFAULT 0,            -- 지능을 증가시키는 값
    LUK_Boost INT DEFAULT 0,            -- 운을 증가시키는 값
    Hp_Boost INT DEFAULT 0,                  -- 체력 증가
    Mana_Boost INT DEFAULT 0,                    -- 마나 증가
    FOREIGN KEY (Item_ID) REFERENCES Items(Item_ID)
);
CREATE TABLE ConsumItems (
    Consum_ID INT PRIMARY KEY AUTO_INCREMENT, -- 소모품 고유 ID
    Item_ID INT,                                  -- Items 테이블과의 외래 키
    Effect VARCHAR(255),                         -- 소모품 효과 (예: Health Recovery, Mana Recovery)
    Recovery_Amount INT DEFAULT 0,                -- 회복량 (체력 또는 마나)
    Duration INT DEFAULT 0,                      -- 효과 지속 시간 (버프 아이템인 경우)
    FOREIGN KEY (Item_ID) REFERENCES Items(Item_ID)
);
CREATE TABLE OtherItems (
    Others_ID INT PRIMARY KEY AUTO_INCREMENT, -- 기타 아이템 고유 ID
    Item_ID INT,                                     -- Items 테이블과의 외래 키
    Description TEXT,                          -- 아이템 용도 설명
    FOREIGN KEY (Item_ID) REFERENCES Items(Item_ID)
);
CREATE TABLE PlayerEquipment (
    User_ID INT,                            -- 유저 고유 ID (Users 테이블의 외래 키)
    HeadItem_ID INT DEFAULT NULL,           -- 투구 (Helmet)
    ArmorItem_ID INT DEFAULT NULL,          -- 갑옷 (Armor)
    GlovesItem_ID INT DEFAULT NULL,         -- 장갑 (Gloves)
    BootsItem_ID INT DEFAULT NULL,          -- 신발 (Boots)
    WeaponItem_ID INT DEFAULT NULL,         -- 무기 (Weapon)
    PendantItem_ID INT DEFAULT NULL,        -- 펜던트 (Pendant)
    RingItem_ID INT DEFAULT NULL,           -- 반지 (Ring)
    FOREIGN KEY (User_ID) REFERENCES Users(User_ID),  -- Users 테이블과 연관
    FOREIGN KEY (HeadItem_ID) REFERENCES Items(Item_ID),    -- Items 테이블의 외래 키
    FOREIGN KEY (ArmorItem_ID) REFERENCES Items(Item_ID),
    FOREIGN KEY (GlovesItem_ID) REFERENCES Items(Item_ID),
    FOREIGN KEY (BootsItem_ID) REFERENCES Items(Item_ID),
    FOREIGN KEY (WeaponItem_ID) REFERENCES Items(Item_ID),
    FOREIGN KEY (PendantItem_ID) REFERENCES Items(Item_ID),
    FOREIGN KEY (RingItem_ID) REFERENCES Items(Item_ID)
);
CREATE TABLE Quests (
    Quest_ID INT PRIMARY KEY AUTO_INCREMENT,  -- 퀘스트 고유 ID
    Ques_tName VARCHAR(100),                  -- 퀘스트 이름
    Quest_Type VARCHAR(50),                   -- 퀘스트 유형 (예: Main, Side, Daily)
    Description TEXT,                        -- 퀘스트 설명
    ReqLevel INT DEFAULT 1,             -- 퀘스트 수락에 필요한 최소 레벨
    Reward_Exp INT DEFAULT 0,          -- 보상 경험치
    Reward_Gold INT DEFAULT 0,                -- 보상 금액
    RewardItem_ID INT DEFAULT NULL,           -- 보상 아이템 ID (Items 테이블과 연관)
    IsRepeatable BOOLEAN DEFAULT FALSE,      -- 반복 가능한 퀘스트인지 여부
    FOREIGN KEY (RewardItem_ID) REFERENCES Items(Item_ID) -- 보상 아이템과 연결
);*/
/*CREATE TABLE Monsters (
    Monster_ID INT PRIMARY KEY AUTO_INCREMENT,  -- 몬스터 고유 ID
    MonsterName VARCHAR(100) NOT NULL,         -- 몬스터 이름
    MaxHp INT NOT NULL,                        -- 최대 체력
    Hp INT NOT NULL,                           -- 현재 체력 (게임에서 동적으로 관리, DB에 저장할 필요가 없음)
    Damage INT NOT NULL,                       -- 공격력
    Defense INT NOT NULL,                      -- 방어력
    MoveSpeed FLOAT NOT NULL,                  -- 이동 속도
    AttackRange FLOAT NOT NULL,                -- 공격 범위
    EXP_Reward INT NOT NULL,                   -- 처치 시 경험치 보상
    Gold_Reward INT NOT NULL                   -- 처치 시 골드 보상
);

CREATE TABLE QuestObjectives (
    Objective_ID INT PRIMARY KEY AUTO_INCREMENT,
    Quest_ID INT,                                   -- 퀘스트와 연결
    Objective_Type VARCHAR(50),                     -- 목표 유형 (Kill, Collect, Visit 등)
    Monster_ID INT DEFAULT NULL,                    -- 몬스터와 연결
    Item_ID INT DEFAULT NULL,                       -- 아이템과 연결
    Location_ID INT DEFAULT NULL,                   -- 장소와 연결
    Required_Amount INT DEFAULT 1,                  -- 목표 달성을 위해 필요한 수량
    Description TEXT,                              -- 목표 설명
    FOREIGN KEY (Quest_ID) REFERENCES Quests(Quest_ID),
    FOREIGN KEY (Monster_ID) REFERENCES Monsters(Monster_ID),
    FOREIGN KEY (Item_ID) REFERENCES Items(Item_ID)
);

CREATE TABLE UserQuests (
    User_ID INT,                                 -- 유저 ID
    Quest_ID INT,                                -- 퀘스트 ID
    Status VARCHAR(50),                         -- 퀘스트 상태 (Accepted, In Progress, Completed)
    FOREIGN KEY (User_ID) REFERENCES Users(User_ID),
    FOREIGN KEY (Quest_ID) REFERENCES Quests(Quest_ID),
    PRIMARY KEY (User_ID, Quest_ID)
);
CREATE TABLE UserQuestObjectives (
    User_ID INT,                                 -- 유저 ID
    Objective_ID INT,                            -- 목표 ID
    Current_Amount INT DEFAULT 0,                -- 현재 달성된 수량
    IsCompleted BOOLEAN DEFAULT FALSE,          -- 목표 완료 여부
    FOREIGN KEY (User_ID) REFERENCES Users(User_ID),
    FOREIGN KEY (Objective_ID) REFERENCES QuestObjectives(Objective_ID),
    PRIMARY KEY (User_ID, Objective_ID)
);*/
/*CREATE TABLE SkillKeyBind(
	User_ID INT,
	Skill_1 INT DEFAULT 0,
	Skill_2 INT DEFAULT 0,
	Skill_3 INT DEFAULT 0,
	Skill_4 INT DEFAULT 0,
	FOREIGN KEY (User_ID) REFERENCES users(User_ID),
	FOREIGN KEY (Skill_1) REFERENCES skills(Skill_ID),
	FOREIGN KEY (Skill_2) REFERENCES skills(Skill_ID),
	FOREIGN KEY (Skill_3) REFERENCES skills(Skill_ID),
	FOREIGN KEY (Skill_4) REFERENCES skills(Skill_ID)
);*/
CREATE TABLE NPCs (
    NPC_ID INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(100) NOT NULL,
    Location VARCHAR(100),
    Description TEXT
);
CREATE TABLE NPCDialogue (
    Dialogue_ID INT PRIMARY KEY AUTO_INCREMENT,
    NPC_ID INT,
    Text TEXT NOT NULL,
    NEXT_DIALOGUE_ID INT, -- 다음 대화 ID (선택적)
    FOREIGN KEY (NPC_ID) REFERENCES NPCs(NPC_ID)
);
CREATE TABLE NPC_Quests (
    NPC_ID INT,
    Quest_ID INT,
    PRIMARY KEY (NPC_ID, Quest_ID),
    FOREIGN KEY (NPC_ID) REFERENCES NPCs(NPC_ID),
    FOREIGN KEY (Quest_ID) REFERENCES Quests(Quest_ID)
);
CREATE TABLE NPC_TalkQuests(
	NPC_ID INT,
	Quest_ID INT,
	PRIMARY KEY (NPC_ID, Quest_ID),
	FOREIGN KEY (NPC_ID) REFERENCES npcs(NPC_ID),
	FOREIGN KEY (Quest_ID) REFERENCES quests(Quest_ID)
);


