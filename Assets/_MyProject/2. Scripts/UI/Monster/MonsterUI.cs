using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class MonsterUI : NetworkBehaviour
{
    public MonsterDamagable Damagable;
    
    public int maxHp;    
    public int currentHp;

    [Header("HP")]
    public Slider hpBar;
    public TextMeshProUGUI hpText;
    public GameObject DamagedContent;
    public TMP_Text DamagedText;

    private void Awake()
    {
        hpBar = GetComponentInChildren<Slider>();
    }

    public void Initialize(MonsterDamagable damagable)
    {
        Debug.Log("����? " + isServer);
        this.Damagable = damagable;

        if (damagable != null)
        {
            this.maxHp = damagable.MaxHp;
            this.currentHp = damagable.Hp;

            SetInitValue();
        }
    }

    public void SetInitValue()
    {
        Debug.Log("����?");
        if (hpBar != null)
        {
            Debug.Log($"{this.maxHp}, {this.currentHp}");
            hpBar.maxValue = (float)maxHp;
            hpBar.value = this.currentHp;
            hpText.text = $"{this.currentHp}";
        }

        if (isServer)
        {
            Damagable.OnTakeDamage += OnHpChange;
            Damagable.OnChangeHPEvent += UpdateHpUI;

            // Ŭ���̾�Ʈ �ʱ�ȭ ȣ��
            RpcInitializeClient(this.maxHp, this.currentHp);
        }
    }

    [ClientRpc]
    private void RpcInitializeClient(int maxHp, int Hp)
    {
        //Debug.LogError($"{maxHp}, {Hp}");
        //Debug.LogError("Ŭ���̾�Ʈ���� �ʱ�ȭ");

        // Ŭ���̾�Ʈ���� ���� �о �ʱ�ȭ
        UpdateUI(maxHp, Hp);
    }

    private void UpdateUI(int maxHp, int currentHp)
    {
        if (hpBar != null)
        {
            hpBar.maxValue = (float)maxHp;
            hpBar.value = currentHp;
            hpText.text = $"{currentHp}";
        }
    }

    public void OnHpChange(int damage)
    {
        if (Damagable.Hp <= 0) return;

        Damagable.Hp -= damage;
        currentHp = Damagable.Hp; // ����ȭ
        if (currentHp < 0)
        {
            currentHp = 0;
        }

        // UI ������Ʈ
        RpcInitializeClient(Damagable.MaxHp, Damagable.Hp);

        // ������ �ؽ�Ʈ ����
        DamagedText.text = damage.ToString();
        var damageText = Instantiate(DamagedText, DamagedContent.transform);
        Destroy(damageText.gameObject, 1 - Time.deltaTime);
    }

    private void UpdateHpUI()
    {
        if (hpBar != null)
        {
            hpBar.value = currentHp;
            hpText.text = $"{currentHp}";
        }
    }
}
