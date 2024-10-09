using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public interface IState
{
    void Enter();
    void Excute();
    void Exit();
}
public interface IAttackable
{
    LayerMask TargetLayer { get; set; }
    int Damage { get; set; }
    float Range { get; set; }

    void SendDamage(Damagable damagable);
}
public interface IDamagable
{
    event Action<int> OnTakeDamage;
    event Action OnDeath;
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
public interface IHpHandler 
{
    Damagable Damagable { get; set; }

    void SetInitValue();
    void OnHpChange(int damage);
}
/// <summary>
/// 상태이상 효과를 관리하는 인터페이스
/// </summary>
public interface IStatusEffect
{
    void Apply(Damagable target);
    void Remove(Damagable target);
}