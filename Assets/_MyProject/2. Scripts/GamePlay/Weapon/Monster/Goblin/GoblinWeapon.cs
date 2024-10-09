using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinWeapon : Weapon
{
    protected override void OnTriggerEnter(Collider other)
    {
        if ((targetLayer | (1 << other.gameObject.layer)) != targetLayer)
        {
            Debug.Log("Ÿ�� ���̾� �ƴ�");
            return;
        }

        
        if (other.TryGetComponent(out Damagable damagable))
        {
            transform.root.GetComponent<Attackable>().SendDamage(damagable);
        }
    }

    
}
