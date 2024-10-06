using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerAttackable : NetworkBehaviour
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
    }

    [Command]
    public void CmdRequestSendDamage(NetworkIdentity damagableIdentity)
    {
        Debug.LogError(damagableIdentity == null);
        Debug.Log("서버야 처리해줘 ");
        // NetworkIdentity를 통해 해당 오브젝트를 찾음
        if (damagableIdentity.TryGetComponent<MonsterDamagable>(out var damagable))
        {
            Debug.Log("서버에서 데미지 처리.");
            damagable.GetDamage(this.Damage);
        }
        else
        {
            Debug.LogError("대상 몬스터를 찾을 수 없습니다.");
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
        // 서버에서 호출된 경우에만 데미지를 적용하도록 합니다.       
        Debug.Log("Damagable found: " + damagable);
        Debug.Log(damagable.GetComponent<NetworkIdentity>() == null);

        Debug.Log(identity.isServer);
        Debug.Log(identity.isLocalPlayer);
        Debug.Log(isLocalPlayer);
        if (identity.isLocalPlayer)
        {
            Debug.Log(damagable.GetComponent<NetworkIdentity>() == null);
            // 클라이언트에서 서버에 요청
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
