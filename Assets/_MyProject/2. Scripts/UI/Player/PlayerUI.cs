using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : UIComponent
{
    // TODO : HP, MP, EXP, ��ų, ������

    public Slider MpBar;
    public Slider ExpBar;

    private void Awake()
    {
        
    }

    public override void Initialize(Damagable damagable)
    {
        base.Initialize(damagable);


    }
}
