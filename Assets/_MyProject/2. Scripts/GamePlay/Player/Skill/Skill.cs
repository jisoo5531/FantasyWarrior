using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class Skill
{
    public int skillDamage;
    public Damagable damagable;
    /// <summary>
    /// 스킬이 들어갔을 때 들어가는 데미지 메커니즘
    /// </summary>
    public virtual void SkillSendDamage(Damagable damagable)
    {
        this.damagable = damagable;
        // override

        // 맞은 놈한테 데미지 여기서 주기        
    }
    public virtual IEnumerator SkillMechanism()
    {
        yield return null;
    }
}

[System.Serializable]
public class SkillResource
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
