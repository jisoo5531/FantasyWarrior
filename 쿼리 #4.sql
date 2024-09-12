#INSERT INTO items (item_name)
#VALUES ('반지');

#INSERT INTO inventory (user_id, item_id, quantity)
#VALUES (1, 3, 2);

#INSERT INTO items (Item_Name)
#VALUES ('Wood');

#SELECT items.item_name, items.Item_Type
#FROM items
#WHERE items.Item_ID=1;

#INSERT INTO userstats (user_id)
#VALUES (1);

#SELECT *
#FROM userstats
#WHERE user_id=1

#SELECT skills.Skill_ID, skills.Skill_Name, skills.Level, skills.Damage, skills.Mana_Cost, skills.Cooltime, skills.Unlock_Level, skills.Skill_Order, skills.Class, skills.Description, skills.Icon_Name
#FROM userstats
#JOIN skills ON userstats.`Level` >= skills.Unlock_Level
#WHERE userstats.user_id = 1 AND skills.Class = 1;

INSERT INTO inventory (inventory.User_ID, inventory.Item_ID, inventory.Quantity)
