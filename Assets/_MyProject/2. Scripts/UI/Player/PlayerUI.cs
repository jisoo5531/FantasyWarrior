using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerUI : UIComponent
{
    // TODO : HP, MP, EXP, ��ų, ������

    [Header("MP")]
    public Slider MpBar;
    [Header("Xp")]
    public Slider ExpBar;
    [Header("Skill")]
    public List<Image> skillIconList;

    private void Awake()
    {
        EventHandler.skillKey.RegisterSkillKeyChange(OnChangeSkill);
    }
    private void Start()
    {
        for (int i = 0; i < skillIconList.Count; i++)
        {
            Sprite skillIcon = UIManager.Instance.skillIconList[PlayerSkill.EquipSkills[i] - 1];
            skillIconList[i].sprite = skillIcon;
        }
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

    private void OnChangeSkill()
    {
        for (int i = 0; i < skillIconList.Count; i++)
        {
            if (PlayerSkill.EquipSkills[i] == 0)
            {
                skillIconList[i].sprite = null;
                skillIconList[i].ImageTransparent(0);

                continue;
            }
          
            skillIconList[i].sprite = UIManager.Instance.skillIconList[PlayerSkill.EquipSkills[i] - 1];
            skillIconList[i].ImageTransparent(1);
        }
    }
}
