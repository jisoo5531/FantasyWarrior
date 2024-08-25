using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : Weapon
{
    public override int damage { get; set; }
    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (IsReturn)
        {
            return;
        }
        
        
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Ä® Âñ·¶´Ù ³ª¿È. ´Ù½Ã Âî¸¦ ÁØºñ!");
        //if (((targetLayer | (1 << other.gameObject.layer)) != targetLayer) && false == boxCollider.enabled)
        //{
        //    return;
        //}
        boxCollider.enabled = true;   
    }
}
