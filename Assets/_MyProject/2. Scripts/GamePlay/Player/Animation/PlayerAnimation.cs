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
        

        // NetworkAnimator 컴포넌트의 애니메이터 설정
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
    // 클라이언트가 움직이면 서버로 애니메이션 동기화를 요청
    public void MoveAnimation(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();
        float speed = Mathf.Max(Mathf.Abs(move.x), Mathf.Abs(move.y));
        speed = isRun ? speed * 2 : speed;
        CmdMoveAnimation(speed);
    }

    // 서버에서 모든 클라이언트로 이동 애니메이션 동기화
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
    // 클라이언트에서 스킬 애니메이션 요청
    public void SkillAnimation(string skillName)
    {
        CmdSkillAnimation(skillName);
    }

    // 서버에서 모든 클라이언트로 스킬 애니메이션 동기화
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
    // 클라이언트에서 공격 애니메이션 요청
    public void AttackAnimation(InputAction.CallbackContext context)
    {
        CmdAttackAnimation();
    }

    // 서버에서 모든 클라이언트로 공격 애니메이션 동기화
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
    // 클라이언트에서 죽음 애니메이션 요청
    public void DeathAnimation()
    {
        CmdDeathAnimation();
    }

    // 서버에서 모든 클라이언트로 죽음 애니메이션 동기화
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
