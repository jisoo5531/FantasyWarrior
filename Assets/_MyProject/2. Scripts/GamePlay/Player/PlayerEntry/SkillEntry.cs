using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillEntry : MonoBehaviour
{
    public Image skillIcon;
    public TMP_Text skillName;
    public TMP_Dropdown skillDropdown;

    public SkillData skillData;
    
    [HideInInspector] public string tableName;

    private void Awake()
    {
        var skillOptions = new List<TMP_Dropdown.OptionData>();

        foreach (string skillKey in Enum.GetNames(typeof(Skill.Skill_Key)))
        {
            skillOptions.Add(new TMP_Dropdown.OptionData(skillKey));
        }
        skillDropdown.options = skillOptions;

        skillDropdown.onValueChanged.AddListener(SkillChange);

    }
    public void Initialize(SkillData skillData, string tableName)
    {
        this.skillData = skillData;        
        skillIcon.sprite = Resources.Load<Sprite>($"{tableName}/{skillData.Icon_Name}");        
        skillName.text = skillData.Skill_Name;
    }
    public void SkillChange(int num)
    {
        PlayerSkill.equipSkills.RemoveAt(num);
        PlayerSkill.equipSkills.Insert(num, skillData.Skill_Order);

    }
}
