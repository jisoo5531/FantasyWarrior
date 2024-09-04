using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Skill
{    
    public enum Skill_Key
    {
        Skill_1,
        Skill_2,
        SKill_3,
        Skill_4
    }

    public List<Effect> skillEffects;

    public void Play(int skillNum)
    {
        Effect effect = skillEffects[skillNum];
        GameObject effectObj = MonoBehaviour.Instantiate(effect.effect, effect.effectPos);
        MonoBehaviour.Destroy(effectObj, effect.destroyAfter);
    }
}
[System.Serializable]
public class Effect
{
    public GameObject effect;
    public Transform effectPos;
    public float destroyAfter;    
}

#region 스킬 데이터베이스
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
    Stun
}

public class SkillData
{
    public int Skill_ID { get; set; }
    public string Skill_Name { get; set; }
    public int Level { get; set; }    
    public int Damage { get; set; }
    public int Mana_Cost { get; set; }
    public float CoolTime { get; set; }    
    public int Unlock_Level { get; set; }    
    public int Skill_Order { get; set; }
    public string Skill_Desc { get; set; }
    public string Icon_Name { get; set; }

    public SkillData(DataRow row) : this
        (
            int.Parse(row["skill_id"].ToString()),
            row["skill_name"].ToString(),
            int.Parse(row["skill_level"].ToString()),            
            int.Parse(row["damage"].ToString()),
            int.Parse(row["mana_cost"].ToString()),
            float.Parse(row["cooltime"].ToString()),            
            int.Parse(row["unlock_level"].ToString()),            
            int.Parse(row["skill_order"].ToString()),
            row["description"].ToString(),
            row["icon_name"].ToString()
        )
    { }

    public SkillData(int skill_ID, string skill_Name, int level, int damage, int mana_Cost, float coolTime, int unlock_Level, int skill_Order, string skill_Desc, string icon_Name)
    {
        this.Skill_ID = skill_ID;
        this.Skill_Name = skill_Name;
        this.Level = level;        
        this.Damage = damage;
        this.Mana_Cost = mana_Cost;
        this.CoolTime = coolTime;        
        this.Unlock_Level = unlock_Level;        
        this.Skill_Order = skill_Order;
        this.Skill_Desc = skill_Desc;
        this.Icon_Name = icon_Name;
    }
}
#endregion