using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIComponent : MonoBehaviour, IHpHandler
{    
    [Header("HP")]
    public Slider hpBar;
    public TextMeshProUGUI hpText;
    
    public Damagable Damagable { get; set; }

    public void Initialize(Damagable damagable)
    {
        this.Damagable = damagable;        
        SetInitValue();
    }

    public virtual void SetInitValue()
    {
        
    }
    public virtual void OnHpChange(int damage)
    {        
    }    
}
