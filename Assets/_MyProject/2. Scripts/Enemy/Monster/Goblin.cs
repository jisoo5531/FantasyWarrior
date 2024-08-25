using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Monster
{
    public override int MaxHp { get; set; }
    public override int Hp { get; set; }

    private void Awake()
    {
        MaxHp = 100;
        Hp = MaxHp;
    }

    public override void GetDamage(int damage)
    {
        Hp -= damage;

        if (Hp <= 0)
        {
            Death();
        }
    }
    public override void Death()
    {
        Debug.Log("고블린 죽었다.");
    }
}
