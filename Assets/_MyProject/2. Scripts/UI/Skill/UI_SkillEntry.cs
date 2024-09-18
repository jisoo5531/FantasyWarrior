using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_SkillEntry : MonoBehaviour
{
    /// <summary>
    /// ��ų�� ��Ÿ�� �̹���
    /// </summary>
    public Image skillIcon;
    
    public TMP_Text skillName;
    /// <summary>
    /// ��ų�� �����ϱ� ���� ��ư
    /// </summary>
    public Button equipButton;
    public Button learnSkillButton;
    // TODO : �ӽ� ��ų ������ �Ǵ��� �׽�Ʈ
    public Button skillLevelUPTestButton;
    /// <summary>
    /// ���� ��ų ����� �������� ������ �� �������� ���� �̹���
    /// </summary>
    public Image LockImage;
    /// <summary>
    /// ��ų���� �������� �����ִ� ������ â
    /// </summary>
    private GameObject SkillInfoWindow;

    public SkillData skillData;

    [HideInInspector] public string tableName;

    /// <summary>
    /// ��ų�� � Ű�� ������ �������� ��Ÿ�� �г�
    /// </summary>
    private GameObject keySetPanel;

    /// <summary>
    /// ���� ��ų�� ��� �������� (���� ������ �� Ǯ�� ����)
    /// </summary>
    private bool isLockSkill = true;

    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClickSkillElement);
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
        EventHandler.playerEvent.RegisterPlayerLevelUp(OnCheckSkillUnlock);        
    }
    private void OnDisable()
    {
        EventHandler.playerEvent.UnRegisterPlayerLevelUp(OnCheckSkillUnlock);
    }
    public void Initialize(SkillData skillData, GameObject keySetPanel, GameObject skillInfoWindow)
    {                
        UserStatClient userStatClient = UserStatManager.Instance.userStatClient;
        this.skillData = skillData;
        this.keySetPanel = keySetPanel;
        this.SkillInfoWindow = skillInfoWindow;

        OnCheckSkillUnlock();

        string folderName = $"{userStatClient.charClass.ToString()}_Skills";
        skillIcon.sprite = Resources.Load<Sprite>($"{folderName}/{skillData.Icon_Name}");
        skillName.text = skillData.Skill_Name;        
    }

    /// <summary>
    /// ��ų ��� �� �ִ� �������� Ȯ��
    /// </summary>
    private void OnCheckSkillUnlock()
    {               
        List<SkillData> availableSkillList = SkillManager.Instance.userAvailableSkillList;
        if (availableSkillList != null)
        {
            SkillData skill = availableSkillList.Find(x => x.Equals(this.skillData));
            isLockSkill = skill == null;
            Debug.Log($"���� ��ų�� ���? {isLockSkill}");
            LockImage.gameObject.SetActive(isLockSkill);                 
        }        
    }
    
    /// <summary>
    /// ��ų�� ������ �̹� ������� Ȯ��
    /// </summary>
    private void CheckSkillLearned()
    {
        List<UserSkillData> userSkillList = SkillManager.Instance.UserSkillList;
        if (userSkillList != null)
        {
            UserSkillData userSkill = userSkillList.Find(x => x.Skill_ID.Equals(this.skillData.Skill_ID));
            bool isSkillLearned = userSkill != null;
            learnSkillButton.gameObject.SetActive(false == isSkillLearned);
            equipButton.gameObject.SetActive(isSkillLearned);
            skillLevelUPTestButton.gameObject.SetActive(isSkillLearned);
            equipButton.interactable = false == isLockSkill;
            learnSkillButton.interactable = false == isLockSkill;
        }
    }
    /// <summary>
    /// ��ų ������ ��ư�� ������
    /// </summary>
    private void OnLevelUpSkillButton()
    {
        SkillManager.Instance.SkillLevelUP(this.skillData);
    }
    /// <summary>
    /// ��ų ���� ��ư�� ������
    /// </summary>
    private void OnCLickLearnSkillButton()
    {
        SkillManager.Instance.LearnSkill(this.skillData);
        OnCheckSkillUnlock();
        CheckSkillLearned();
    }
    /// <summary>
    /// ��ų �׸� (�ڱ� �ڽ�)�� ������ ��ų ������ â�� �����Բ�
    /// </summary>
    private void OnClickSkillElement()
    {
        SkillInfoWindow.gameObject.SetActive(true);
        SkillInfoWindow.GetComponent<UI_SkillInfoWindow>().Initialize(skillData);
    }
}
