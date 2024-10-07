using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public enum Skill_Type
{
    Attack,
    Deffense,
    Buff,
    Debuff
}
public enum Status_Effect
{
    None,
    Slow,
    Stun,
    Airbone,
    NuckBack
}

public class SkillData
{
    public int Skill_ID { get; set; }
    public string Skill_Name { get; set; }
    public int MaxLevel { get; set; }
    public int Level { get; set; }
    public float Multiplier { get; set; }
    public float Multi_Amount { get; set; }
    public int HitCount { get; set; }
    public int Mana_Cost { get; set; }
    public float CoolTime { get; set; }
    public int Unlock_Level { get; set; }
    public Status_Effect CC { get; set; }
    public int Skill_Order { get; set; }
    public CharClass CharClass { get; set; }
    public string Skill_Desc { get; set; }
    public string Skill_Info { get; set; }
    public string Icon_Name { get; set; }

    public SkillData(DataRow row) : this
        (
            int.Parse(row["skill_id"].ToString()),
            row["skill_name"].ToString(),
            int.Parse(row["maxlevel"].ToString()),
            int.Parse(row["level"].ToString()),
            float.Parse(row["Multiplier"].ToString()),
            float.Parse(row["Multi_Amount"].ToString()),
            int.Parse(row["HitCount"].ToString()),
            int.Parse(row["mana_cost"].ToString()),
            float.Parse(row["cooltime"].ToString()),
            int.Parse(row["unlock_level"].ToString()),
            (Status_Effect)System.Enum.Parse(typeof(Status_Effect), row["CC"].ToString()),            
            int.Parse(row["skill_order"].ToString()),            
            (CharClass)System.Enum.Parse(typeof(CharClass), row["class"].ToString()),
            row["description"].ToString(),
            row["Skill_Info"].ToString(),
            row["icon_name"].ToString()
        )
    { }

    public SkillData(int skill_ID, string skill_Name, int maxLevel, int level, float multiplier, float multi_Amount, int hitCount, int mana_Cost, float coolTime, int unlock_Level, Status_Effect cC, int skill_Order, CharClass charClass, string skill_Desc, string skill_Info, string icon_Name)
    {
        this.Skill_ID = skill_ID;
        this.Skill_Name = skill_Name;
        this.MaxLevel = maxLevel;
        this.Level = level;
        this.Multiplier = multiplier;
        this.Multi_Amount = multi_Amount;
        this.HitCount = hitCount;
        this.Mana_Cost = mana_Cost;
        this.CoolTime = coolTime;
        this.Unlock_Level = unlock_Level;
        this.CC = cC;
        this.Skill_Order = skill_Order;
        this.CharClass = charClass;
        this.Skill_Desc = skill_Desc;
        this.Skill_Info = skill_Info;
        this.Icon_Name = icon_Name;
    }
}