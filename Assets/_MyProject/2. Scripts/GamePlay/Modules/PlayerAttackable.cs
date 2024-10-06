using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerAttackable : NetworkBehaviour
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
    }

    [Command]
    public void CmdRequestSendDamage(NetworkIdentity damagableIdentity)
    {
        Debug.LogError(damagableIdentity == null);
        Debug.Log("������ ó������ ");
        // NetworkIdentity�� ���� �ش� ������Ʈ�� ã��
        if (damagableIdentity.TryGetComponent<MonsterDamagable>(out var damagable))
        {
            Debug.Log("�������� ������ ó��.");
            damagable.GetDamage(this.Damage);
        }
        else
        {
            Debug.LogError("��� ���͸� ã�� �� �����ϴ�.");
        }
    }
    public void SendDamage(MonsterDamagable damagable)
    {        
        NetworkIdentity identity = GetComponent<NetworkIdentity>();
        bool isLocalPlayer = GetComponent<PlayerController>().isLocalPlayer;
        if (!isLocalPlayer)
        {
            return;
        }        
        Debug.Log("ddd ");
        // �������� ȣ��� ��쿡�� �������� �����ϵ��� �մϴ�.       
        Debug.Log("Damagable found: " + damagable);
        Debug.Log(damagable.GetComponent<NetworkIdentity>() == null);

        Debug.Log(identity.isServer);
        Debug.Log(identity.isLocalPlayer);
        Debug.Log(isLocalPlayer);
        if (identity.isLocalPlayer)
        {
            Debug.Log(damagable.GetComponent<NetworkIdentity>() == null);
            // Ŭ���̾�Ʈ���� ������ ��û
            CmdRequestSendDamage(damagable.GetComponent<NetworkIdentity>());
        }
        //GetComponent<PlayerController>().CmdRequestSendDamage(damagable.GetComponent<NetworkIdentity>(), this.Damage);        
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
