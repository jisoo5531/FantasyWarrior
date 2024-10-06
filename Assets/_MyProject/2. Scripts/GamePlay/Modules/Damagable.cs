using System;
using UnityEngine;
using Mirror;

public class Damagable : NetworkBehaviour
{

    public virtual void Initialize(int unitID, int maxHp, int hp, bool isMonster)
    {
    }

    public virtual void GetDamage(int damage)
    {

    }
}
