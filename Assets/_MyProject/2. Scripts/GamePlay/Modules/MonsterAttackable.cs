using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MonsterAttackable : NetworkBehaviour, IAttackable
{
    //TODO : ������ ���� ������ �ٲ�� ��Ȳ���� �̺�Ʈ�� ���� �ݿ��ϱ�
    public LayerMask TargetLayer { get; set; }

    public int _damage;

    public float _range;

    // �������̽��� �Ӽ� ����
    public int Damage
    {
        get => _damage; // ���� �ʵ� ����
        set => _damage = value; // �ʵ忡 �� ����
    }

    public float Range
    {
        get => _range; // ���� �ʵ� ����
        set => _range = value; // �ʵ忡 �� ����
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
