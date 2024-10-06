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
        Debug.Log("서버? " + isServer);
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
        Debug.Log("여기?");
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

            // 클라이언트 초기화 호출
            RpcInitializeClient(this.maxHp, this.currentHp);
        }
    }

    [ClientRpc]
    private void RpcInitializeClient(int maxHp, int Hp)
    {
        //Debug.LogError($"{maxHp}, {Hp}");
        //Debug.LogError("클라이언트한테 초기화");

        // 클라이언트에서 값을 읽어서 초기화
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
        currentHp = Damagable.Hp; // 동기화
        if (currentHp < 0)
        {
            currentHp = 0;
        }

        // UI 업데이트
        RpcInitializeClient(Damagable.MaxHp, Damagable.Hp);

        // 데미지 텍스트 생성
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
