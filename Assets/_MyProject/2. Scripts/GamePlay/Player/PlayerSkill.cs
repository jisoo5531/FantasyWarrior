using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public Skill_SwordSpear swordSpear;

    public void SKill_Play(int skillNum)
    {
        switch (skillNum)
        {
            case 0:
                
                break;
            default:
                break;
        }
    }
}

[System.Serializable]
public class Skill_SwordSpear
{
    public GameObject effect;
    
}
