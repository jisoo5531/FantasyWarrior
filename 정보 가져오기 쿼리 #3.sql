#SELECT jobs.Job_ID
#FROM userjobs
#JOIN Jobs ON UserJobs.Job_ID = Jobs.Job_ID
#WHERE UserJobs.User_ID = 1;   # 유저 id 가 1의 직업 정보 가져오기

#INSERT INTO jobs (job_name)
#VALUES ("None");

#SELECT skills.Skill_ID, skills.Skill_Name, skills.Level, skills.Damage, skills.Mana_Cost, skills.Cooltime, skills.Unlock_Level, skills.Skill_Order, skills.Class, skills.Description, skills.Icon_Name
#FROM userskills
#JOIN Skills ON UserSkills.Skill_ID = Skills.Skill_ID
#WHERE UserSkills.User_ID = 1 AND UserSkills.Job_ID = 1

#SELECT skills.Skill_ID, skills.Skill_Name, skills.Level, skills.Damage, skills.Mana_Cost, skills.Cooltime, skills.Unlock_Level, skills.Skill_Order, skills.Class, skills.Description, skills.Icon_Name
#FROM users
#JOIN skills ON users.`level` >= skills.Unlock_Level
#WHERE users.user_id = 1 AND skills.Class = 1;

/*SELECT 
    i1.Item_Name AS HeadItem, 
    i2.Item_Name AS ArmorItem, 
    i3.Item_Name AS GlovesItem,
    i4.Item_Name AS BootsItem,
    i5.Item_Name AS WeaponItem,
    i6.Item_Name AS PendantItem,
    i7.Item_Name AS RingItem
FROM PlayerEquipment
LEFT JOIN Items i1 ON PlayerEquipment.HeadItem_ID = i1.Item_ID
LEFT JOIN Items i2 ON PlayerEquipment.ArmorItem_ID = i2.Item_ID
LEFT JOIN Items i3 ON PlayerEquipment.GlovesItem_ID = i3.Item_ID
LEFT JOIN Items i4 ON PlayerEquipment.BootsItem_ID = i4.Item_ID
LEFT JOIN Items i5 ON PlayerEquipment.WeaponItem_ID = i5.Item_ID
LEFT JOIN Items i6 ON PlayerEquipment.PendantItem_ID = i6.Item_ID
LEFT JOIN Items i7 ON PlayerEquipment.RingItem_ID = i7.Item_ID
WHERE PlayerEquipment.User_ID = 1;*/

/*INSERT INTO inventory (inventory.User_ID, inventory.Item_ID)
VALUES (1, 1);
INSERT INTO inventory (inventory.User_ID, inventory.Item_ID)
VALUES (1, 2);
INSERT INTO inventory (inventory.User_ID, inventory.Item_ID)
VALUES (1, 4);
INSERT INTO inventory (inventory.User_ID, inventory.Item_ID)
VALUES (1, 5);
INSERT INTO inventory (inventory.User_ID, inventory.Item_ID)
VALUES (1, 6);
INSERT INTO inventory (inventory.User_ID, inventory.Item_ID)
VALUES (1, 7);
INSERT INTO inventory (inventory.User_ID, inventory.Item_ID)
VALUES (1, 8);*/

INSERT INTO inventory (inventory.User_ID, inventory.Item_ID)
VALUES (1, 1), (1, 2);
