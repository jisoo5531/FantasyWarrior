using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{    
    public Dictionary<int, string> skillTable;

    private void Awake()
    {
        Initialize();
    }
    

    protected virtual void Initialize()
    {
        skillTable = new Dictionary<int, string>
        {
            { 0, "Skill_1" },
            { 1, "Skill_2" },
            { 2, "Skill_3" },
            { 3, "Skill_4" }
        };
    }

    public virtual void SKill_Play(int skillNum)
    {
        
    }    
}
