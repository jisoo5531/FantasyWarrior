using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : UIComponent
{
    // TODO : HP, MP, EXP, 스킬, 아이템

    [Header("MP")]
    public Slider MpBar;
    [Header("Xp")]
    public Slider ExpBar;


    public override void SetInitValue()
    {
        Damagable.OnHpChange += OnHpChange;

        if (hpBar != null)
        {            
            hpBar.maxValue = (float)Damagable.MaxHp;
            hpBar.value = hpBar.maxValue;

            hpText.text = $"{Damagable.Hp} / {Damagable.MaxHp}";
        }
    }
    public override void OnHpChange(int damage)
    {
        hpBar.value = Damagable.Hp;
        hpText.text = $"{Damagable.Hp} / {Damagable.MaxHp}";
    }
}
