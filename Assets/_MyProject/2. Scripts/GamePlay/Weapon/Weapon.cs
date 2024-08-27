using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public int damage { get; set; }

    public LayerMask targetLayer;

    public virtual void Initialize()
    {

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log("�¾Ҵ�.");
        if ((targetLayer | (1 << other.gameObject.layer)) != targetLayer)
        {
            Debug.Log("Ÿ�� ���̾� �ƴ�");
            return;
        }

        if (other.TryGetComponent(out Damagable damagable))
        {
            damagable.GetDamage(damage);
        }           
    }
}
