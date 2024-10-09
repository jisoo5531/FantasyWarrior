using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour, IAttackable
{
    //TODO : 레벨업 같은 스탯이 바뀌는 상황에서 이벤트로 스탯 반영하기
    public LayerMask TargetLayer { get; set; }
    public int Damage { get; set; }
    public float Range { get; set; }

    [SerializeField] public Weapon weapon;
    [SerializeField] public Collider weaponCollider;

    private void Awake()
    {
        if (weapon != null)
        {
            weapon = GetComponentInChildren<Weapon>();
        }
        if (weaponCollider != null)
        {
            weaponCollider = weapon.GetComponent<Collider>();
        }
    }

    public void Initialize(int damage, float range)
    {
        this.Damage = damage;
        this.Range = range;
        if (weapon != null)
        {
            this.TargetLayer = weapon.targetLayer;
        }            
        
        if (weapon != null)
        {
            weapon.damage = this.Damage;
        }
    }

    public void SendDamage(Damagable damagable)
    {
        damagable.GetDamage(this.Damage);
    }

    public void OnWeaponCollider()
    {
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
