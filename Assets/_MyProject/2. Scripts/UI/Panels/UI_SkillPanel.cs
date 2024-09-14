using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class KeySetIcon
{    
    /// <summary>
    /// 스킬창에 보여질 각 스킬 이미지
    /// </summary>
    public Image ActionBarIcon;
    /// <summary>
    /// 키셋 변경 창에 보일 스킬 이미지
    /// </summary>
    public Image KeyIcon;    
}
public class UI_SkillPanel : MonoBehaviour
{
    [Header("스킬")]
    public GameObject skillWindowContent;   // 스킬창
    public GameObject skillInfo;            // 각 스킬들의 스킬 정보 항목
    public GameObject keySetPanel;          // 스킬을 장착하려고 할 때 쓰일 키셋 변경 창
    public List<KeySetIcon> iconPanelList;  // 현재 장착한 4개의 스킬 이미지를 관리할 리스트
    public List<SkillData> skillDataList = new List<SkillData>();    

    private void Awake()
    {
        EventHandler.skillKey.RegisterSkillKeyChange(OnChangeSkill);
    }
    private void SkillContentClear(GameObject content)
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);            
        }
    }
    public void SkillPanelInit()
    {        
        SkillManager.Instance.OnUnlockSkillEvent += SkillWindowSetting;

        for (int i = 0; i < iconPanelList.Count; i++)
        {
            Sprite skillIcon = PlayerUIManager.Instance.skillIconList[PlayerSkill.EquipSkills[i] - 1];
            iconPanelList[i].ActionBarIcon.sprite = skillIcon;
            iconPanelList[i].KeyIcon.sprite = skillIcon;
        }
        SkillWindowSetting();
    }
    /// <summary>
    /// 스킬 창에 세팅
    /// </summary>
    private void SkillWindowSetting()
    {
        List<SkillData> skills = SkillManager.Instance.ClassSkillDataList;
        
        SkillContentClear(skillWindowContent);
        if (skills != null)
        {
            foreach (SkillData skillData in skills)
            {
                GameObject skillInfoObj = Instantiate(skillInfo, skillWindowContent.transform);
                UI_SkillEntry skillEntry = skillInfoObj.GetComponent<UI_SkillEntry>();
                                       
                skillEntry.Initialize(skillData, keySetPanel);
            }
        }        
    }

    /// <summary>
    /// 스킬 키세팅을 바꿀 때마다 호출하는 메서드
    /// </summary>
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
