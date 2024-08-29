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

    }

    public virtual void SKill_Play(int skillNum)
    {
        
    }    
}
