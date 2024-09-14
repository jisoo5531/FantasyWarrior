using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_SkillEntry : MonoBehaviour
{
    /// <summary>
    /// 스킬을 나타낼 이미지
    /// </summary>
    public Image skillIcon;
    
    public TMP_Text skillName;
    /// <summary>
    /// 스킬을 장착하기 위한 버튼
    /// </summary>
    public Button equipButton;
    public Button learnSkillButton;
    // TODO : 임시 스킬 레벨업 되는지 테스트
    public Button skillLevelUPTestButton;
    /// <summary>
    /// 아직 스킬 잠금을 해제하지 못했을 때 가려지기 위한 이미지
    /// </summary>
    public Image LockImage;

    public SkillData skillData;

    [HideInInspector] public string tableName;

    /// <summary>
    /// 스킬을 어떤 키에 세팅할 것인지를 나타낼 패널
    /// </summary>
    private GameObject keySetPanel;

    /// <summary>
    /// 현재 스킬이 잠금 상태인지 (해제 조건이 안 풀린 상태)
    /// </summary>
    private bool isLockSkill = true;

    private void Awake()
    {        
        equipButton.onClick.AddListener(() =>
            {
                keySetPanel.gameObject.SetActive(true);
                keySetPanel.GetComponent<UI_SkillKeySetting>().Initialize(skillData.Skill_Order);
            });
        learnSkillButton.onClick.AddListener(OnCLickLearnSkillButton);
        skillLevelUPTestButton.onClick.AddListener(OnLevelUpSkillButton);
    }
    private void Start()
    {
        CheckSkillLearned();
        EventHandler.playerEvent.RegisterPlayerLevelUp(OnLevelUp_UnlockSkill);        
    }
    private void OnDisable()
    {
        EventHandler.playerEvent.UnRegisterPlayerLevelUp(OnLevelUp_UnlockSkill);
    }
    public void Initialize(SkillData skillData, GameObject keySetPanel)
    {        
        UserStatClient userStatClient = UserStatManager.Instance.userStatClient;
        this.skillData = skillData;
        this.keySetPanel = keySetPanel;

        OnLevelUp_UnlockSkill();

        string folderName = $"{userStatClient.charClass.ToString()}_Skills";
        skillIcon.sprite = Resources.Load<Sprite>($"{folderName}/{skillData.Icon_Name}");
        skillName.text = skillData.Skill_Name;        
    }

    /// <summary>
    /// 레벨업에 따라 해당 스킬 잠금 해제
    /// </summary>
    private void OnLevelUp_UnlockSkill()
    {               
        List<SkillData> availableSkillList = SkillManager.Instance.userAvailableSkillList;
        if (availableSkillList != null)
        {
            SkillData skill = availableSkillList.Find(x => x.Equals(this.skillData));
            isLockSkill = skill == null;
            LockImage.gameObject.SetActive(isLockSkill);                 
        }        
    }
    /// <summary>
    /// 스킬을 유저가 이미 배웠는지 확인
    /// </summary>
    private void CheckSkillLearned()
    {
        List<UserSkillData> userSkillList = SkillManager.Instance.UserSkillList;
        if (userSkillList != null)
        {
            UserSkillData userSkill = userSkillList.Find(x => x.Skill_ID.Equals(this.skillData.Skill_ID));
            bool isSkillLearned = userSkill != null;
            learnSkillButton.gameObject.SetActive(false == isSkillLearned);
            equipButton.interactable = false == isLockSkill;
            learnSkillButton.interactable = false == isLockSkill;
        }
    }
    /// <summary>
    /// 스킬 레벨업 버튼을 누르면
    /// </summary>
    private void OnLevelUpSkillButton()
    {
        SkillManager.Instance.SkillLevelUP(this.skillData);
    }
    /// <summary>
    /// 스킬 배우기 버튼을 누르면
    /// </summary>
    private void OnCLickLearnSkillButton()
    {
        SkillManager.Instance.LearnSkill(this.skillData);
        CheckSkillLearned();
    }
}
