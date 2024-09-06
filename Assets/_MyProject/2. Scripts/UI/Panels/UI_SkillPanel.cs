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

    private void Awake()
    {
        EventHandler.skillKey.RegisterSkillKeyChange(OnChangeSkill);
    }

    private void Start()
    {        
        for (int i = 0; i < iconPanelList.Count; i++)
        {
            Sprite skillIcon = PlayerUIManager.Instance.skillIconList[PlayerSkill.EquipSkills[i] - 1];
            iconPanelList[i].ActionBarIcon.sprite = skillIcon;
            iconPanelList[i].KeyIcon.sprite = skillIcon;            
        }
        InitSkillWindow();        
    }
    private void InitSkillWindow()
    {
        List<SkillData> skills = SkillManager.Instance.userSkillDataList;
        
        if (skills != null)
        {
            foreach (SkillData skillData in skills)
            {
                GameObject skillInfoObj = Instantiate(skillInfo, skillWindowContent.transform);
                UI_SkillEntry skillEntry = skillInfoObj.GetComponent<UI_SkillEntry>();

                // TODO : 직업 추가 시, tableName 바꾸기                            
                skillEntry.Initialize(skillData, keySetPanel);
            }
        }        
    }

    private void OnChangeSkill()
    {
        for (int i = 0; i < iconPanelList.Count; i++)
        {
            if (PlayerSkill.EquipSkills[i] == 0)
            {
                iconPanelList[i].ActionBarIcon.sprite = null;                           
                iconPanelList[i].KeyIcon.sprite = null;
                iconPanelList[i].ActionBarIcon.ImageTransparent(0);                
                iconPanelList[i].KeyIcon.ImageTransparent(0);

                continue;
            }

            iconPanelList[i].ActionBarIcon.sprite = PlayerUIManager.Instance.skillIconList[PlayerSkill.EquipSkills[i] - 1];
            iconPanelList[i].KeyIcon.sprite = PlayerUIManager.Instance.skillIconList[PlayerSkill.EquipSkills[i] - 1];            
            iconPanelList[i].ActionBarIcon.ImageTransparent(1);
            iconPanelList[i].KeyIcon.ImageTransparent(1);
        }        
    }
}
