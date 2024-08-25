using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Enemy, IDamagable
{
    public virtual int MaxHp { get; set; }
    public virtual int Hp { get; set; }

    public virtual void Death()
    {
        
    }

    public virtual void GetDamage(int damage)
    {
        
    }
}
