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
    public void SKill_Play(int skillNum)
    {
        switch (currentSkillNum)
        {
            case 0:
                skillList[0].Play(skillNum);
                break;
            case 1:
                skillList[1].Play(skillNum);
                break;
            default:
                break;
        }                
    }
}