using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : UIComponent
{
    // TODO : HP, MP, EXP, 스킬, 아이템
    // TODO : HP UI 동그란 걸로 바꾸기?
    [Header("MP")]
    public Slider MpBar;
    public TMP_Text mpText;
    [Header("Xp")]
    public Slider ExpBar;
    public TMP_Text expText;
    [Header("Skill")]
    public List<Image> skillIconList;    

    private void Awake()
    {
        EventHandler.skillKey.RegisterSkillKeyChange(OnChangeSkillKeyBind);        
    }
    private void Start()
    {
        PlayerEquipManager.Instance.OnEquipItem += OnChangeMP;
        PlayerEquipManager.Instance.OnUnEquipItem += OnChangeMP;
        PlayerEquipManager.Instance.OnAllUnEquipButtonClick += OnChangeMP;
        UserStatManager.Instance.OnLevelUpUpdateStat += OnChangeMP;
        UserStatManager.Instance.OnChangeExpStat += OnChangeExp;
        OnChangeSkillKeyBind();
    }

    public override void SetInitValue()
    {
        Damagable.OnTakeDamage += OnHpChange;
        Damagable.OnChangeHPEvent += OnChangeHP;
        
        UserStatClient userStatClient = UserStatManager.Instance.userStatClient;

        if (hpBar != null)
        {
            hpBar.maxValue = (float)Damagable.MaxHp;
            hpBar.value = Damagable.Hp;

            hpText.text = $"{Damagable.Hp} / {Damagable.MaxHp}";
        }
        if (MpBar != null)
        {
            MpBar.maxValue = userStatClient.MaxMP;
            MpBar.value = userStatClient.MP;

            mpText.text = $"{userStatClient.MP} / {userStatClient.MaxMP}";
        }
        if (ExpBar != null)
        {
            ExpBar.maxValue = userStatClient.MaxExp;
            ExpBar.value = userStatClient.Exp;
            float expTextValue = Mathf.Floor((float)userStatClient.Exp / (float)userStatClient.MaxExp * 1000f) / 1000f;
            expText.text = $"{userStatClient.Exp} / {userStatClient.MaxExp} {expTextValue}%";
        }
    }
    public override void OnHpChange(int damage)
    {
        hpBar.value = Damagable.Hp;
        hpText.text = $"{Damagable.Hp} / {Damagable.MaxHp}";
    }

    private void OnChangeSkillKeyBind()
    {        
        for (int i = 0; i < skillIconList.Count; i++)
        {
            Debug.Log(PlayerSkill.EquipSkills[i]);
            if (PlayerSkill.EquipSkills[i] == 0)
            {
                skillIconList[i].sprite = null;
                skillIconList[i].ImageTransparent(0);

                continue;
            }
          
            skillIconList[i].sprite = PlayerUIManager.Instance.skillIconList[PlayerSkill.EquipSkills[i] - 1];
            skillIconList[i].ImageTransparent(1);
        }
    }
    /// <summary>
    /// 스탯의 변화가 있을 때 사용
    /// </summary>
    private void OnChangeHP()
    {
        Debug.Log($"최대 체력 : {Damagable.MaxHp}, 체력 : {Damagable.Hp}");
        hpText.text = $"{Damagable.Hp} / {Damagable.MaxHp}";
        hpBar.maxValue = Damagable.MaxHp;
        hpBar.value = Damagable.Hp;
    }
    /// <summary>
    /// 마나의 변화가 있을 때 (스킬 시전, 레벨업, 물약 사용)
    /// </summary>
    private void OnChangeMP()
    {        
        UserStatClient userStatClient = UserStatManager.Instance.userStatClient;
        MpBar.maxValue = userStatClient.MaxMP;
        MpBar.value = userStatClient.MP;

        mpText.text = $"{userStatClient.MP} / {userStatClient.MaxMP}";
    }
    /// <summary>
    /// 경험치의 변화가 있을 때 
    /// </summary>
    private void OnChangeExp()
    {        
        UserStatClient userStatClient = UserStatManager.Instance.userStatClient;
        ExpBar.maxValue = userStatClient.MaxExp;
        ExpBar.value = userStatClient.Exp;
        float expTextValue = Mathf.Floor((float)userStatClient.Exp / (float)userStatClient.MaxExp * 1000f) / 1000f;
        expText.text = $"{userStatClient.Exp} / {userStatClient.MaxExp} {expTextValue}%";
    }
}
