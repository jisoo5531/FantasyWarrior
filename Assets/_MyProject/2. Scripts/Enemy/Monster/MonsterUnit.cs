using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterUnit : Enemy, IAttackable, IDamagable, IMovable
{
    public MonsterStateMachine M_StateMachine;
    public PlayerController player;

    public abstract int Damage { get; set; }
    public virtual int MaxHp { get; set; }
    public virtual int Hp { get; set; }
    public float DistanceToPlayer { get; set; }
    public int MoveSpeed { get; set; }
    public int Range { get; set; }

    [Tooltip("모든 몬스터의 공통된 탐지거리값")]
    public int detectionRange = 10;

    public abstract void SendDamage(int damage);

    public virtual void GetDamage(int damage)
    {
        if (Hp <= 0)
        {
            Death();
        }
    }

    public virtual void Death()
    {        

    }

    public void CalculateDistance()
    {
        DistanceToPlayer = Vector3.Distance(player.transform.position, transform.position);        
    }
}
