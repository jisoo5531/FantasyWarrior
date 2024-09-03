using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_SkillEntry : MonoBehaviour
{
    public Image skillIcon;
    public TMP_Text skillName;
    public Button equipButton;    

    public SkillData skillData;
    
    [HideInInspector] public string tableName;

    private GameObject keySetPanel;

    private void Awake()
    {
        equipButton.onClick.AddListener(() => 
            { 
                keySetPanel.gameObject.SetActive(true);
                keySetPanel.GetComponent<UI_SkillKeySetting>().Initialize(skillData.Skill_Order);
            });

        //var skillOptions = new List<TMP_Dropdown.OptionData>();

        //foreach (string skillKey in Enum.GetNames(typeof(Skill.Skill_Key)))
        //{
        //    skillOptions.Add(new TMP_Dropdown.OptionData(skillKey));
        //}
        //skillDropdown.options = skillOptions;

        //skillDropdown.onValueChanged.AddListener(SkillChange);

    }
    public void Initialize(SkillData skillData, string tableName, GameObject keySetPanel)
    {
        this.skillData = skillData;
        this.keySetPanel = keySetPanel;
        skillIcon.sprite = Resources.Load<Sprite>($"{tableName}/{skillData.Icon_Name}");        
        skillName.text = skillData.Skill_Name;
    }
    public void SkillChange(int num)
    {
        PlayerSkill.equipSkills.RemoveAt(num);
        PlayerSkill.equipSkills.Insert(num, skillData.Skill_Order);

    }
}
