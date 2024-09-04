using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerUI : UIComponent
{
    // TODO : HP, MP, EXP, 스킬, 아이템
    [Header("Panel")]
    public UI_SkillPanel SkillPanel;


    [Header("MP")]
    public Slider MpBar;
    [Header("Xp")]
    public Slider ExpBar;

    private void OnEnable()
    {        
        Debug.Log(GameManager.inputActions == null);
        GameManager.inputActions.PlayerActions.UI_Skill.performed += OnSkill_UI;
    }
    private void OnDisable()
    {
        GameManager.inputActions.PlayerActions.UI_Skill.performed -= OnSkill_UI;
    }

    private void OnSkill_UI(InputAction.CallbackContext context)
    {
        SkillPanel.gameObject.SetActive(!SkillPanel.gameObject.activeSelf);
    }

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
