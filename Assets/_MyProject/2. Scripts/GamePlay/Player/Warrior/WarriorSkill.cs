using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkill : PlayerSkill
{        
    public Sword_Stab sword_Stab;
    public static event Action OnEndSkill_1;    
    public event Action OnEndSkill_2;    
    public event Action OnEndSkill_3;    
    public event Action OnEndSkill_4;    

    protected override void Initialize()
    {
        base.Initialize();

        skillTable = new Dictionary<int, string>
        {
            { 0, "Skill_1" },
            { 1, "Skill_2" },
            { 2, "Skill_3" },
            { 3, "Skill_4" },
            { 4, "Skill_5" },
            { 5, "Skill_6" },
            { 6, "Skill_7" },
            { 7, "Skill_8" },
            { 8, "Skill_9" },
            { 9, "Skill_10" },
            { 10, "Skill_11" },
            { 11, "Skill_12" },
        };
    }
    protected override void GetSkillFromDatabaseData()
    {
        base.GetSkillFromDatabaseData();
        //whereQuery = new Dictionary<string, object>
        //{

        //}
    }
    public override void SKill_Play(int skillNum)
    {
        switch (skillNum)
        {
            case 0:                
                sword_Stab.Play(GetComponent<CharacterController>());
                break;
            default:
                break;
        }                
    }
    public void SkillEnd(int num)
    {
        OnEndSkill_1?.Invoke();
        Debug.Log("¸ØÃè³ª?");
    }
}
[System.Serializable]
public class Sword_Stab
{
    public GameObject effect;
    public Transform effectPos;
    private Rigidbody rigid;
    CharacterController controller;    
    

    public void Play(CharacterController controller)
    {
        this.controller = controller;
        
        EffectPlay();
    }        
    private void EffectPlay()
    {        
        GameObject skillEffect = MonoBehaviour.Instantiate(effect, effectPos);
        MonoBehaviour.Destroy(skillEffect, 1f);
        
    }
}
