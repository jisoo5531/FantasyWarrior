using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage { get; set; }

    public LayerMask targetLayer;

    public virtual void Initialize()
    {

    }

    protected void OnTriggerEnter(Collider other)
    {
        Debug.Log("맞았다.");
        Debug.Log("부모 트리거");
        if ((targetLayer | (1 << other.gameObject.layer)) != targetLayer)
        {
            Debug.Log("타겟 레이어 아님");
            return;
        }

        if (other.TryGetComponent(out Damagable damagable))
        {            
            damagable.GetDamage(damage);
        }           
    }
    private void OnTriggerStay(Collider other)
    {
        if ((targetLayer | (1 << other.gameObject.layer)) != targetLayer)
        {
            Debug.Log("타겟 레이어 아님");
            return;
        }
        GetComponent<Collider>().enabled = false;
    }
}
