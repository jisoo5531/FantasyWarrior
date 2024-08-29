using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_HPBar
{
    private Slider hpBar;
    private TextMeshProUGUI hpText;

    private Damagable damagable;    

    public UI_HPBar(Damagable damagable, Slider slider, TextMeshProUGUI hpText)
    {        
        this.damagable = damagable;
        this.hpBar = slider;
        this.hpText = hpText;

        damagable.OnHpChange += OnHpChange;        

        SetInitValue();
    }
    public void SetInitValue()
    {
        Debug.Log(damagable.MaxHp);
        if (hpBar != null)
        {
            Debug.Log("¿©±â?");
            hpBar.maxValue = (float)damagable.MaxHp;
            hpBar.value = hpBar.maxValue;

            hpText.text = $"{damagable.Hp}";
        }        
    }

    private void OnHpChange(int damage)
    {        
        hpBar.value = damagable.Hp;
        hpText.text = $"{damagable.Hp}";
    }
}
