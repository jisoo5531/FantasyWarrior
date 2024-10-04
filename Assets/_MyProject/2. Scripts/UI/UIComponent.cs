using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class UIComponent : MonoBehaviour, IHpHandler
{    
    [Header("HP")]
    public Slider hpBar;
    public TextMeshProUGUI hpText;
    
    public Damagable Damagable { get; set; }

    public void Initialize(Damagable damagable)
    {
        // 서버에서 실행되지 않도록
        if (NetworkServer.active)
        {
            return; // 서버에서는 UI를 실행하지 않음
        }
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
