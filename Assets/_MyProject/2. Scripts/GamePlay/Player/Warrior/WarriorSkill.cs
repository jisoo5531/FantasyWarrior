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
    /// ��ų�� ��Ŀ���� (�������� ��� ������)
    /// </summary>
    public override void SkillSendDamage(Damagable damagable)
    {
        base.SkillSendDamage(damagable);
        // ���� ������ ������ ���⼭ �ֱ�
        SkillManager.Instance.StartSkill(this);
    }
    public override IEnumerator SkillMechanism()
    {
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("������ ��");
            damagable.GetDamage(skillDamage);
            yield return new WaitForSeconds(0.5f);
        }        
    }
}