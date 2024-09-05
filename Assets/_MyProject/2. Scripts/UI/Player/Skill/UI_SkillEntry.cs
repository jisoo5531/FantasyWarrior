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
    public void Initialize(SkillData skillData, GameObject keySetPanel)
    {
        UserData userData = DatabaseManager.Instance.userData;      
        
        bool isLock = false == SkillManager.Instance.userAvailableSkillList.Exists((x) => { return x.Skill_ID.Equals(skillData.Skill_ID); });

        if (isLock)
        {
            LockImage.gameObject.SetActive(true);
        }

        this.skillData = skillData;
        this.keySetPanel = keySetPanel;
        string folderName = $"{userData.CharClass.ToString()}_Skills";
        Debug.Log(folderName);
        skillIcon.sprite = Resources.Load<Sprite>($"{folderName}/{skillData.Icon_Name}");        
        skillName.text = skillData.Skill_Name;
    }
}
