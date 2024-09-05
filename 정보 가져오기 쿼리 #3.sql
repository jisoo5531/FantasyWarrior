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

SELECT *
FROM monsters
WHERE NAME='Goblin';