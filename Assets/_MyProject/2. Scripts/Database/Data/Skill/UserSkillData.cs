using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class UserSkillData
{
    public int UserSkill_ID { get; set; }
    public int User_ID { get; set; }
    public CharClass Class { get; set; }
    public int Skill_ID { get; set; }
    public int Skill_Level { get; set; }    

    public UserSkillData(DataRow row) : this
        (
            int.Parse(row["UserSkill_ID"].ToString()),
            int.Parse(row["User_ID"].ToString()),
            (CharClass)int.Parse(row["Job_ID"].ToString()),
            int.Parse(row["Skill_ID"].ToString()),
            int.Parse(row["Skill_Level"].ToString())            
        )
    { }    
    public UserSkillData(int userSkill_ID, int user_ID, CharClass @class, int skill_ID, int skill_Level)
    {
        this.UserSkill_ID = userSkill_ID;
        this.User_ID = user_ID;
        this.Class = @class;
        this.Skill_ID = skill_ID;
        this.Skill_Level = skill_Level;
    }
    public UserSkillData(int user_ID, CharClass @class, int skill_ID)
    {
        this.User_ID = user_ID;
        this.Class = @class;
        this.Skill_ID = skill_ID;
    }
}
