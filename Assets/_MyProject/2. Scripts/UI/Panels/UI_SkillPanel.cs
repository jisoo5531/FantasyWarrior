using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

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

    public KeySetIcon(Image actionBarIcon, Image keyIcon)
    {
        this.ActionBarIcon = actionBarIcon;
        this.KeyIcon = keyIcon;
    }
}
public class UI_SkillPanel : MonoBehaviour
{
    [Header("��ų")]
    public GameObject skillWindowContent;   // ��ųâ
    public GameObject skillInfoPrefab;            // �� ��ų���� ��ų ���� �׸�
    public GameObject keySetPanel;          // ��ų�� �����Ϸ��� �� �� ���� Ű�� ���� â    
    public List<KeySetIcon> iconPanelList;  // ���� ������ 4���� ��ų �̹����� ������ ����Ʈ
    public List<SkillData> skillDataList = new List<SkillData>();
    [Header("��ų ������")]
    public GameObject SkillInfoWindow;

    private void Awake()
    {
        // ���� �÷��̾� üũ
        NetworkIdentity networkIdentity = transform.root.GetComponent<NetworkIdentity>();
        if (networkIdentity == null || !networkIdentity.isLocalPlayer)
        {
            return; // ���� �÷��̾ �ƴ� ��� UI�� �������� ����
        }
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
        // ���� �÷��̾� üũ
        NetworkIdentity networkIdentity = transform.root.GetComponent<NetworkIdentity>();
        if (networkIdentity == null || !networkIdentity.isLocalPlayer)
        {
            return; // ���� �÷��̾ �ƴ� ��� UI�� �������� ����
        }
        SkillManager.Instance.OnUnlockSkillEvent += SkillWindowSetting;

        for (int i = 0; i < iconPanelList.Count; i++)
        {
            // ������ ��ų�� ���ٸ�
            if (PlayerSkill.EquipSkills[i] == 0)
            {
                continue;
            }            
            Sprite skillIcon = PlayerUIManager.Instance.skillIconList[PlayerSkill.EquipSkills[i] - 1];            
            iconPanelList[i].ActionBarIcon.sprite = skillIcon;
            iconPanelList[i].KeyIcon.sprite = skillIcon;
            iconPanelList[i].ActionBarIcon.ImageTransparent(1);
            iconPanelList[i].KeyIcon.ImageTransparent(1);            
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
                GameObject skillInfoObj = Instantiate(skillInfoPrefab, skillWindowContent.transform);
                UI_SkillEntry skillEntry = skillInfoObj.GetComponent<UI_SkillEntry>();
                                       
                // �� ��ų���� ��� �ִ� �׸�
                skillEntry.Initialize(skillData, keySetPanel, SkillInfoWindow);
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
    private void Reset()
    {
        skillWindowContent = GameObject.Find("SkillWindowContent");
        keySetPanel = GameObject.Find("SkillKeySet");

        //iconPanelList = new List<KeySetIcon>();
        //for (int i = 1; i <= 4; i++)
        //{
        //    KeySetIcon keySetIcon = new KeySetIcon
        //    (
        //    GameObject.Find($"ActionBar ICON {i}").GetComponent<Image>(),
        //    GameObject.Find($"KeySet ICON {i}").GetComponent<Image>()
        //    );
        //    iconPanelList.Add(keySetIcon);
        //}
    }
}
