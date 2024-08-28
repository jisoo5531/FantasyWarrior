using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUI : MonoBehaviour
{
    public UI_HPBar ui_HPBar;
    private Slider hpBar;

    private void Awake()
    {
        hpBar = GetComponentInChildren<Slider>();        
        
    }
    public void Initialize(Damagable damagable)
    {
        ui_HPBar = new UI_HPBar(damagable, hpBar);
        Debug.Log("¿©±â?");
    }
}
