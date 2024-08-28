using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour, IDamagable
{
    public int MaxHp { get; set; }
    public int Hp { get; set; }


    private bool isDeath = false;

    public void Initialize(int maxHp, int hp)
    {
        this.MaxHp = maxHp;
        this.Hp = hp;
    }

    
    public void GetDamage(int damage)
    {
        if (isDeath)
        {
            return;
        }

        EventHandler.actionEvent.TriggerHpChange(damage);        

        if (Hp <= 0)
        {                        
            Death();
        }
    }
    public void Death()
    {
        isDeath = true;
        Debug.Log("ав╬З╢ы");
        EventHandler.actionEvent.TriggetDeath();        
    }
}
