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
                StartCoroutine(sword_Stab.Rush());
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
    }    
    public IEnumerator Rush()
    {
        Debug.Log(isPlay);
        yield return new WaitUntil(() => isPlay);
        Debug.Log(isPlay);

        Vector3 position = controller.transform.position;

        while (true)
        {
            yield return null;            
            position.z = Mathf.Lerp(position.z, position.z + .5f, Time.deltaTime);
            controller.Move(position);

            if (isEnd)
            {
                break;
            }            
        }

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
