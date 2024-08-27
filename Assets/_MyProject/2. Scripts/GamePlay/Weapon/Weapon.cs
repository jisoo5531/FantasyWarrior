using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract int damage { get; set; }

    public LayerMask targetLayer;

    protected bool IsReturn = false;

    protected virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log("맞았다.");
        if ((targetLayer | (1 << other.gameObject.layer)) != targetLayer)
        {
            Debug.Log("타겟 레이어 아님");
            IsReturn = true;
            return;
        }
        if (other.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            Debug.Log("적맞았다.");
            damagable.GetDamage(damage);
        }

        IsReturn = false;
    }
}
