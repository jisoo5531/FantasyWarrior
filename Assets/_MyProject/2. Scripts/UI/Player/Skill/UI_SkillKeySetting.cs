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
        PlayerSkill.equipSkills.Remove(0);
        PlayerSkill.equipSkills.Insert(0, skillNum);
        gameObject.SetActive(false);
    }
    public void OnClickKey2Button()
    {
        PlayerSkill.equipSkills.Remove(1);
        PlayerSkill.equipSkills.Insert(1, skillNum);
        gameObject.SetActive(false);
    }
    public void OnClickKey3Button()
    {
        PlayerSkill.equipSkills.Remove(2);
        PlayerSkill.equipSkills.Insert(2, skillNum);
        gameObject.SetActive(false);
    }
    public void OnClickKey4Button()
    {
        PlayerSkill.equipSkills.Remove(3);
        PlayerSkill.equipSkills.Insert(3, skillNum);
        gameObject.SetActive(false);
    }
}
