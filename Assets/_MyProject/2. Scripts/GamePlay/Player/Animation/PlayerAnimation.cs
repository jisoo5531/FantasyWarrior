using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerAnimation : NetworkBehaviour
{
    [HideInInspector] public Animator anim;
    private bool isRun = false;
    public Dictionary<int, string> skillTable;
    public List<int> equipSkills;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public override void OnStartLocalPlayer()
    {
        

        // NetworkAnimator ������Ʈ�� �ִϸ����� ����
        GetComponent<NetworkAnimator>().animator = anim;

        PlayerSkill skill = GetComponent<PlayerSkill>();
        skillTable = skill.skillTable;
        equipSkills = PlayerSkill.EquipSkills;
    }

    private void OnEnable()
    {
        GameManager.inputActions.PlayerActions.Move.performed += MoveAnimation;
        GameManager.inputActions.PlayerActions.Move.canceled += MoveAnimation;
        GameManager.inputActions.PlayerActions.Run.performed += OnRunAction;
        GameManager.inputActions.PlayerActions.Run.canceled += OnRunAction;

        GameManager.inputActions.PlayerActions.Attack.performed += AttackAnimation;
    }

    private void OnDisable()
    {
        GameManager.inputActions.PlayerActions.Move.performed -= MoveAnimation;
        GameManager.inputActions.PlayerActions.Move.canceled -= MoveAnimation;
        GameManager.inputActions.PlayerActions.Run.performed -= OnRunAction;
        GameManager.inputActions.PlayerActions.Run.canceled -= OnRunAction;

        GameManager.inputActions.PlayerActions.Attack.performed -= AttackAnimation;
    }

    #region Move
    // Ŭ���̾�Ʈ�� �����̸� ������ �ִϸ��̼� ����ȭ�� ��û
    public void MoveAnimation(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();
        float speed = Mathf.Max(Mathf.Abs(move.x), Mathf.Abs(move.y));
        speed = isRun ? speed * 2 : speed;
        CmdMoveAnimation(speed);
    }

    // �������� ��� Ŭ���̾�Ʈ�� �̵� �ִϸ��̼� ����ȭ
    [Command]
    public void CmdMoveAnimation(float speed)
    {
        RpcMoveAnimation(speed);
    }

    [ClientRpc]
    public void RpcMoveAnimation(float speed)
    {
        anim.SetFloat("Speed", speed);
    }

    public void OnRunAction(InputAction.CallbackContext context)
    {
        isRun = context.ReadValueAsButton();
    }
    #endregion

    #region Skill
    // Ŭ���̾�Ʈ���� ��ų �ִϸ��̼� ��û
    public void SkillAnimation(string skillName)
    {
        CmdSkillAnimation(skillName);
    }

    // �������� ��� Ŭ���̾�Ʈ�� ��ų �ִϸ��̼� ����ȭ
    [Command]
    public void CmdSkillAnimation(string skillName)
    {
        RpcSkillAnimation(skillName);
    }

    [ClientRpc]
    public void RpcSkillAnimation(string skillName)
    {
        anim.SetTrigger(skillName);
    }
    #endregion

    #region Attack
    // Ŭ���̾�Ʈ���� ���� �ִϸ��̼� ��û
    public void AttackAnimation(InputAction.CallbackContext context)
    {
        CmdAttackAnimation();
    }

    // �������� ��� Ŭ���̾�Ʈ�� ���� �ִϸ��̼� ����ȭ
    [Command]
    public void CmdAttackAnimation()
    {
        RpcAttackAnimation();
    }

    [ClientRpc]
    public void RpcAttackAnimation()
    {
        anim.SetTrigger("Attack");
    }
    #endregion

    #region Death
    // Ŭ���̾�Ʈ���� ���� �ִϸ��̼� ��û
    public void DeathAnimation()
    {
        CmdDeathAnimation();
    }

    // �������� ��� Ŭ���̾�Ʈ�� ���� �ִϸ��̼� ����ȭ
    [Command]
    public void CmdDeathAnimation()
    {
        RpcDeathAnimation();
    }

    [ClientRpc]
    public void RpcDeathAnimation()
    {
        anim.SetTrigger("Death");
    }
    #endregion
}
