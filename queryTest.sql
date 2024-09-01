INSERT INTO Monsters (name, maxhp, hp, damage, defense, move_speed, attack_range, experience_reward, gold_reward) 
VALUES ('Goblin', 100, 100, 15, 5, 1.5, 2, 20, 10);
#INSERT INTO Monsters (name, maxhp, hp, damage, defense, move_speed, attack_range, experience_reward, gold_reward) 
#VALUES ('Mummy', 100, 100, 8, 0, 1, 0.7, 7, 5);
UPDATE monsters SET maxhp=200, hp=200 WHERE NAME='Goblin'