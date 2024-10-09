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
    private bool isMonster = false;

    #region 이벤트

    /// <summary>
    /// 공격을 받아 데미지를 입었을 때 발생하는 이벤트
    /// </summary>
    public event Action<int> OnTakeDamage;
    /// <summary>
    /// 플레이어 레벨 업이나 몬스터가 특정 상황에서 스탯이 바뀌었을 때 발생하는 이벤트
    /// </summary>
    public event Action OnChangeHPEvent;
    /// <summary>
    /// 죽었을 때 발생하는 이벤트
    /// </summary>
    public event Action OnDeath;

    #endregion

    public void Initialize(int unitID, int maxHp, int hp, bool isMonster)
    {
        this.isMonster = isMonster;
        this.Unit_ID = unitID;
        this.MaxHp = maxHp;
        this.Hp = hp;
    }

    private void Start()
    {
        if (isMonster)
        {
            return;
        }
        UserStatManager.Instance.OnLevelUpUpdateStat += OnChangeHp;
        PlayerEquipManager.Instance.OnEquipItem += OnChangeHp;
        PlayerEquipManager.Instance.OnUnEquipItem += OnChangeHp;
        PlayerEquipManager.Instance.OnAllUnEquipButtonClick += OnChangeHp;
    }
    private void OnDisable()
    {
        if (isMonster)
        {
            return;
        }
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
        Debug.Log(gameObject.name + "데미지 입었다");
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
