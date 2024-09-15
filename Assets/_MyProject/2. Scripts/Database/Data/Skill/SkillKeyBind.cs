using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SkillKeyBind
{
    public int User_ID { get; set; }
    public int Skill_1 { get; set; }
    public int Skill_2 { get; set; }
    public int Skill_3 { get; set; }
    public int Skill_4 { get; set; }

    public SkillKeyBind(DataRow row) : this
        (
            int.Parse(row["User_ID"].ToString()),
            int.TryParse(row["Skill_ID_1"].ToString(), out int skill_1) ? skill_1 : 0,
            int.TryParse(row["Skill_ID_2"].ToString(), out int skill_2) ? skill_2 : 0,
            int.TryParse(row["Skill_ID_3"].ToString(), out int skill_3) ? skill_3 : 0,
            int.TryParse(row["Skill_ID_4"].ToString(), out int skill_4) ? skill_4 : 0
        )
    { }

    public SkillKeyBind(int user_ID, int skill_1, int skill_2, int skill_3, int skill_4)
    {
        this.User_ID = user_ID;
        this.Skill_1 = skill_1;
        this.Skill_2 = skill_2;
        this.Skill_3 = skill_3;
        this.Skill_4 = skill_4;
    }
}
