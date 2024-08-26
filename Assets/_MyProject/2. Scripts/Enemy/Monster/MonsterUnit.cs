using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterUnit : Enemy, IAttackable, IDamagable
{
    public virtual int MaxHp { get; set; }
    public virtual int Hp { get; set; }
    public int Damage { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private MonsterStateMachine m_StateMachine;

    public virtual void Death()
    {
        
    }

    public virtual void GetDamage(int damage)
    {
        
    }

    public void SendDamage(int damage)
    {
        throw new System.NotImplementedException();
    }
}
