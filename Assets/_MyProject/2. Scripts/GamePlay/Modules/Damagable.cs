using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour, IDamagable
{
    // TODO : 장비 장착, 해제 시 스탯 반영한거. 플레이어 UI에도 반영
    public int MaxHp { get; set; }
    public int Hp { get; set; }


    private bool isDeath = false;

    /// <summary>
    /// 공격을 받아 데미지를 입었을 때 발생하는 이벤트
    /// </summary>
    public event Action<int> OnHpChange;
    /// <summary>
    /// 플레이어 레벨 업이나 몬스터가 특정 상황에서 스탯이 바뀌었을 때 발생하는 이벤트
    /// </summary>
    public event Action OnChangeHPEvent;
    /// <summary>
    /// 죽었을 때 발생하는 이벤트
    /// </summary>
    public event Action OnDeath;

    public void Initialize(int maxHp, int hp)
    {
        this.MaxHp = maxHp;
        this.Hp = hp;
    }
    private void Awake()
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

    public void GetDamage(int damage)
    {
        if (isDeath)
        {
            return;
        }

        OnHpChange?.Invoke(damage);

        if (Hp <= 0)
        {                        
            Death();
        }
    }
    public void Death()
    {
        isDeath = true;
        Debug.Log("죽었다");
        OnDeath?.Invoke();
    }

    private void OnChangeHp()
    {
        UserStatData userStatData = UserStatManager.Instance.GetUserStatDataFromDB();
        this.MaxHp = userStatData.MaxHp;
        this.Hp = userStatData.Hp;        
        OnChangeHPEvent?.Invoke();
    }
}
