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
        sword_Stab.Initialize(OnEndSkill_1);

        skillTable = new Dictionary<int, string>
        {
            { 0, "Skill_Spear" }
        };        
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

    private bool isPlay = false;
    private bool isEnd = false;

    public void Initialize(Action skillsEnd)
    {
        WarriorSkill.OnEndSkill_1 += () => { isEnd = true; };        
    }

    public void Play(CharacterController controller)
    {
        this.controller = controller;

        isPlay = true;
        EffectPlay();
    }        
    private void EffectPlay()
    {        
        GameObject skillEffect = MonoBehaviour.Instantiate(effect, effectPos);
        MonoBehaviour.Destroy(skillEffect, 1f);

        isPlay = false;
        isEnd = false;
    }
}
