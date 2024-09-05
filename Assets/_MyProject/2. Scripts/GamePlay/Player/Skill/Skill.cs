using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Skill
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
