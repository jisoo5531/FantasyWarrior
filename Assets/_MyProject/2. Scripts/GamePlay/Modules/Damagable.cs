using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour, IDamagable
{
    public int MaxHp { get; set; }
    public int Hp { get; set; }

    public event DeathEventHandler OnDeathEvent;
    public event HpChangeEventHandler OnHpChangeEvent;

    public Damagable(int maxHp, int hp)
    {
        this.MaxHp = maxHp;
        this.Hp = hp;
    }

    public virtual void Death()
    {
        OnDeathEvent?.Invoke();
    }

    public virtual void GetDamage(int damage)
    {
        OnHpChangeEvent?.Invoke();
    }
}
