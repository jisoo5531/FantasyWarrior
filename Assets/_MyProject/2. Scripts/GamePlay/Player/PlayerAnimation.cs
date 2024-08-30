using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    public Animator anim;
    private bool isRun = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        PlayerController.inputActions.PlayerActions.Move.performed += MoveAnimation;
        PlayerController.inputActions.PlayerActions.Move.canceled += MoveAnimation;

        PlayerController.inputActions.PlayerActions.Run.performed += OnRunAction;
        PlayerController.inputActions.PlayerActions.Run.canceled += OnRunAction;
        
    }
    private void OnDisable()
    {
        PlayerController.inputActions.PlayerActions.Move.performed -= MoveAnimation;
        PlayerController.inputActions.PlayerActions.Move.canceled -= MoveAnimation;

        PlayerController.inputActions.PlayerActions.Run.performed -= OnRunAction;
        PlayerController.inputActions.PlayerActions.Run.canceled -= OnRunAction;
    }

    public void MoveAnimation(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();
        float speed = Mathf.Max(Mathf.Abs(move.x), Mathf.Abs(move.y));
        speed = isRun ? speed * 2 : speed;        
        anim.SetFloat("Speed", speed);
    }
    public void OnRunAction(InputAction.CallbackContext context)
    {
        isRun = context.ReadValueAsButton();
    }

    public void AttackAnimation()
    {
        anim.SetTrigger("Attack");
    }
    public void SkillAnimation(List<bool> skillInput, Dictionary<int, string> skillTable)
    {
        int skillNum = skillInput.IndexOf(true);
        if (skillNum < 0)
        {
            return;
        }
        Debug.Log($"{skillTable[skillNum]} »ç¿ë");
        anim.SetTrigger($"{skillTable[skillNum]}"); 
    }
    public void DeathAnimation()
    {
        anim.SetTrigger("Death");
    }
}
