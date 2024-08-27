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
        Debug.Log("�¾Ҵ�.");
        if ((targetLayer | (1 << other.gameObject.layer)) != targetLayer)
        {
            Debug.Log("Ÿ�� ���̾� �ƴ�");
            IsReturn = true;
            return;
        }
        if (other.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            Debug.Log("���¾Ҵ�.");
            damagable.GetDamage(damage);
        }

        IsReturn = false;
    }
}
