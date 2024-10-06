using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinWeapon : Weapon
{
    protected override void OnTriggerEnter(Collider other)
    {
        if ((targetLayer | (1 << other.gameObject.layer)) != targetLayer)
        {
            Debug.Log("Å¸°Ù ·¹ÀÌ¾î ¾Æ´Ô");
            return;
        }

        if (other.TryGetComponent(out PlayerDamagable damagable))
        {
            transform.root.GetComponent<MonsterAttackable>().SendDamage(damagable);            
        }
    }
}
