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

    protected virtual void OnTriggerEnter(Collider other)
    {                
        // override    
    }
    private void OnTriggerStay(Collider other)
    {
        if ((targetLayer | (1 << other.gameObject.layer)) != targetLayer)
        {            
            return;
        }
        GetComponent<Collider>().enabled = false;
    }
}
