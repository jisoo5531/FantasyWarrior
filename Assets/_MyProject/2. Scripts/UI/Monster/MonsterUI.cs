using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterUI : UIComponent
{
    public GameObject DamagedContent;
    public TMP_Text DamagedText;
    private void Awake()
    {
        Debug.Log("자식 Monster Awake");
        hpBar = GetComponentInChildren<Slider>();
    }

    public override void SetInitValue()
    {
        Debug.Log("여기?");
        Damagable.OnHpChange += OnHpChange;

        if (hpBar != null)
        {            
            hpBar.maxValue = (float)Damagable.MaxHp;
            hpBar.value = Damagable.Hp;

            hpText.text = $"{Damagable.Hp}";
        }
    }
    public override void OnHpChange(int damage)
    {
        base.OnHpChange(damage);
        if (Damagable.Hp <= 0)
        {
            Damagable.Hp = 0;
        }
        hpBar.value = Damagable.Hp;
        hpText.text = $"{Damagable.Hp}";

        DamagedText.text = damage.ToString();
        var damageText = Instantiate(DamagedText, DamagedContent.transform);
        Destroy(damageText.gameObject, 1 - Time.deltaTime);
    }    
}
