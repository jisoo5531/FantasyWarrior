using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerDamagable : Damagable, IDamagable
{
    public int Unit_ID { get; private set; }

    // SyncVar는 필드에만 적용 가능
    //[SyncVar(hook = nameof(OnHpSynced))]
    public int _maxHp; // 필드로 변경

    //[SyncVar(hook = nameof(OnHpSynced))]
    public int _hp; // 필드로 변경

    // 인터페이스의 속성 구현
    public int MaxHp
    {
        get => _maxHp; // 내부 필드 참조
        set => _maxHp = value; // 필드에 값 설정
    }

    public int Hp
    {
        get => _hp; // 내부 필드 참조
        set => _hp = value; // 필드에 값 설정
    }

    public bool isStunned { get; set; }
    private bool isDeath = false;
    private bool isMonster = false;

    #region 이벤트
    public event Action<int> OnTakeDamage;
    public event Action OnChangeHPEvent;
    public event Action OnDeath;
    #endregion

    private MonsterData monsterData;
    // 서버에서 몬스터 데이터를 요청하는 Command


    public override void Initialize(int unitID, int maxHp, int hp, bool isMonster)
    {
        this.isMonster = isMonster;

        this.Unit_ID = unitID;
        this._maxHp = maxHp; // 속성 사용 (내부적으로 필드에 값이 설정됨)
        this._hp = hp; // 속성 사용        

        if (isMonster)
        {
            //RPCSyncHP(maxHp, hp);
        }

        EventListener();
    }
    [ClientRpc]
    private void RPCSyncHP(int maxHP, int HP)
    {
        this._maxHp = maxHP;
        this._hp = HP;
    }

    private void EventListener()
    {
        if (isMonster) return;

        UserStatManager.Instance.OnLevelUpUpdateStat += OnChangeHp;
        PlayerEquipManager.Instance.OnEquipItem += OnChangeHp;
        PlayerEquipManager.Instance.OnUnEquipItem += OnChangeHp;
        PlayerEquipManager.Instance.OnAllUnEquipButtonClick += OnChangeHp;
    }

    private void OnDisable()
    {
        if (isMonster) return;

        UserStatManager.Instance.OnLevelUpUpdateStat -= OnChangeHp;
        PlayerEquipManager.Instance.OnEquipItem -= OnChangeHp;
        PlayerEquipManager.Instance.OnUnEquipItem -= OnChangeHp;
        PlayerEquipManager.Instance.OnAllUnEquipButtonClick -= OnChangeHp;
    }

    [Command]
    public void CmdGetDamage(int damage)
    {
        GetDamage(damage);
    }
    public override void GetDamage(int damage)
    {
        if (isDeath) return;
        Debug.Log(FindObjectOfType<MonsterDamagable>().Hp);
        Debug.Log(damage);
        Hp -= damage; // 체력 감소

        if (Hp <= 0)
        {
            Hp = 0;
            Death();
        }

        OnTakeDamage?.Invoke(damage); // 데미지 이벤트 호출
    }

    [Server] // 서버에서만 실행
    public void Death()
    {
        isDeath = true;
        OnDeath?.Invoke(); // 사망 이벤트 호출
    }

    private void OnChangeHp(int userid)
    {
        if (this.Unit_ID != userid) return;

        UserStatClient userStatClient = UserStatManager.Instance.GetUserStatClient(this.Unit_ID);
        this.MaxHp = userStatClient.MaxHP; // 속성 사용
        this.Hp = userStatClient.HP; // 속성 사용

        OnChangeHPEvent?.Invoke(); // 체력 변경 이벤트 호출
    }

    // SyncVar를 통해 체력 변경 시 클라이언트에서 UI 업데이트
    private void OnHpSynced(int oldHp, int newHp)
    {
        OnChangeHPEvent?.Invoke(); // UI 업데이트 트리거
    }
}
