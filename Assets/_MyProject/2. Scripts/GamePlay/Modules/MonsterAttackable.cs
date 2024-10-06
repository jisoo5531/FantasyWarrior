using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MonsterAttackable : NetworkBehaviour, IAttackable
{
    //TODO : 레벨업 같은 스탯이 바뀌는 상황에서 이벤트로 스탯 반영하기
    public LayerMask TargetLayer { get; set; }

    public int _damage;

    public float _range;

    // 인터페이스의 속성 구현
    public int Damage
    {
        get => _damage; // 내부 필드 참조
        set => _damage = value; // 필드에 값 설정
    }

    public float Range
    {
        get => _range; // 내부 필드 참조
        set => _range = value; // 필드에 값 설정
    }

    [SerializeField] public Weapon weapon;
    [SerializeField] public Collider weaponCollider;

    private void Awake()
    {
        weapon = GetComponentInChildren<Weapon>();
        weaponCollider = weapon.GetComponent<Collider>();
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
        //RPCInitialize(damage, range);
    }

    [ClientRpc]
    public void RPCInitialize(int damage, float range)
    {
        this.Damage = damage;
        this.Range = range;
    }
    
    public void SendDamage(PlayerDamagable damagable)
    {
        
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
