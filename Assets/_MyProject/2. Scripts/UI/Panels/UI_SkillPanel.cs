using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class KeySetIcon
{    
    /// <summary>
    /// ��ųâ�� ������ �� ��ų �̹���
    /// </summary>
    public Image ActionBarIcon;
    /// <summary>
    /// Ű�� ���� â�� ���� ��ų �̹���
    /// </summary>
    public Image KeyIcon;    
}
public class UI_SkillPanel : MonoBehaviour
{
    [Header("��ų")]
    public GameObject skillWindowContent;   // ��ųâ
    public GameObject skillInfo;            // �� ��ų���� ��ų ���� �׸�
    public GameObject keySetPanel;          // ��ų�� �����Ϸ��� �� �� ���� Ű�� ���� â
    public List<KeySetIcon> iconPanelList;  // ���� ������ 4���� ��ų �̹����� ������ ����Ʈ
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
    /// ��ų â�� ����
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
    /// ��ų Ű������ �ٲ� ������ ȣ���ϴ� �޼���
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
