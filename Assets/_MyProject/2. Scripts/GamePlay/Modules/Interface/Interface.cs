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
    float Range { get; set; }

    void SendDamage(int damage);
}

public interface IDamagable
{
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