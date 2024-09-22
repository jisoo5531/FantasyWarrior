using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour, IDamagable
{
    public int Unit_ID { get; private set; }

    public int MaxHp { get; set; }
    public int Hp { get; set; }

    public bool isStunned { get; set; }

    private bool isDeath = false;


    #region �̺�Ʈ

    /// <summary>
    /// ������ �޾� �������� �Ծ��� �� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public event Action<int> OnTakeDamage;
    /// <summary>
    /// �÷��̾� ���� ���̳� ���Ͱ� Ư�� ��Ȳ���� ������ �ٲ���� �� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public event Action OnChangeHPEvent;
    /// <summary>
    /// �׾��� �� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public event Action OnDeath;

    #endregion

    public void Initialize(int unitID, int maxHp, int hp)
    {
        this.Unit_ID = unitID;
        this.MaxHp = maxHp;
        this.Hp = hp;
    }
    private void Awake()
    {
        
    }
    private void Start()
    {
        UserStatManager.Instance.OnLevelUpUpdateStat += OnChangeHp;
        PlayerEquipManager.Instance.OnEquipItem += OnChangeHp;
        PlayerEquipManager.Instance.OnUnEquipItem += OnChangeHp;
        PlayerEquipManager.Instance.OnAllUnEquipButtonClick += OnChangeHp;
    }

    private void OnDisable()
    {
        UserStatManager.Instance.OnLevelUpUpdateStat -= OnChangeHp;
        PlayerEquipManager.Instance.OnEquipItem -= OnChangeHp;
        PlayerEquipManager.Instance.OnUnEquipItem -= OnChangeHp;
        PlayerEquipManager.Instance.OnAllUnEquipButtonClick -= OnChangeHp;
    }

    public void ApplyStatusEffect(IStatusEffect statusEffect)
    {
        statusEffect.Apply(this);
    }

    public void RemoveStatusEffect(IStatusEffect statusEffect)
    {
        statusEffect.Remove(this);
    }

    public void GetDamage(int damage)
    {
        if (isDeath)
        {
            return;
        }        
        if (Hp <= 0)
        {
            Hp = 0;            
            Death();
        }
        OnTakeDamage?.Invoke(damage);
    }
    public void Death()
    {
        isDeath = true;        
        OnDeath?.Invoke();
    }

    private void OnChangeHp()
    {
        UserStatClient userStatClient = UserStatManager.Instance.userStatClient;
        this.MaxHp = userStatClient.MaxHP;
        this.Hp = userStatClient.HP;        
        OnChangeHPEvent?.Invoke();
    }
}
