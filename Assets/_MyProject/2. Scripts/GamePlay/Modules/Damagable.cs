using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour, IDamagable
{
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

    public void Initialize(int maxHp, int hp)
    {
        this.MaxHp = maxHp;
        this.Hp = hp;
    }
    private void Start()
    {
        EventHandler.playerEvent.RegisterPlayerLevelUp(OnLevelUpChangeHp);
    }

    private void OnDisable()
    {
        EventHandler.playerEvent.UnRegisterPlayerLevelUp(OnLevelUpChangeHp);
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
        Debug.Log("�׾���");
        OnDeath?.Invoke();
    }

    private void OnLevelUpChangeHp()
    {
        UserStatData userStatData = DatabaseManager.Instance.userStatData;
        this.MaxHp = userStatData.MaxHp;
        this.Hp = userStatData.Hp;
        OnChangeHPEvent?.Invoke();
    }
}
