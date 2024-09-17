using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill
{
    /// <summary>
    /// 스킬의 계수
    /// </summary>
    protected float skillMultiplier;
    /// <summary>
    /// 스킬의 데미지
    /// </summary>
    protected int skillDamage;
    /// <summary>
    /// 모든 것을 합산하여 최종적으로 들어갈 데미지
    /// </summary>
    protected int finalDamage;
    /// <summary>
    /// 스킬의 타수
    /// </summary>
    protected int skillHitCount;
    protected Status_Effect Status_Effect;
    protected UserStatClient userStat;
    /// <summary>
    /// 스킬이 들어갔을 때 들어가는 데미지 메커니즘
    /// </summary>
    public virtual void SkillSendDamage(Damagable damagable)
    {
        userStat = UserStatManager.Instance.userStatClient;        
        // override

        // 맞은 놈한테 데미지 여기서 주기        
    }
    protected SkillData GetSkillData(int skill_Order)
    {
        List<SkillData> skillDatas = SkillManager.Instance.ClassSkillDataList;
        return skillDatas.Find(x => x.Skill_Order.Equals(skill_Order));
    }
    protected abstract void CalculateSkillDamage();    
    public abstract IEnumerator SkillMechanism(Damagable damagable);
}

[System.Serializable]
public class SkillResource
{

    public List<Effect> skillEffects;
    GameObject effectObj;

    public void OnCollider()
    {
        Debug.Log(effectObj.name);
        effectObj.GetComponent<Collider>().enabled = true;
    }
    public void Play(int skillNum)
    {        
        Effect effect = skillEffects[skillNum];
        effectObj = MonoBehaviour.Instantiate(effect.effect, effect.effectPos);        
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
