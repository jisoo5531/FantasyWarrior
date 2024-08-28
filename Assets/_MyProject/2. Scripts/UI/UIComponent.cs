using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIComponent : MonoBehaviour
{    
    public Slider hpBar;

    protected UI_HPBar ui_HPBar;

    public virtual void Initialize(Damagable damagable)
    {
        Debug.Log(hpBar == null);
        ui_HPBar = new UI_HPBar(damagable, hpBar);
        
    }
}
