using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill
{
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