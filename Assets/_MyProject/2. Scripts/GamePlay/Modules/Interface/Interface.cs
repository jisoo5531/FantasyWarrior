using System;
using UnityEngine;
public interface IState
{
    void Enter();
    void Excute();
    void Exit();
}
public interface IAttackable
{
    int Damage { get; set; }
    int Range { get; set; }

    void SendDamage(int damage);
}

public delegate void DeathEventHandler();
public delegate void HpChangeEventHandler(int damage);

public interface IDamagable
{
    event DeathEventHandler OnDeathEvent;
    event HpChangeEventHandler OnHpChangeEvent;

    int MaxHp { get; set; }
    int Hp { get; set; }

    void GetDamage(int damage);
    void Death();
}
public interface IFollowable
{
    float MoveSpeed { get; set; }
    float DistanceToPlayer { get; set; }

    void CalculateDistance(Vector3 originPos, Vector3 targetPos);     
}