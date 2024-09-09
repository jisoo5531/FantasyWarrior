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
    public Image LockImage;

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

        
    }
    private void Start()
    {
        EventHandler.playerEvent.RegisterPlayerLevelUp(OnLevelUp_UnlockSkill);
    }
    private void OnDisable()
    {
        EventHandler.playerEvent.UnRegisterPlayerLevelUp(OnLevelUp_UnlockSkill);
    }
    public void Initialize(SkillData skillData, GameObject keySetPanel)
    {
        UserStatData userStatData = UserStatManager.Instance.GetUserStatDataFromDB();        
        this.skillData = skillData;
        this.keySetPanel = keySetPanel;

        OnLevelUp_UnlockSkill();

        string folderName = $"{userStatData.CharClass.ToString()}_Skills";
        skillIcon.sprite = Resources.Load<Sprite>($"{folderName}/{skillData.Icon_Name}");
        skillName.text = skillData.Skill_Name;        
    }

    private void OnLevelUp_UnlockSkill()
    {        
        if (SkillManager.Instance.userAvailableSkillList != null)
        {
            bool isLock = false == SkillManager.Instance.userAvailableSkillList.Exists((x) => { return x.Skill_ID.Equals(skillData.Skill_ID); });

            LockImage.gameObject.SetActive(isLock);
        }        
    }
}
