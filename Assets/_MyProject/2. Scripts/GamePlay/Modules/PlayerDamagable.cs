using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerDamagable : Damagable, IDamagable
{
    public int Unit_ID { get; private set; }

    // SyncVar�� �ʵ忡�� ���� ����
    //[SyncVar(hook = nameof(OnHpSynced))]
    public int _maxHp; // �ʵ�� ����

    //[SyncVar(hook = nameof(OnHpSynced))]
    public int _hp; // �ʵ�� ����

    // �������̽��� �Ӽ� ����
    public int MaxHp
    {
        get => _maxHp; // ���� �ʵ� ����
        set => _maxHp = value; // �ʵ忡 �� ����
    }

    public int Hp
    {
        get => _hp; // ���� �ʵ� ����
        set => _hp = value; // �ʵ忡 �� ����
    }

    public bool isStunned { get; set; }
    private bool isDeath = false;
    private bool isMonster = false;

    #region �̺�Ʈ
    public event Action<int> OnTakeDamage;
    public event Action OnChangeHPEvent;
    public event Action OnDeath;
    #endregion

    private MonsterData monsterData;
    // �������� ���� �����͸� ��û�ϴ� Command


    public override void Initialize(int unitID, int maxHp, int hp, bool isMonster)
    {
        this.isMonster = isMonster;

        this.Unit_ID = unitID;
        this._maxHp = maxHp; // �Ӽ� ��� (���������� �ʵ忡 ���� ������)
        this._hp = hp; // �Ӽ� ���        

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
        Hp -= damage; // ü�� ����

        if (Hp <= 0)
        {
            Hp = 0;
            Death();
        }

        OnTakeDamage?.Invoke(damage); // ������ �̺�Ʈ ȣ��
    }

    [Server] // ���������� ����
    public void Death()
    {
        isDeath = true;
        OnDeath?.Invoke(); // ��� �̺�Ʈ ȣ��
    }

    private void OnChangeHp(int userid)
    {
        if (this.Unit_ID != userid) return;

        UserStatClient userStatClient = UserStatManager.Instance.GetUserStatClient(this.Unit_ID);
        this.MaxHp = userStatClient.MaxHP; // �Ӽ� ���
        this.Hp = userStatClient.HP; // �Ӽ� ���

        OnChangeHPEvent?.Invoke(); // ü�� ���� �̺�Ʈ ȣ��
    }

    // SyncVar�� ���� ü�� ���� �� Ŭ���̾�Ʈ���� UI ������Ʈ
    private void OnHpSynced(int oldHp, int newHp)
    {
        OnChangeHPEvent?.Invoke(); // UI ������Ʈ Ʈ����
    }
}
