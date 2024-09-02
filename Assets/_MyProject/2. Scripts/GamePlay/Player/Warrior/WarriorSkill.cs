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
