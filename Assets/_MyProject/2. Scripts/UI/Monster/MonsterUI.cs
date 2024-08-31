using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUI : UIComponent
{
    private void Awake()
    {
        Debug.Log("ÀÚ½Ä Monster Awake");
        hpBar = GetComponentInChildren<Slider>();
    }

    public override void SetInitValue()
    {
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
        hpBar.value = Damagable.Hp;
        hpText.text = $"{Damagable.Hp}";
    }    
}
