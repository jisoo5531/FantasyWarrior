using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour, IAttackable
{
    public int Damage { get; set; }
    public float Range { get; set; }

    public void Initialize(int damage, float range)
    {
        this.Damage = damage;
        this.Range = range;
    }

    public void SendDamage(int damage)
    {
        
    }
}
