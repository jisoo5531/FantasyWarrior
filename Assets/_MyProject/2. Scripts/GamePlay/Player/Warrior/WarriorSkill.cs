using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkill : PlayerSkill
{
    public List<Skill> skillList;
    

    protected override void Initialize()
    {
        base.Initialize();
    }
    protected override void GetSkillFromDatabaseData()
    {
        base.GetSkillFromDatabaseData();
        //whereQuery = new Dictionary<string, object>
        //{

        //}
    }
    public void Skill_Play(int skillNum)
    {        
        skillList[currentSkillNum].Play(skillNum);
    }
}