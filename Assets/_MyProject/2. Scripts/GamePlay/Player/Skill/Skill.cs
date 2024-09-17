using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill
{
    /// <summary>
    /// ��ų�� ���
    /// </summary>
    protected float skillMultiplier;
    /// <summary>
    /// ��ų�� ������
    /// </summary>
    protected int skillDamage;
    /// <summary>
    /// ��� ���� �ջ��Ͽ� ���������� �� ������
    /// </summary>
    protected int finalDamage;
    /// <summary>
    /// ��ų�� Ÿ��
    /// </summary>
    protected int skillHitCount;
    protected Status_Effect Status_Effect;
    protected UserStatClient userStat;
    /// <summary>
    /// ��ų�� ���� �� ���� ������ ��Ŀ����
    /// </summary>
    public virtual void SkillSendDamage(Damagable damagable)
    {
        userStat = UserStatManager.Instance.userStatClient;        
        // override

        // ���� ������ ������ ���⼭ �ֱ�        
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
