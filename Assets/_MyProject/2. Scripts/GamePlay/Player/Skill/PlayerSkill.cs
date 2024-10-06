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
    /// 현재 장착하고 있는 스킬
    /// </summary>
    [HideInInspector] public static List<int> EquipSkills { get; private set; }
    /// <summary>
    /// 전체 스킬 정보
    /// </summary>
    public Dictionary<int, string> skillTable = new Dictionary<int, string>();

    /// <summary>
    /// 유저 스킬의 리소스(스킬 아이콘 이미지 등)을 담는 리스트
    /// </summary>
    public List<SkillResource> skillResourceList;
    /// <summary>
    /// 스킬들의 정보를 담고 있는 리스트
    /// </summary>
    public List<Skill> skillList;

    protected int currentSkillNum;
    private PlayerAnimation playerAnimation;

    /// <summary>
    /// 처음 게임 시작할 때 키셋 초기화가 완료되었을 때,
    /// 키셋에 대한 정보가 필요한 다른 스크립트가
    /// 키셋 초기화 이후에 초기화를 하기 위한 이벤트
    /// </summary>
    public static event Action OnKeyBindInit;

    // 서버에서 스킬을 실행하는 명령(Command)
    [Command]
    public void CmdTriggerSkill(int currentSkillNum, int skillNum)
    {
        RpcPlaySkillEffect(currentSkillNum, skillNum); // 모든 클라이언트에서 스킬 이펙트 실행
    }

    // 모든 클라이언트에서 스킬 이펙트를 실행하는 RPC
    [ClientRpc]
    void RpcPlaySkillEffect(int currentSkillNum, int skillNum)
    {
        SkillResource skillResource = skillResourceList[currentSkillNum];
        skillResource.PlayOnClients(skillNum); // 클라이언트에서 이펙트 생성
    }

    [Command]
    private void CmdSetUserID(int id)
    {
        this.userId = id;  // 서버에서 userID 설정
        Debug.Log($"서버에서 User ID 설정: {userId}");        
    }


    private void Start()
    {
        if (!isLocalPlayer) return;
        playerAnimation = GetComponent<PlayerAnimation>();

        // 로컬 플레이어만 이 코드를 실행
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
        Debug.Log("여기가 먼저???ㄴㅇㄹ");        
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
        Debug.Log($"현재 스킬 번호 : {currentSkillNum}");
    }
    private void OnSkill_2(InputAction.CallbackContext context)
    {
        currentSkillNum = EquipSkills[1];
        if (currentSkillNum == 0)
        {
            return;
        }
        playerAnimation.SkillAnimation($"{skillTable[EquipSkills[1]]}");
        Debug.Log($"현재 스킬 번호 : {currentSkillNum}");
    }
    private void OnSkill_3(InputAction.CallbackContext context)
    {
        currentSkillNum = EquipSkills[2];
        if (currentSkillNum == 0)
        {
            return;
        }
        playerAnimation.SkillAnimation($"{skillTable[EquipSkills[2]]}");
        Debug.Log($"현재 스킬 번호 : {currentSkillNum}");
    }
    private void OnSkill_4(InputAction.CallbackContext context)
    {
        currentSkillNum = EquipSkills[3];
        if (currentSkillNum == 0)
        {
            return;
        }
        playerAnimation.SkillAnimation($"{skillTable[EquipSkills[3]]}");
        Debug.Log($"현재 스킬 번호 : {currentSkillNum}");
    }
    #endregion
}

