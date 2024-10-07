using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill : MonoBehaviour
{
    public List<BossSkillInfo> bossSkillList;
    private int skillNum = 0;

    private void Awake()
    {
        GetComponent<BossAction>().OnSkillPlay += CurrentSkill;
    }
    private void OnDisable()
    {
        GetComponent<BossAction>().OnSkillPlay -= CurrentSkill;
    }

    private void CurrentSkill(int skillNum)
    {
        this.skillNum = skillNum;
    }

    public void SkillPlay(int num)
    {
        Collider collider = bossSkillList[skillNum].bossSkillEffect[num].effect.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = true;
        }
    }
    public void SkillFinish(int num)
    {
        Collider collider = bossSkillList[skillNum].bossSkillEffect[num].effect.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }
    }
}

[System.Serializable]
public class BossSkillInfo
{
    public List<BossSkillEffect> bossSkillEffect;
}
[System.Serializable]
public class BossSkillEffect
{
    public GameObject effect;
}
