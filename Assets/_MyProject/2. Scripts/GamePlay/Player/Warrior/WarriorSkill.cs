using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkill : PlayerSkill
{

    protected override void Initialize()
    {
        List<SkillData> skillInfoList = SkillManager.Instance.ClassSkillDataList;
        skillList = new List<Skill>
        {
            new W_Skill_1(),
            new W_Skill_2(),
            new W_Skill_3(),
            new W_Skill_4(),
            new W_Skill_5(),
            new W_Skill_6(),
            new W_Skill_7(),
            new W_Skill_8(),
            new W_Skill_9()
        };
        base.Initialize();
    }
    public void Skill_Attack()
    {
        if (!isLocalPlayer) return;

        skillResourceList[currentSkillNum - 1].OnCollider();
    }
    public void Skill_Play(int skillNum)
    {
        if (!isLocalPlayer) return;        

        Debug.Log($"{currentSkillNum}, {skillNum}");
        skillResourceList[currentSkillNum - 1].Play(skillNum);
        // 스킬을 서버에 요청
        CmdTriggerSkill(currentSkillNum - 1, skillNum); // 서버로 스킬 실행 명령 전송
    }
}
public class W_Skill_1 : Skill
{
    public W_Skill_1()
    {
        SkillData skill = GetSkillData(1);
        if (skill != null)
        {
            skillMultiplier = skill.Multiplier + (skill.Level * skill.Multi_Amount);
            skillHitCount = skill.HitCount;
        }
    }

    /// <summary>
    /// 스킬의 메커니즘 (데미지가 어떻게 들어가는지)
    /// </summary>
    public override void SkillSendDamage(Damagable damagable)
    {
        base.SkillSendDamage(damagable);
        // 맞은 놈한테 데미지 여기서 주기        

        SkillManager.Instance.StartSkill(this, damagable);
    }
    protected override void CalculateSkillDamage()
    {
        skillDamage = Mathf.RoundToInt(skillMultiplier * (userStat.ATK + userStat.STR * .5f));
        finalDamage = Random.Range(skillDamage - 100, skillDamage + 100);
    }
    public override IEnumerator SkillMechanism(Damagable damagable)
    {
        for (int i = 0; i < skillHitCount; i++)
        {
            Debug.Log("W_Skill_1 데미지 들어감");
            CalculateSkillDamage();
            damagable.GetDamage(finalDamage);
            yield return new WaitForSeconds(0.05f);
        }
    }
}

public class W_Skill_2 : Skill
{
    public W_Skill_2()
    {
        SkillData skill = GetSkillData(2);
        if (skill != null)
        {
            skillMultiplier = skill.Multiplier + (skill.Level * skill.Multi_Amount);
            skillHitCount = skill.HitCount;
        }
    }

    /// <summary>
    /// 스킬의 메커니즘 (데미지가 어떻게 들어가는지)
    /// </summary>
    public override void SkillSendDamage(Damagable damagable)
    {
        base.SkillSendDamage(damagable);
        // 맞은 놈한테 데미지 여기서 주기        

        SkillManager.Instance.StartSkill(this, damagable);
    }
    protected override void CalculateSkillDamage()
    {
        skillDamage = Mathf.RoundToInt(skillMultiplier * (userStat.ATK + userStat.STR * .5f));
        finalDamage = Random.Range(skillDamage - 100, skillDamage + 100);
    }
    public override IEnumerator SkillMechanism(Damagable damagable)
    {
        for (int i = 0; i < skillHitCount; i++)
        {
            Debug.Log("W_Skill_2 데미지 들어감");
            CalculateSkillDamage();
            damagable.GetDamage(finalDamage);
            yield return new WaitForSeconds(0.05f);
        }
    }
}

public class W_Skill_3 : Skill
{
    public W_Skill_3()
    {
        SkillData skill = GetSkillData(3);
        if (skill != null)
        {
            skillMultiplier = skill.Multiplier + (skill.Level * skill.Multi_Amount);
            skillHitCount = skill.HitCount;
        }
    }

    /// <summary>
    /// 스킬의 메커니즘 (데미지가 어떻게 들어가는지)
    /// </summary>
    public override void SkillSendDamage(Damagable damagable)
    {
        base.SkillSendDamage(damagable);
        // 맞은 놈한테 데미지 여기서 주기        

        SkillManager.Instance.StartSkill(this, damagable);
    }
    protected override void CalculateSkillDamage()
    {
        skillDamage = Mathf.RoundToInt(skillMultiplier * (userStat.ATK + userStat.STR * .5f));
        finalDamage = Random.Range(skillDamage - 100, skillDamage + 100);
    }
    public override IEnumerator SkillMechanism(Damagable damagable)
    {
        for (int i = 0; i < skillHitCount; i++)
        {
            Debug.Log("W_Skill_3 데미지 들어감");
            CalculateSkillDamage();
            damagable.GetDamage(finalDamage);
            yield return new WaitForSeconds(0.05f);
        }
    }
}

public class W_Skill_4 : Skill
{
    public W_Skill_4()
    {
        SkillData skill = GetSkillData(4);
        if (skill != null)
        {
            skillMultiplier = skill.Multiplier + (skill.Level * skill.Multi_Amount);
            skillHitCount = skill.HitCount;
        }
    }

    /// <summary>
    /// 스킬의 메커니즘 (데미지가 어떻게 들어가는지)
    /// </summary>
    public override void SkillSendDamage(Damagable damagable)
    {
        base.SkillSendDamage(damagable);
        // 맞은 놈한테 데미지 여기서 주기        

        SkillManager.Instance.StartSkill(this, damagable);
    }
    protected override void CalculateSkillDamage()
    {
        skillDamage = Mathf.RoundToInt(skillMultiplier * (userStat.ATK + userStat.STR * .5f));
        finalDamage = Random.Range(skillDamage - 100, skillDamage + 100);
    }
    public override IEnumerator SkillMechanism(Damagable damagable)
    {
        for (int i = 0; i < skillHitCount; i++)
        {
            Debug.Log("W_Skill_4 데미지 들어감");
            CalculateSkillDamage();
            damagable.GetDamage(finalDamage);
            yield return new WaitForSeconds(0.05f);
        }
    }
}

public class W_Skill_5 : Skill
{
    public W_Skill_5()
    {
        SkillData skill = GetSkillData(5);
        if (skill != null)
        {
            skillMultiplier = skill.Multiplier + (skill.Level * skill.Multi_Amount);
            skillHitCount = skill.HitCount;
        }
    }

    /// <summary>
    /// 스킬의 메커니즘 (데미지가 어떻게 들어가는지)
    /// </summary>
    public override void SkillSendDamage(Damagable damagable)
    {
        base.SkillSendDamage(damagable);
        // 맞은 놈한테 데미지 여기서 주기        

        SkillManager.Instance.StartSkill(this, damagable);
    }
    protected override void CalculateSkillDamage()
    {
        skillDamage = Mathf.RoundToInt(skillMultiplier * (userStat.ATK + userStat.STR * .5f));
        finalDamage = Random.Range(skillDamage - 100, skillDamage + 100);
    }
    public override IEnumerator SkillMechanism(Damagable damagable)
    {
        for (int i = 0; i < skillHitCount; i++)
        {
            Debug.Log("W_Skill_5 데미지 들어감");
            CalculateSkillDamage();
            damagable.GetDamage(finalDamage);
            yield return new WaitForSeconds(0.05f);
        }
    }
}

public class W_Skill_6 : Skill
{
    public W_Skill_6()
    {
        SkillData skill = GetSkillData(6);
        if (skill != null)
        {
            skillMultiplier = skill.Multiplier + (skill.Level * skill.Multi_Amount);
            skillHitCount = skill.HitCount;
        }
    }

    /// <summary>
    /// 스킬의 메커니즘 (데미지가 어떻게 들어가는지)
    /// </summary>
    public override void SkillSendDamage(Damagable damagable)
    {
        base.SkillSendDamage(damagable);
        // 맞은 놈한테 데미지 여기서 주기        

        SkillManager.Instance.StartSkill(this, damagable);
    }
    protected override void CalculateSkillDamage()
    {
        skillDamage = Mathf.RoundToInt(skillMultiplier * (userStat.ATK + userStat.STR * .5f));
        finalDamage = Random.Range(skillDamage - 100, skillDamage + 100);
    }
    public override IEnumerator SkillMechanism(Damagable damagable)
    {
        for (int i = 0; i < skillHitCount; i++)
        {
            Debug.Log("W_Skill_6 데미지 들어감");
            CalculateSkillDamage();
            damagable.GetDamage(finalDamage);
            yield return new WaitForSeconds(0.05f);
        }
    }
}

public class W_Skill_7 : Skill
{
    public W_Skill_7()
    {
        SkillData skill = GetSkillData(7);
        if (skill != null)
        {
            skillMultiplier = skill.Multiplier + (skill.Level * skill.Multi_Amount);
            skillHitCount = skill.HitCount;
        }
    }

    /// <summary>
    /// 스킬의 메커니즘 (데미지가 어떻게 들어가는지)
    /// </summary>
    public override void SkillSendDamage(Damagable damagable)
    {
        base.SkillSendDamage(damagable);
        // 맞은 놈한테 데미지 여기서 주기        

        SkillManager.Instance.StartSkill(this, damagable);
    }
    protected override void CalculateSkillDamage()
    {
        skillDamage = Mathf.RoundToInt(skillMultiplier * (userStat.ATK + userStat.STR * .5f));
        finalDamage = Random.Range(skillDamage - 100, skillDamage + 100);
    }
    public override IEnumerator SkillMechanism(Damagable damagable)
    {
        for (int i = 0; i < skillHitCount; i++)
        {
            Debug.Log("W_Skill_7 데미지 들어감");
            CalculateSkillDamage();
            damagable.GetDamage(finalDamage);
            yield return new WaitForSeconds(0.05f);
        }
    }
}

public class W_Skill_8 : Skill
{
    private float stunDuration = 2.0f;
    public W_Skill_8()
    {
        SkillData skill = GetSkillData(8);
        if (skill != null)
        {
            skillMultiplier = skill.Multiplier + (skill.Level * skill.Multi_Amount);
            skillHitCount = skill.HitCount;
            Status_Effect = skill.CC;
        }
    }

    /// <summary>
    /// 스킬의 메커니즘 (데미지가 어떻게 들어가는지)
    /// </summary>
    public override void SkillSendDamage(Damagable damagable)
    {
        base.SkillSendDamage(damagable);
        // 맞은 놈한테 데미지 여기서 주기        

        SkillManager.Instance.StartSkill(this, damagable);
    }
    protected override void CalculateSkillDamage()
    {
        skillDamage = Mathf.RoundToInt(skillMultiplier * (userStat.ATK + userStat.STR * .5f));
        finalDamage = Random.Range(skillDamage - 100, skillDamage + 100);
    }
    public override IEnumerator SkillMechanism(Damagable damagable)
    {
        IStatusEffect stun = new StunEffect(stunDuration);
        damagable.ApplyStatusEffect(stun);
        for (int i = 0; i < skillHitCount; i++)
        {
            Debug.Log("W_Skill_8 데미지 들어감");
            CalculateSkillDamage();
            damagable.GetDamage(finalDamage);
            yield return new WaitForSeconds(0.05f);
        }
    }
}

public class W_Skill_9 : Skill
{
    public W_Skill_9()
    {
        SkillData skill = GetSkillData(9);
        if (skill != null)
        {
            skillMultiplier = skill.Multiplier + (skill.Level * skill.Multi_Amount);
            skillHitCount = skill.HitCount;
        }
    }

    /// <summary>
    /// 스킬의 메커니즘 (데미지가 어떻게 들어가는지)
    /// </summary>
    public override void SkillSendDamage(Damagable damagable)
    {
        base.SkillSendDamage(damagable);
        // 맞은 놈한테 데미지 여기서 주기        

        SkillManager.Instance.StartSkill(this, damagable);
    }
    protected override void CalculateSkillDamage()
    {
        skillDamage = Mathf.RoundToInt(skillMultiplier * (userStat.ATK + userStat.STR * .5f));
        finalDamage = Random.Range(skillDamage - 100, skillDamage + 100);
    }
    public override IEnumerator SkillMechanism(Damagable damagable)
    {
        for (int i = 0; i < skillHitCount; i++)
        {
            Debug.Log("W_Skill_9 데미지 들어감");
            CalculateSkillDamage();
            damagable.GetDamage(finalDamage);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
