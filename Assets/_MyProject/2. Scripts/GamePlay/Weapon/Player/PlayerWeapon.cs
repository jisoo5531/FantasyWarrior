using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : Weapon
{    
    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);              
    }
    
}
