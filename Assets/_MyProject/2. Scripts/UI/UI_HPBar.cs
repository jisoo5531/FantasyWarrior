using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar
{
    private Slider hpBar;

    private Damagable damagable;    

    public UI_HPBar(Damagable damagable, Slider slider)
    {        
        this.damagable = damagable;
        this.hpBar = slider;

        damagable.OnHpChange += OnHpChange;        

        SetInitValue();
    }
    private void SetInitValue()
    {
        Debug.Log(damagable.MaxHp);
        if (hpBar != null)
        {
            Debug.Log("¿©±â?");
            hpBar.maxValue = (float)damagable.MaxHp;
            hpBar.value = hpBar.maxValue;
        }        
    }

    private void OnHpChange(int damage)
    {
        Debug.Log(hpBar == null);
        Debug.Log(damagable == null);
        hpBar.value = damagable.Hp;
    }
}
