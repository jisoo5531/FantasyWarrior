using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerSkill : NetworkBehaviour
{

    private int userId;

    /// <summary>
    /// ���� �����ϰ� �ִ� ��ų
    /// </summary>
    [HideInInspector] public static List<int> EquipSkills { get; private set; }
    /// <summary>
    /// ��ü ��ų ����
    /// </summary>
    public Dictionary<int, string> skillTable = new Dictionary<int, string>();

    /// <summary>
    /// ���� ��ų�� ���ҽ�(��ų ������ �̹��� ��)�� ��� ����Ʈ
    /// </summary>
    public List<SkillResource> skillResourceList;
    /// <summary>
    /// ��ų���� ������ ��� �ִ� ����Ʈ
    /// </summary>
    public List<Skill> skillList;

    protected int currentSkillNum;
    private PlayerAnimation playerAnimation;

    /// <summary>
    /// ó�� ���� ������ �� Ű�� �ʱ�ȭ�� �Ϸ�Ǿ��� ��,
    /// Ű�¿� ���� ������ �ʿ��� �ٸ� ��ũ��Ʈ��
    /// Ű�� �ʱ�ȭ ���Ŀ� �ʱ�ȭ�� �ϱ� ���� �̺�Ʈ
    /// </summary>
    public static event Action OnKeyBindInit;

    // �������� ��ų�� �����ϴ� ���(Command)
    [Command]
    public void CmdTriggerSkill(int currentSkillNum, int skillNum)
    {
        RpcPlaySkillEffect(currentSkillNum, skillNum); // ��� Ŭ���̾�Ʈ���� ��ų ����Ʈ ����
    }

    // ��� Ŭ���̾�Ʈ���� ��ų ����Ʈ�� �����ϴ� RPC
    [ClientRpc]
    void RpcPlaySkillEffect(int currentSkillNum, int skillNum)
    {
        SkillResource skillResource = skillResourceList[currentSkillNum];
        skillResource.PlayOnClients(skillNum); // Ŭ���̾�Ʈ���� ����Ʈ ����
    }

    [Command]
    private void CmdSetUserID(int id)
    {
        this.userId = id;  // �������� userID ����
        Debug.Log($"�������� User ID ����: {userId}");        
    }


    private void Start()
    {
        if (!isLocalPlayer) return;
        playerAnimation = GetComponent<PlayerAnimation>();

        // ���� �÷��̾ �� �ڵ带 ����
        this.userId = DatabaseManager.Instance.GetPlayerData(this.gameObject).UserId;        
        CmdSetUserID(userId);
        Initialize();
        GameManager.inputActions.PlayerActions.Skill_1.performed += OnSkill_1;
        GameManager.inputActions.PlayerActions.Skill_2.performed += OnSkill_2;
        GameManager.inputActions.PlayerActions.Skill_3.performed += OnSkill_3;
        GameManager.inputActions.PlayerActions.Skill_4.performed += OnSkill_4;
    }

    public override void OnStartLocalPlayer()
    {
        
    }

    protected virtual void Initialize()
    {
        Debug.Log("���Ⱑ ����???������");        
        SkillKeyBind userSkillKeyBind = SkillManager.Instance.GetSkillKeyBind(this.userId);        
        EquipSkills = new List<int>
        {
            userSkillKeyBind.Skill_1,
            userSkillKeyBind.Skill_2,
            userSkillKeyBind.Skill_3,
            userSkillKeyBind.Skill_4
        };
        Debug.Log($"{userSkillKeyBind.Skill_1}, {userSkillKeyBind.Skill_2}, {userSkillKeyBind.Skill_3}, {userSkillKeyBind.Skill_4}");
        OnKeyBindInit?.Invoke();

        SkillTableInitialize();
    }
    private void SkillTableInitialize()
    {
        List<SkillData> skillDatas = SkillManager.Instance.ClassSkillDataList;

        if (skillDatas != null)
        {
            for (int i = 0; i < skillDatas.Count; i++)
            {
                SkillData skillData = skillDatas[i];
                skillTable.Add(i + 1, skillData.Skill_Name);
            }
        }
    }

    

    #region Input System
    private void OnSkill_1(InputAction.CallbackContext context)
    {
        currentSkillNum = EquipSkills[0];
        if (currentSkillNum == 0)
        {
            return;
        }
        playerAnimation.SkillAnimation($"{skillTable[EquipSkills[0]]}");
        Debug.Log($"���� ��ų ��ȣ : {currentSkillNum}");
    }
    private void OnSkill_2(InputAction.CallbackContext context)
    {
        currentSkillNum = EquipSkills[1];
        if (currentSkillNum == 0)
        {
            return;
        }
        playerAnimation.SkillAnimation($"{skillTable[EquipSkills[1]]}");
        Debug.Log($"���� ��ų ��ȣ : {currentSkillNum}");
    }
    private void OnSkill_3(InputAction.CallbackContext context)
    {
        currentSkillNum = EquipSkills[2];
        if (currentSkillNum == 0)
        {
            return;
        }
        playerAnimation.SkillAnimation($"{skillTable[EquipSkills[2]]}");
        Debug.Log($"���� ��ų ��ȣ : {currentSkillNum}");
    }
    private void OnSkill_4(InputAction.CallbackContext context)
    {
        currentSkillNum = EquipSkills[3];
        if (currentSkillNum == 0)
        {
            return;
        }
        playerAnimation.SkillAnimation($"{skillTable[EquipSkills[3]]}");
        Debug.Log($"���� ��ų ��ȣ : {currentSkillNum}");
    }
    #endregion
}

