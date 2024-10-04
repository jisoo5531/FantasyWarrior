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
    /// 스킬창에 보여질 각 스킬 이미지
    /// </summary>
    public Image ActionBarIcon;
    /// <summary>
    /// 키셋 변경 창에 보일 스킬 이미지
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
    [Header("스킬")]
    public GameObject skillWindowContent;   // 스킬창
    public GameObject skillInfoPrefab;            // 각 스킬들의 스킬 정보 항목
    public GameObject keySetPanel;          // 스킬을 장착하려고 할 때 쓰일 키셋 변경 창    
    public List<KeySetIcon> iconPanelList;  // 현재 장착한 4개의 스킬 이미지를 관리할 리스트
    public List<SkillData> skillDataList = new List<SkillData>();
    [Header("스킬 상세정보")]
    public GameObject SkillInfoWindow;

    private void Awake()
    {
        // 로컬 플레이어 체크
        NetworkIdentity networkIdentity = transform.root.GetComponent<NetworkIdentity>();
        if (networkIdentity == null || !networkIdentity.isLocalPlayer)
        {
            return; // 로컬 플레이어가 아닐 경우 UI를 실행하지 않음
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
        // 로컬 플레이어 체크
        NetworkIdentity networkIdentity = transform.root.GetComponent<NetworkIdentity>();
        if (networkIdentity == null || !networkIdentity.isLocalPlayer)
        {
            return; // 로컬 플레이어가 아닐 경우 UI를 실행하지 않음
        }
        SkillManager.Instance.OnUnlockSkillEvent += SkillWindowSetting;

        for (int i = 0; i < iconPanelList.Count; i++)
        {
            // 장착한 스킬이 없다면
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
                GameObject skillInfoObj = Instantiate(skillInfoPrefab, skillWindowContent.transform);
                UI_SkillEntry skillEntry = skillInfoObj.GetComponent<UI_SkillEntry>();
                                       
                // 각 스킬들을 담고 있는 항목
                skillEntry.Initialize(skillData, keySetPanel, SkillInfoWindow);
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
