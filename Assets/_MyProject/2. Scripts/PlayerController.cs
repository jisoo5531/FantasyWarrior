using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private PlayerInput playerInput = new();
    private PlayerMovement playerMovement = new();
    private PlayerAnimation playerAnimation;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerAnimation = new PlayerAnimation(GetComponent<Animator>());
    }

    private void OnEnable()
    {
        playerInput.OnAttack += playerAnimation.AttackAnimation;
    }

    private void Update()
    {
        playerInput?.Keyinput();
        
        float moveValue = Mathf.Max(Mathf.Abs(playerInput.Vertical), Mathf.Abs(playerInput.Horizontal));
        playerAnimation?.MoveAnimation(moveValue, playerInput.IsRun);
    }
    
    private void FixedUpdate()
    {
        playerMovement?.Move(transform, controller, playerInput.Horizontal, playerInput.Vertical, playerInput.IsRun);
    }

    private void OnDisable()
    {        
        playerInput.OnAttack -= playerAnimation.AttackAnimation;
    }
    private void OnDestroy()
    {
        playerInput.OnAttack -= playerAnimation.AttackAnimation;
    }
}

public class PlayerInput
{
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public bool IsRun { get; private set; }
    public bool IsAttack { get; private set; }

    public event Action OnAttack;

    public void Keyinput()
    {
        // 이동 입력
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");
        IsRun = Input.GetButton("Run");
        IsAttack = Input.GetButton("Fire1");

        InputAction();
    }

    private void InputAction()
    {
        if (IsAttack)
        {
            OnAttack?.Invoke();
        }
    }
}
public class PlayerMovement
{
    private float initSpeed = 0f;
    private float walkSpeed = 6f;
    private float runSpeed = 10f;
    private float gravity = -9.81f;
    private float jumpHeight = 1.5f;
    private float rotationSpeed = 100f; // 회전 속도
    private Vector3 moveDir;
    private Vector3 velocity;
    private bool isGrounded;
    private CharacterController controller;


    public void Move(Transform unitTrans, CharacterController controller, float horizontal, float vertical, bool isRun)
    {
        this.controller = controller;
        CheckGrounded();

        moveDir = new Vector3(horizontal, 0, vertical);

        float moveSpeed = initSpeed;

        if (horizontal != 0 || vertical != 0)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, Time.deltaTime * 30f);
            controller.Move(isRun ? moveDir * runSpeed * Time.deltaTime : moveDir * moveSpeed * Time.deltaTime);
            Rotate(unitTrans, moveDir);
        }                        

        // 중력 적용
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Rotate(Transform unitTrans, Vector3 direction)
    {
        unitTrans.rotation = Quaternion.Lerp(unitTrans.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);        
    }

    private void CheckGrounded()
    {
        // 땅에 닿아 있는지 확인
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 땅에 있을 때의 속도 초기화
        }
    }
}
public class PlayerAnimation
{    
    public Animator anim;

    public PlayerAnimation(Animator anim)
    {        
        this.anim = anim;
    }

    public void MoveAnimation(float speed, bool isRun)
    {
        speed = isRun ? speed * 2 : speed;
        anim.SetFloat("Speed", speed);
    }

    public void AttackAnimation()
    {        
        anim.SetTrigger("Attack");
    }    
}