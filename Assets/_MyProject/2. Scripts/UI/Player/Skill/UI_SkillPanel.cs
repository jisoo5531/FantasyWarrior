using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class KeySetIcon
{    
    public Image ActionBarIcon;
    public Image KeyIcon;
}
public class UI_SkillPanel : MonoBehaviour
{
    [Header("스킬")]
    public GameObject skillWindowContent;
    public GameObject skillInfo;
    public GameObject keySetPanel;
    public List<KeySetIcon> iconPanelList;
    public List<SkillData> skillDataList = new List<SkillData>();

    private string tableName = "warrior_skills";

    private void Awake()
    {
        EventHandler.skillKey.RegisterSkillKeyChange(OnChangeSkill_1, 1);
        EventHandler.skillKey.RegisterSkillKeyChange(OnChangeSkill_2, 2);
        EventHandler.skillKey.RegisterSkillKeyChange(OnChangeSkill_3, 3);
        EventHandler.skillKey.RegisterSkillKeyChange(OnChangeSkill_4, 4);
    }

    private void Start()
    {
        //GetSkillFromDatabaseData();
        for (int i = 0; i < iconPanelList.Count; i++)
        {
            Sprite skillIcon = UIManager.Instance.skillIconList[PlayerSkill.EquipSkills[i] - 1];
            iconPanelList[i].ActionBarIcon.sprite = skillIcon;
            iconPanelList[i].KeyIcon.sprite = skillIcon;
        }
        InitSkillWindow();        
    }
    private void InitSkillWindow()
    {
        if (GameManager.Instance.userSkillDataList != null)
        {
            foreach (SkillData skillData in GameManager.Instance.userSkillDataList)
            {
                GameObject skillInfoObj = Instantiate(skillInfo, skillWindowContent.transform);
                UI_SkillEntry skillEntry = skillInfoObj.GetComponent<UI_SkillEntry>();

                // TODO : 직업 추가 시, tableName 바꾸기            
                skillEntry.Initialize(skillData, this.tableName, keySetPanel);
            }
        }        
    }

    private void OnChangeSkill_1()
    {
        iconPanelList[0].ActionBarIcon.sprite = UIManager.Instance.skillIconList[PlayerSkill.EquipSkills[0]];
        iconPanelList[0].KeyIcon.sprite = UIManager.Instance.skillIconList[PlayerSkill.EquipSkills[0]];
    }
    private void OnChangeSkill_2()
    {
        iconPanelList[1].ActionBarIcon.sprite = UIManager.Instance.skillIconList[PlayerSkill.EquipSkills[1]];
        iconPanelList[1].KeyIcon.sprite = UIManager.Instance.skillIconList[PlayerSkill.EquipSkills[1]];
    }
    private void OnChangeSkill_3()
    {
        iconPanelList[2].ActionBarIcon.sprite = UIManager.Instance.skillIconList[PlayerSkill.EquipSkills[2]];
        iconPanelList[2].KeyIcon.sprite = UIManager.Instance.skillIconList[PlayerSkill.EquipSkills[2]];
    }
    private void OnChangeSkill_4()
    {
        iconPanelList[3].ActionBarIcon.sprite = UIManager.Instance.skillIconList[PlayerSkill.EquipSkills[3]];
        iconPanelList[3].KeyIcon.sprite = UIManager.Instance.skillIconList[PlayerSkill.EquipSkills[3]];
    }
}
