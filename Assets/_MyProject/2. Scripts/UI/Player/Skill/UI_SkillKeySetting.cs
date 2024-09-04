using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillKeySetting : MonoBehaviour
{
    public Button key1_Button;
    public Button key2_Button;
    public Button key3_Button;
    public Button key4_Button;
    

    public int skillNum;

    private void Awake()
    {
        key1_Button.onClick.AddListener(OnClickKey1Button);
        key2_Button.onClick.AddListener(OnClickKey2Button);
        key3_Button.onClick.AddListener(OnClickKey3Button);
        key4_Button.onClick.AddListener(OnClickKey4Button);
    }
    public void Initialize(int skillNum)
    {
        this.skillNum = skillNum;

    }

    public void OnClickKey1Button()
    {
        ChangeSkillSet(0);
        EventHandler.skillKey.TriggetSkillKeyChange(1);
    }    
    public void OnClickKey2Button()
    {
        ChangeSkillSet(1);
        EventHandler.skillKey.TriggetSkillKeyChange(2);
    }
    public void OnClickKey3Button()
    {
        ChangeSkillSet(2);
        EventHandler.skillKey.TriggetSkillKeyChange(3);
    }
    public void OnClickKey4Button()
    {
        ChangeSkillSet(3);
        EventHandler.skillKey.TriggetSkillKeyChange(4);
    }

    private void ChangeSkillSet(int index)
    {
        PlayerSkill.EquipSkills.Remove(index);
        int originIndex = PlayerSkill.EquipSkills.Find((x) => { return x == skillNum; });
        PlayerSkill.EquipSkills.Remove(originIndex);

        PlayerSkill.EquipSkills.Insert(index, skillNum);

        Debug.Log($"{PlayerSkill.EquipSkills[0]},{PlayerSkill.EquipSkills[1]},{PlayerSkill.EquipSkills[2]},{PlayerSkill.EquipSkills[3]}");

        gameObject.SetActive(false);
    }
}
