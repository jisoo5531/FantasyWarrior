using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkill : PlayerSkill
{    

    protected override void Initialize()
    {
        base.Initialize();
        skillList = new List<Skill>
        {
            new W_Skill_1()
        };
    }
    public void Skill_Play(int skillNum)
    {                
        skillResourceList[currentSkillNum - 1].Play(skillNum);
    }
}
public class W_Skill_1 : Skill
{
    /// <summary>
    /// 스킬의 메커니즘 (데미지가 어떻게 들어가는지)
    /// </summary>
    public override void SkillSendDamage(Damagable damagable)
    {
        base.SkillSendDamage(damagable);
        // 맞은 놈한테 데미지 여기서 주기
        SkillManager.Instance.StartSkill(this);
    }
    public override IEnumerator SkillMechanism()
    {
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("데미지 들어감");
            damagable.GetDamage(skillDamage);
            yield return new WaitForSeconds(0.5f);
        }        
    }
}