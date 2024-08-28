using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour, IAttackable
{
    public LayerMask TargetLayer { get; set; }
    public int Damage { get; set; }
    public float Range { get; set; }

    [SerializeField] public Weapon weapon;
    [SerializeField] public BoxCollider weaponCollider;

    private void Awake()
    {
        weapon = GetComponentInChildren<Weapon>();
        weaponCollider = weapon.GetComponent<BoxCollider>();
    }
    
    public void Initialize(int damage, float range)
    {
        this.Damage = damage;
        this.Range = range;
        this.TargetLayer = weapon.targetLayer;

        if (weapon != null)
        {
            weapon.damage = this.Damage;
        }
    }

    public void SendDamage(int damage)
    {
        
    }

    public void OnWeaponCollider()
    {
        Debug.Log("콜라이더 켜짐");
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
        }        
    }
    public void OffWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }
}
