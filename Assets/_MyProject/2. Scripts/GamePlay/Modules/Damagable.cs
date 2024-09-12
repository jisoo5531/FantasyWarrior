using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour, IDamagable
{
    public int Unit_ID { get; private set; }

    // TODO : ��� ����, ���� �� ���� �ݿ��Ѱ�. �÷��̾� UI���� �ݿ�
    public int MaxHp { get; set; }
    public int Hp { get; set; }


    private bool isDeath = false;

    /// <summary>
    /// ������ �޾� �������� �Ծ��� �� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public event Action<int> OnHpChange;
    /// <summary>
    /// �÷��̾� ���� ���̳� ���Ͱ� Ư�� ��Ȳ���� ������ �ٲ���� �� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public event Action OnChangeHPEvent;
    /// <summary>
    /// �׾��� �� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public event Action OnDeath;

    public void Initialize(int unitID, int maxHp, int hp)
    {
        this.Unit_ID = unitID;
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
