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
        if ((targetLayer | (1 << other.gameObject.layer)) != targetLayer)
        {
            Debug.Log("Å¸°Ù ·¹ÀÌ¾î ¾Æ´Ô");
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
            Debug.Log("Å¸°Ù ·¹ÀÌ¾î ¾Æ´Ô");
            return;
        }
        GetComponent<Collider>().enabled = false;
    }
}
